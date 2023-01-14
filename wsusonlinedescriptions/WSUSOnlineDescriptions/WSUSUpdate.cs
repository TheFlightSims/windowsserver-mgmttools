using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WSUSOnlineDescriptions
{
	public class WSUSUpdate : INotifyPropertyChanged
	{
		string kb;
		string description;

		public string KB
		{
			get { return this.kb; }
		}

		public string Description
		{
			get { return this.description; }
			set
			{
				this.description = value;
				NotifyPropertyChanged("Description");
			}
		}

		public WSUSUpdate(string kb)
		{
			this.kb = kb;
		}

		public async Task QueryKBDetailsAsync()
		{
			Task task = new Task(() =>
			{
				try
				{
					//-------------------------
					// Querying the Microsoft Support API needs some cookies to be set, otherwise 404 errors are thrown.
					// Thanks to gunr2171 @ http://stackoverflow.com/a/31349305/3414957 for finding a solution.
					//-------------------------

					//this holds all the cookies we need to add
					CookieContainer cookieJar = new CookieContainer();
					cookieJar.Add(new Cookie("SMCsiteDir", "ltr", "/", ".support.microsoft.com"));
					cookieJar.Add(new Cookie("SMCsiteLang", "en-US", "/", ".support.microsoft.com"));
					cookieJar.Add(new Cookie("smc_f", "upr", "/", ".support.microsoft.com"));
					cookieJar.Add(new Cookie("smcexpsessionticket", "100", "/", ".microsoft.com"));
					cookieJar.Add(new Cookie("smcexpticket", "100", "/", ".microsoft.com"));
					cookieJar.Add(new Cookie("smcflighting", "wwp", "/", ".microsoft.com"));

					HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://support.microsoft.com/api/content/kb/" + this.kb);
					//attach the cookie container
					request.CookieContainer = cookieJar;

					//and now go to the internet, fetching back the contents
					HttpWebResponse response = (HttpWebResponse)request.GetResponse();
					using (StreamReader sr = new StreamReader(response.GetResponseStream()))
					{
						string site = sr.ReadToEnd();
						HtmlDocument doc = new HtmlDocument();
						doc.LoadHtml(site);

						// Get update title
						string result = doc.DocumentNode.SelectSingleNode("//h1[1]").Attributes["title"].Value;


						// Get detailed update description
						HtmlNode descriptionNode =
							doc.DocumentNode.SelectSingleNode("//div[@class='kb-symptoms-section section']") ??
							doc.DocumentNode.SelectSingleNode("//div[@class='kb-summary-section section']");

						if (descriptionNode != null)
						{
							string details = String.Empty;

							foreach (HtmlNode node in descriptionNode.ChildNodes)
							{
								HtmlAttribute attr = node.Attributes["class"];
								if (attr != null && attr.Value.Contains("header"))
									continue;

								details += node.InnerText;
							}

							result += " - " + details.Trim();
						}

						result = Regex.Replace(result, "&#160;", " ");
						result = Regex.Replace(result, "&#39;", "'");
						result = Regex.Replace(result, "&quot;", "\"");
						result = Regex.Replace(result, "<.*?>", "");
						result = Regex.Replace(result, "[ ]{2,}", " ");
						result = Regex.Replace(result, "&lt;", "<");
						result = Regex.Replace(result, "&gt;", ">");

						this.Description = result;
					}
				}
				catch (Exception ex)
				{
					this.Description = "ERROR: " + ex.Message;
				}
			});
			task.Start();
			await task;
		}

		public override string ToString()
		{
			return "KB" + this.kb;
		}

		#region Notify
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(String propertyName)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}