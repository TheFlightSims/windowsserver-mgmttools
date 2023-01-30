using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CM_App_Creator
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fileNEWS = Server.MapPath(@"About.txt");
            try
            {

                string[] AllLines = System.IO.File.ReadAllLines(fileNEWS, Encoding.GetEncoding("iso-8859-1"));

                // Demonstrates how to return query from a method.   
                // The query is executed here.   
                foreach (string str in AllLines)
                {
                    txtfileNews.InnerHtml += str + "<br>";
                }
            }
            catch (Exception ex)
            {
                // Let the user know what went wrong.
                alertError.Visible = true;
                contentdiv.Visible = false;
                spanError.InnerHtml = String.Format("An error occured when attempting to access file {1}.<br>Error message: {0}", ex.Message, fileNEWS);
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}