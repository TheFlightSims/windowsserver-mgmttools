﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using HGM.Hotbird64.Vlmcs;

// ReSharper disable once CheckNamespace
namespace HGM.Hotbird64.LicenseManager
{
    public static partial class NativeMethods
    {
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static extern IntPtr LoadLibraryW(string dllFileName);
    }

    public partial class App
    {
        public static readonly Brush DefaultTextBoxBackground = new SolidColorBrush(Color.FromRgb(0xf0, 0xf0, 0xf0));
        public const double ZoomFactor = 1.025;
        public static bool HaveLibKms, IsLibKmsLoadError;
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public static string DatabaseFileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "KmsDataBase.xml");
        //public static readonly IDictionary<KmsGuid, CsvlkRule> CsvlkRules = new Dictionary<KmsGuid, CsvlkRule>(40);
        public const string GuidPattern = @"^([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-4[0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12})|(\{[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-4[0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}\})$";
        public static readonly IReadOnlyList<KmsGuid> ServerKmsGuids;
        public static event Action DataBaseLoaded;
        public static readonly string ExeDirectoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static IReadOnlyList<Brush> DatagridBackgroundBrushes = new Brush[]
        {
            new SolidColorBrush(new Color { A = 255, R = 255, B = 255, G = 255 }),
            new SolidColorBrush(new Color { A = 255, R = 240, B = 255, G = 248 }),
        };

        public static bool IsDatabaseLoaded;


        static App()
        {
            // TODO: Get rid of this
            var win2008A = new KmsGuid("33e156e4-b76f-4a52-9f91-f641dd95ac48");
            var win2008B = new KmsGuid("8fe53387-3087-4447-8985-f75132215ac9");
            var win2008C = new KmsGuid("8a21fdf3-cbc5-44eb-83f3-fe284e6680a7");
            var win2008R2A = new KmsGuid("0fc6ccaf-ff0e-4fae-9d08-4370785bf7ed");
            var win2008R2B = new KmsGuid("ca87f5b6-cd46-40c0-b06d-8ecd57a4373f");
            var win2008R2C = new KmsGuid("b2ca2689-a9a8-42d7-938d-cf8e9f201958");
            var win2012 = new KmsGuid("8665cb71-468c-4aa3-a337-cb9bc9d5eaac");
            var win2012R2 = new KmsGuid("8456efd3-0c04-4089-8740-5b7238535a65");
            var win2016 = new KmsGuid("6e9fc069-257d-4bc4-b4a7-750514d32743");
            var win2019 = new KmsGuid("8449b1fb-f0ea-497a-99ab-66ca96e9a0f5");
            var win2022 = new KmsGuid("b74263e4-0f92-46c6-bcf8-c11d5efe2959");
            ServerKmsGuids = new[] { win2008A, win2008B, win2008C, win2008R2A, win2008R2B, win2008R2C, win2012, win2012R2, win2016, win2019, win2022 };
        }

        private static bool TryLoadLibrary(string dllFileName)
        {
            if (NativeMethods.LoadLibraryW(dllFileName) != IntPtr.Zero)
            {
                return true;
            }

            var error = Marshal.GetLastWin32Error();

            if (error == 126)
            {
                return false;
            }

            var ex = new Win32Exception(error);
            var message = ex.Message.Replace("%1", dllFileName);
            throw new Win32Exception(error, message);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                TapMirror.Stop();
                if (ProductBrowser.PKeyConfigFiles == null) { return; }

                foreach (var file in ProductBrowser.PKeyConfigFiles)
                {
                    if (!file.IsOnFileSystem || file.IsUnzippedExternal)
                    {
                        continue;
                    }

                    try
                    {
                        File.Delete(file.TempFileName);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            finally
            {
                base.OnExit(e);
            }
        }

        private void AppUI_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show
            (
#if DEBUG
        e.Exception == null ? "An unknown error has occured." : $"{e.Exception}",
#else
        e.Exception == null ? "An unknown error has occured." : $"{e.Exception.GetType().Name}: {e.Exception.Message}",
#endif
        "License Manager Error",
              MessageBoxButton.OK,
              MessageBoxImage.Error
            );

            e.Handled = true;
        }

        public static void LoadLibKms(string fileName)
        {
            try
            {
                HaveLibKms = TryLoadLibrary(fileName);
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show(ex.Message, $"Error Loading \"{fileName}\"", MessageBoxButton.OK, MessageBoxImage.Error);
                HaveLibKms = false;
                IsLibKmsLoadError = true;
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if
            (
                Clipboard.ContainsText() &&
                Regex.IsMatch(Clipboard.GetText().ToUpperInvariant(), BinaryProductKey.KeyPattern) &&
                MessageBox.Show
                (
                    "There is a Product Key in the Windows Clipboard." + Environment.NewLine +
                    "Do you want to clear the Clipboard?",
                    "License Manager Privacy Warning",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.Yes
                ) == MessageBoxResult.Yes
            )
            {
                Clipboard.Clear();
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //var xmlDoc = new XmlDocument();
            //xmlDoc.Load(@"C:\Windows\System32\spp\tokens\skus\Enterprise\Enterprise-Volume-CSVLK-2-ul-oob-rtm.xrm-ms");
            ////var result=xmlDoc.SelectNodes("/*[local-name()='licenseGroup']/*[local-name()='license']/*[local-name()='grant']/*[local-name()='grant']/*[local-name()='allConditions']/*[local-name()='allConditions']/*[local-name()='productPolicies']/*[local-name()='policyStr']");
            //var kmsIds = Regex.Split(xmlDoc.SelectSingleNode("//*[@name='Security-SPP-KmsCountedIdList']")?.InnerText, @"\s*,\s*");
            //var skuId = xmlDoc.SelectSingleNode("//*[@name='productSkuId']")?.InnerText;
            //var appId = xmlDoc.SelectSingleNode("//*[@name='applicationId']")?.InnerText;
            //var name = xmlDoc.SelectSingleNode("//*[@name='productDescription']")?.InnerText;

            LoadLibKms($"libkms{IntPtr.Size << 3}.dll");

            KmsLists.LoadDatabase = () =>
            {
                if (IsDatabaseLoaded)
                {
                    return;
                }

                try
                {
                    using (var stream = new FileStream(DatabaseFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        KmsLists.ReadDatabase(stream);
                        IsDatabaseLoaded = true;
                        DataBaseLoaded?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is FileNotFoundException || ex.InnerException is FileNotFoundException))
                    {
                        MessageBox.Show
                  (
                    $"The KMS database \"{DatabaseFileName}\" could not be read:\n{ex.Message}\n\nUsing License Manager's built-in database",
                    $"Could not read {Path.GetFileName(DatabaseFileName)}", MessageBoxButton.OK, MessageBoxImage.Error
                  );
                    }
                    using (var stream = GetResourceStream(new Uri("pack://application:,,,/LmInternalDatabase.xml"))?.Stream)
                    {
                        KmsLists.ReadDatabase(stream);
                        IsDatabaseLoaded = true;
                        DataBaseLoaded?.Invoke();
                    }
                }
            };

            KmsLists.GetXsdValidationStream = () => GetResourceStream(new Uri("pack://application:,,,/KmsDataBase.xsd"))?.Stream;
        }
    }
}
