using DeploymentToolkit.Messaging.Events;
using DeploymentToolkit.Messaging.Messages;
using DeploymentToolkit.Modals;
using DeploymentToolkit.ToolkitEnvironment;
using System;
using System.Threading;

namespace DeploymentToolkit.Messaging
{
    internal class TimeoutManager : IDisposable
    {
        // Time in miliseconds
        private const int SimulationSleepTime = 1000;
        // Time in seconds
        private const int ThreadSleepTime = 30;

        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly CancellationTokenSource _cancellationToken = new();

        private volatile bool _isSimulating = false;
        private int _trayAppRetrys = 3;

        private readonly PipeClientManager _pipeClientManager;

        private readonly Thread _timeoutThread;

        private DateTime _timeoutDeadline = default(DateTime);

        public TimeoutManager(PipeClientManager pipeClientManager)
        {
            _pipeClientManager = pipeClientManager;

            _logger.Trace("Setting events ...");
            _pipeClientManager.OnTrayStarted += OnProcessStarted;
            _pipeClientManager.OnTrayStopped += OnProcessStopped;

            _logger.Trace("Starting thread ...");
            _timeoutThread = new Thread(TimeoutThread);
            _timeoutThread.Start();

            _logger.Debug("Timeout Manager started");
        }

        private void TimeoutThread()
        {
            try
            {
                _logger.Debug($"Timeout thread started");

                var lastExpectedResponse = _pipeClientManager.CurrentStep;

                while(!_cancellationToken.IsCancellationRequested)
                {
                    var sleepTime = 0;
                    while(sleepTime++ != ThreadSleepTime)
                    {
                        Thread.Sleep(1000);
                        _cancellationToken.Token.ThrowIfCancellationRequested();
                    }


                    if(_isSimulating)
                    {
                        _logger.Trace("Responses are simulated. Timeout thread stopping ...");
                        return;
                    }

                    var expectedResponse = _pipeClientManager.CurrentStep;
                    _logger.Trace($"Checking for timeouts for dialog {expectedResponse}...");

                    if(_timeoutDeadline == default(DateTime) || lastExpectedResponse != expectedResponse)
                    {
                        _logger.Trace("Updating deadline ...");
                        UpdateTimeout();
                        lastExpectedResponse = expectedResponse;
                    }

                    if(_timeoutDeadline > DateTime.Now)
                    {
                        _logger.Trace("Deadline not yet reached ...");
                        continue;
                    }

                    _logger.Info("Timeout deadline reached! Sending fake response ...");

                    _pipeClientManager.FakeReceivedMessage(new NewMessageEventArgs()
                    {
                        MessageId = MessageId.ContinueDeployment,
                        Message = new ContinueMessage()
                        {
                            DeploymentStep = _pipeClientManager.CurrentStep
                        }
                    });
                }
            }
            catch(OperationCanceledException) { }
        }

        private void UpdateTimeout()
        {
            var timeout = 0;
            var activeSequence = EnvironmentVariables.ActiveSequence;
            var expectedResponse = _pipeClientManager.CurrentStep;
            switch(expectedResponse)
            {
                case DeploymentStep.CloseApplications:
                {
                    timeout = activeSequence.CloseProgramsSettings.TimeUntilForcedClose;

                    if(timeout <= 0)
                    {
                        timeout = EnvironmentVariables.DeploymentToolkitStepTimout;
                        _logger.Trace($"No close timeout specified. Using global threshold of {timeout} seconds");
                    }
                }
                break;

                case DeploymentStep.Restart:
                {
                    timeout = activeSequence.RestartSettings.TimeUntilForcedRestart;

                    if(timeout <= 0)
                    {
                        timeout = EnvironmentVariables.DeploymentToolkitStepTimout;
                        _logger.Trace($"No restart timeout specified. Using global threshold of {timeout} seconds");
                    }
                }
                break;

                default:
                {
                    timeout = EnvironmentVariables.DeploymentToolkitStepTimout;
                    _logger.Trace($"No close timeout specified. Using global threshold of {timeout} seconds");
                }
                break;
            }

            _timeoutDeadline = DateTime.Now.AddSeconds(timeout + EnvironmentVariables.DeploymentToolkitStepExtraTime);

            _logger.Trace($"Deadline updated to {_timeoutDeadline.ToString()}");
        }

        private void OnProcessStarted(object sender, EventArgs e)
        {
            if(_pipeClientManager.ConnectedClients > 0)
            {
                _logger.Debug($"{_pipeClientManager.ConnectedClients} clients connected. Expecting responses ...");
            }
        }

        private void OnProcessStopped(object sender, EventArgs e)
        {
            if(_pipeClientManager.ConnectedClients > 0)
            {
                _logger.Debug($"Tray app disconnected. {_pipeClientManager.ConnectedClients} remaining clients");
                return;
            }

            _logger.Warn($"No more tray apps running !");

            if(_trayAppRetrys == 0)
            {
                _logger.Warn("No more retries left. Forcing deployment ...");
                SimulateUserResponses();
                return;
            }

            _logger.Info($"Trying to start tray apps. {--_trayAppRetrys} retries left ...");

            _pipeClientManager.TryStartTray();
        }

        private void SimulateUserResponses()
        {
            _logger.Trace("Starting simulation of user responses");
            _isSimulating = true;

            var lastExpectedResponse = DeploymentStep.Start;

            while(_pipeClientManager.CurrentStep != DeploymentStep.End)
            {
                var expectedResponse = _pipeClientManager.CurrentStep;
                _logger.Trace($"Current expected answer: {expectedResponse}");


                if(lastExpectedResponse != expectedResponse)
                {
                    _logger.Info($"Sending fake response for {expectedResponse}");

                    _pipeClientManager.FakeReceivedMessage(new NewMessageEventArgs()
                    {
                        MessageId = MessageId.ContinueDeployment,
                        Message = new ContinueMessage()
                        {
                            DeploymentStep = expectedResponse
                        }
                    });

                    if(expectedResponse == DeploymentStep.Restart)
                    {
                        // Quit when a Restart is detected
                        break;
                    }

                    lastExpectedResponse = expectedResponse;
                }

                // Wait so deployment toolkit can send next response
                Thread.Sleep(SimulationSleepTime);
            }

            _logger.Trace("Deployment ended");
        }

        public void Dispose()
        {
            if(!_cancellationToken.IsCancellationRequested)
            {
                _cancellationToken.Cancel();
            }

            if(_isSimulating)
            {
                _logger.Debug("Aborting Simulation ...");
                _pipeClientManager.CurrentStep = DeploymentStep.End;
            }
        }
    }
}
