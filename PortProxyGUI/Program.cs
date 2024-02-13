using PortProxyGUI.Data;
using System;
using System.IO;
using System.Windows.Forms;

namespace PortProxyGUI
{
    static class Program
    {
        public static readonly ApplicationDbScope Database = ApplicationDbScope.FromFile(
            Path.Combine(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "PortProxyGUI"
                ), "config.db"
            ));

        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PortProxyGUI());
        }
    }
}
