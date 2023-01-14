using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WSUSOnlineDescriptions
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		#region Fields
		RelayCommand queryUpdatesCommand = new RelayCommand(true);
		RelayCommand saveDescriptionsCommand = new RelayCommand(false);

		ObservableCollection<WSUSUpdate> unapprovedUpdates = new ObservableCollection<WSUSUpdate>();
		ObservableCollection<WSUSLanguage> availableLanguages = new ObservableCollection<WSUSLanguage>();
		List<WSUSServerVersion> serverVersions = new List<WSUSServerVersion>()
		{
			new WSUSServerVersion("Windows Server 2012", @"\\.\pipe\MICROSOFT##WID\tsql\query"),
			new WSUSServerVersion("Windows Server 2003-2008", @"\\.\pipe\MSSQL$MICROSOFT##SSEE\sql\query")
		};

		WSUSServerVersion selectedServerVersion;
		WSUSLanguage selectedLanguage;
		double progressValue;
		double progressMax = 100;
		bool isProgressBarIndeterminate;
		#endregion

		#region Properties
		private string dbConnectionString
		{
			get
			{
				return new SqlConnectionStringBuilder()
				{
					DataSource = this.SelectedServerVersion.DatabaseName,
					InitialCatalog = "SUSDB",
					IntegratedSecurity = true
				}.ConnectionString;
			}
		}

		public RelayCommand QueryUpdatesCommand
		{
			get { return queryUpdatesCommand; }
		}

		public RelayCommand SaveDescriptionsCommand
		{
			get { return saveDescriptionsCommand; }
		}

		public ReadOnlyObservableCollection<WSUSUpdate> UnapprovedUpdates
		{
			get { return new ReadOnlyObservableCollection<WSUSUpdate>(unapprovedUpdates); }
		}

		public ReadOnlyObservableCollection<WSUSLanguage> AvailableLanguages
		{
			get { return new ReadOnlyObservableCollection<WSUSLanguage>(availableLanguages); }
		}

		public ReadOnlyCollection<WSUSServerVersion> ServerVersions
		{
			get { return serverVersions.AsReadOnly(); }
		}

		public WSUSServerVersion SelectedServerVersion
		{
			get { return selectedServerVersion; }
			set
			{
				selectedServerVersion = value;
				NotifyPropertyChanged("SelectedServerVersion");
			}
		}

		public WSUSLanguage SelectedLanguage
		{
			get { return selectedLanguage; }
			set
			{
				selectedLanguage = value;
				NotifyPropertyChanged("SelectedLanguage");
			}
		}

		public double ProgressValue
		{
			get { return progressValue; }
			private set
			{
				progressValue = value;
				NotifyPropertyChanged("ProgressValue");
			}
		}

		public double ProgressMax
		{
			get { return progressMax; }
			private set
			{
				progressMax = value;
				NotifyPropertyChanged("ProgressMax");
			}
		}

		public bool IsProgressBarIndeterminate
		{
			get { return isProgressBarIndeterminate; }
			private set
			{
				isProgressBarIndeterminate = value;
				NotifyPropertyChanged("IsProgressBarIndeterminate");
			}
		}
		#endregion

		public MainWindowViewModel()
		{
			this.queryUpdatesCommand.CommandExecuted += queryKBCommand_CommandExecuted;
			this.saveDescriptionsCommand.CommandExecuted += saveDescriptionsCommand_CommandExecuted;
			this.SelectedServerVersion = this.serverVersions[0];
		}

		#region Commands
		private async void queryKBCommand_CommandExecuted(object sender, EventArgs e)
		{
			this.queryUpdatesCommand.SetCanExecute(false);
			this.unapprovedUpdates.Clear();
			this.IsProgressBarIndeterminate = true;

			try
			{
				using (SqlConnection con = new SqlConnection(this.dbConnectionString))
				{
					con.Open();

					using (SqlCommand command = new SqlCommand(
						"SELECT l.ShortLanguage, l.LongLanguage " +
						"FROM tbLanguage l " +
						"WHERE l.Enabled = 1 " +
						"ORDER BY l.LongLanguage",
						con))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								this.availableLanguages.Add(new WSUSLanguage(reader.GetString(0), reader.GetString(1)));
							}
						}
					}

					using (SqlCommand command = new SqlCommand(
						"SELECT DISTINCT kb.KBArticleID " +
						"FROM tbPreComputedLocalizedProperty p " +
						"JOIN tbUpdate u ON p.UpdateID = u.UpdateID " +
						"JOIN tbRevision r ON u.LocalUpdateID = r.LocalUpdateID " +
						"JOIN tbKBArticleForRevision kb ON r.RevisionID = kb.RevisionID " +
						"WHERE u.IsHidden = 0 AND r.State = 3 AND r.IsLatestRevision = 1 " +
						"ORDER BY kb.KBArticleID",
						con))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								this.unapprovedUpdates.Add(new WSUSUpdate(reader.GetString(0)));
							}
						}
					}
				}

				this.SelectedLanguage = this.availableLanguages[0];
				await Task.WhenAll(from update in this.unapprovedUpdates
								   select update.QueryKBDetailsAsync());
			}
			catch (Exception ex)
			{
				MessageBox.Show(String.Format("The following error occurred:{0}{1}", Environment.NewLine, ex.Message));
				this.unapprovedUpdates.Clear();
			}
			finally
			{
				this.IsProgressBarIndeterminate = false;

				if (this.unapprovedUpdates.Count > 0)
					this.saveDescriptionsCommand.SetCanExecute(true);
				else
					this.queryUpdatesCommand.SetCanExecute(true);
			}
		}

		private async void saveDescriptionsCommand_CommandExecuted(object sender, EventArgs e)
		{
			this.saveDescriptionsCommand.SetCanExecute(false);

			Task dbTask = new Task(SaveDescriptions);
			dbTask.Start();
			await dbTask;

			this.saveDescriptionsCommand.SetCanExecute(true);
		}

		private void SaveDescriptions()
		{
			try
			{
				this.ProgressValue = 0;
				this.ProgressMax = this.unapprovedUpdates.Count;

				using (SqlConnection con = new SqlConnection(this.dbConnectionString))
				{
					con.Open();

					foreach (WSUSUpdate update in unapprovedUpdates)
					{
						using (SqlCommand command = new SqlCommand(
							"UPDATE tbPreComputedLocalizedProperty " +
							"SET Description = @Description " +
							"FROM tbPreComputedLocalizedProperty p " +
							"JOIN tbUpdate u ON p.UpdateID = u.UpdateID " +
							"JOIN tbRevision r ON u.LocalUpdateID = r.LocalUpdateID " +
							"JOIN tbKBArticleForRevision kb ON r.RevisionID = kb.RevisionID " +
							"WHERE kb.KBArticleID = @KB AND p.ShortLanguage = @Language",
							con))
						{
							// Description field is limited to 1500 chars.
							string shortDescription = new String(update.Description.Take(1500).ToArray());

							command.Parameters.Add(new SqlParameter("@Description", shortDescription));
							command.Parameters.Add(new SqlParameter("@KB", update.KB));
							command.Parameters.Add(new SqlParameter("@Language", this.selectedLanguage.ShortLanguage));

							command.ExecuteNonQuery();
						}
						this.ProgressValue++;
					}
				}
			}
			catch (Exception ex)
			{
				Dispatcher.CurrentDispatcher.Invoke(new ThreadStart(() =>
				{
					MessageBox.Show(String.Format("The following error occurred:{0}{1}", Environment.NewLine, ex.Message));
				}));
			}
		}
		#endregion

		#region Notify
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(String propertyName)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}