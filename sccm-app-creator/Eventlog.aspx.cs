using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CM_App_Creator
{
    public partial class Eventlog : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fileNEWS = Server.MapPath(@"Eventlog.log");
            try
            {
                string[] AllLines = System.IO.File.ReadAllLines(fileNEWS);
                int sortField = 0;
                int sortField2 = 1;
                int appcounter = 0;

                // Demonstrates how to return query from a method.   
                // The query is executed here.   

                DataTable dt = new DataTable();
                dt.Columns.Add("Time");
                dt.Columns.Add("Creator");
                dt.Columns.Add("Action");

                foreach (string[] str in RunQueryReturnFields(AllLines, sortField, sortField2))
                {
                    if (str[2].Contains("Successfully saved application"))
                    {
                        ++appcounter;
                        str[2] = "<b>" + str[2] + "</b>";
                    }
                    else if (str[2].Contains("ERROR"))
                    {
                        str[2] = "<b><font color='red'>" + str[2] + "</b></font><br>";
                    }
                    dt.Rows.Add(str);
                }

                    foreach (string str in RunQuery(AllLines, sortField, sortField2))
                {
                    //dt.Rows.Add(str, str, str);

                    //if (str.Contains("Successfully saved application"))
                    //{
                    //    ++appcounter;
                    //    txtfileNews.InnerHtml += "<b>" + str + "</b><br>";
                    //}
                    //else if (str.Contains("ERROR"))
                    //{
                    //    txtfileNews.InnerHtml += "<b><font color='red'>" + str + "</b></font><br>";
                    //}
                    //else
                    //{
                    //    txtfileNews.InnerHtml += str + "<br>";
                    //}
                }
                GridView1.DataSource = dt;
                GridView1.DataBind();

                nrofappscreated.InnerText = "Number of applications created: " + appcounter.ToString();

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

        // Returns the query variable, not query results!   
        static IEnumerable<string[]> RunQueryReturnFields(IEnumerable<string> source, int num, int num2)
        {
            // Split the string and sort on field[num]   
            var scoreQuery = from line in source
                             let fields = line.Split(';')
                             orderby fields[num] descending, fields[num2]
                             select fields;

            return scoreQuery;
        }

        // Returns the query variable, not query results!   
        static IEnumerable<string> RunQuery(IEnumerable<string> source, int num, int num2)
        {
            // Split the string and sort on field[num]   
            var scoreQuery = from line in source
                             let fields = line.Split(';')
                             orderby fields[num] descending, fields[num2]
                             select line;

            return scoreQuery;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //if (e.Row.Cells[2].Text == "Successfully saved application")
            //{
            //    e.Row.Attributes.Add("style", "font-weight: bold;"); // = "highlightRow"; // ...so highlight it
            //}
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}