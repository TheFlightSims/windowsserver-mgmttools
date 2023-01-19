using System.Windows;
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
      var version = Assembly.GetCallingAssembly().GetName().Version;
      LabelVersion.Content = "Version " + version.ToString(3) + (version.MinorRevision < 16384 ? $" Beta {version.MinorRevision}": "") + "revision 1";
    }

    private void button_Ok_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }
  }
}
