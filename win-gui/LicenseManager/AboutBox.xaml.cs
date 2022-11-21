using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace HGM.Hotbird64.LicenseManager
{
  public partial class AboutBox
  {
    public AboutBox(MainWindow mainWindow) : base(mainWindow)
    {
      InitializeComponent();
      TopElement.LayoutTransform = Scaler;

      LabelProduct.Content = Assembly.GetCallingAssembly().GetName().Name;
      LabelCopyright.Content = ((AssemblyCopyrightAttribute)(Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0])).Copyright;
      LabelCompanyName.Content = ((AssemblyCompanyAttribute)(Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0])).Company;
      var version = Assembly.GetCallingAssembly().GetName().Version;
      LabelVersion.Content = "Version " + version.ToString(3) + (version.MinorRevision < 16384 ?
                               $" Beta {version.MinorRevision}"
                               : "") + $" {IntPtr.Size << 3}-bit";
    }

    private void button_Ok_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }
  }
}
