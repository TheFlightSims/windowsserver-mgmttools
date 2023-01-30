using DeploymentToolkit.Messaging.Events;
using DeploymentToolkit.Messaging.Messages;
using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;

namespace DeploymentToolkit.Messaging
{
    public class PipeServer : IDisposable
    {
        public event EventHandler<NewMessageEventArgs> OnNewMessage;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly NamedPipeServerStream _receiverPipe;
        private readonly NamedPipeServerStream _senderPipe;

        private StreamReader _reader;
        private StreamWriter _writer;

        private readonly NotifyIcon _notifyIcon;

        public PipeServer(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;

            var process = Process.GetCurrentProcess();
            var processId = process.Id;

            var pipeName = $"DT_{processId}";

            try
            {
                _senderPipe = new NamedPipeServerStream($"{pipeName}_In", PipeDirection.InOut, 1, PipeTransmissionMode.Message);
                _receiverPipe = new NamedPipeServerStream($"{pipeName}_Out", PipeDirection.InOut, 1, PipeTransmissionMode.Message);

                WaitForClient();
            }
            catch(Exception ex)
            {
                _logger.Fatal(ex, "Failed to create pipe server");
                return;
            }

            _logger.Info($"Initialized pipe server ({pipeName})");
        }

        public void Dispose()
        {
            if(!_cancellationToken.IsCancellationRequested)
            {
                _cancellationToken.Cancel();
            }

            _senderPipe?.Dispose();
            _receiverPipe?.Dispose();
        }

        public async void WaitForClient()
        {
            do
            {
                try
                {
                    await _senderPipe.WaitForConnectionAsync(_cancellationToken.Token);
                    await _receiverPipe.WaitForConnectionAsync(_cancellationToken.Token);
                }
                catch(IOException ex)
                {
                    _senderPipe.Disconnect();
                    _receiverPipe.Disconnect();
                    _logger.Info(ex, "Disconnecting pipe");
                }
            }
            while(!_cancellationToken.IsCancellationRequested && (!_senderPipe.IsConnected || !_receiverPipe.IsConnected));
            _logger.Info($"Client connected");

            _reader = new StreamReader(_receiverPipe);
            _writer = new StreamWriter(_senderPipe)
            {
                AutoFlush = true
            };

            _logger.Trace("Sending initial connect message");
            var user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var split = user.Split('\\');
            var connectMessage = new InitialConnectMessage()
            {
                Username = split[1],
                Domain = split[0],
                SessionId = Process.GetCurrentProcess().SessionId
            };
            SendMessage(connectMessage);

            // Wait till the other end has read the message before processing further messages
            _senderPipe.WaitForPipeDrain();

            ReadMessages();
        }

        public void SendMessage(IMessage message)
        {
            if(!_senderPipe.IsConnected)
            {
                _logger.Warn("Tried to send a message while not being connected");
                return;
            }
            _logger.Info($"Sending message of type {message.MessageId}");
            try
            {
                var data = Serializer.SerializeMessage(message);
                _writer.WriteLine(data);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to send message {message.MessageId}");
            }
        }

        private async void ReadMessages()
        {
            _logger.Info("Reading messages");
            do
            {
                var message = await _reader.ReadLineAsync();
                if(string.IsNullOrEmpty(message))
                {
                    continue;
                }

                ProcessMessage(message);
            }
            while(_receiverPipe.IsConnected);
            _logger.Info("Client disconnected");
            _senderPipe.Disconnect();
            _receiverPipe.Disconnect();
            WaitForClient();
        }

        private void ProcessMessage(string data)
        {
            _logger.Trace($"Starting processing message ({data})");
            try
            {
                var simpleMessage = Serializer.DeserializeMessage<BasicMessage>(data);
                if(simpleMessage == null)
                {
                    return;
                }

                _logger.Info($"Received new message {simpleMessage.MessageId}");



                switch(simpleMessage.MessageId)
                {
                    case MessageId.DeploymentInformationMessage:
                    {
                        var message = Serializer.DeserializeMessage<DeploymentInformationMessage>(data);
                        OnNewMessage?.BeginInvoke(
                            this,
                            new NewMessageEventArgs()
                            {
                                MessageId = simpleMessage.MessageId,
                                Message = message
                            },
                            OnNewMessage.EndInvoke,
                            null
                        );
                    }
                    break;

                    case MessageId.DeploymentStarted:
                    case MessageId.DeploymentSuccess:
                    case MessageId.DeploymentError:
                    {
                        // We don't need special parsing of these messages so just straight send them through
                        OnNewMessage?.BeginInvoke(
                            this,
                            new NewMessageEventArgs()
                            {
                                MessageId = simpleMessage.MessageId,
                                Message = simpleMessage
                            },
                            OnNewMessage.EndInvoke,
                            null
                        );
                    }
                    break;

                    case MessageId.DeploymentRestart:
                    {
                        var message = Serializer.DeserializeMessage<DeploymentRestartMessage>(data);
                        OnNewMessage?.BeginInvoke(
                            this,
                            new NewMessageEventArgs()
                            {
                                MessageId = simpleMessage.MessageId,
                                Message = message
                            },
                            OnNewMessage.EndInvoke,
                            null
                        );
                    }
                    break;

                    case MessageId.DeploymentLogoff:
                    {
                        var message = Serializer.DeserializeMessage<DeploymentLogoffMessage>(data);
                        OnNewMessage?.BeginInvoke(
                            this,
                            new NewMessageEventArgs()
                            {
                                MessageId = simpleMessage.MessageId,
                                Message = message
                            },
                            OnNewMessage.EndInvoke,
                            null
                        );
                    }
                    break;

                    case MessageId.CloseApplications:
                    {
                        var message = Serializer.DeserializeMessage<CloseApplicationsMessage>(data);
                        OnNewMessage?.BeginInvoke(
                            this,
                            new NewMessageEventArgs()
                            {
                                MessageId = simpleMessage.MessageId,
                                Message = message
                            },
                            OnNewMessage.EndInvoke,
                            null
                        );
                    }
                    break;

                    case MessageId.DeferDeployment:
                    {
                        var message = Serializer.DeserializeMessage<DeferMessage>(data);
                        OnNewMessage?.BeginInvoke(
                            this,
                            new NewMessageEventArgs()
                            {
                                MessageId = simpleMessage.MessageId,
                                Message = message
                            },
                            OnNewMessage.EndInvoke,
                            null
                        );
                    }
                    break;

                    default:
                    {
                        _logger.Warn($"Unknown message type: {simpleMessage.MessageId}");
                    }
                    break;
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Error processing message");
            }
        }
    }
}
