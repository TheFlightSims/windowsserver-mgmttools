// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Navigation;
using System.Windows.Input;
using System.Linq;
using HGM.Hotbird64.Vlmcs;
using System;
using System.Diagnostics.CodeAnalysis;
using HGM.Hotbird64.Icons;
using HGM.Hotbird64.LicenseManager.Extensions;

// ReSharper disable once CheckNamespace
namespace HGM.Hotbird64.LicenseManager
{
	public partial class ProductKeys
	{
		public static IList<ProductKey> ProductKeyList => productKeyList;
		private static readonly ProductKey[] productKeyList =
		{
			new ProductKey ("Windows Preview Core Single Language", "WRR6N-3JB78-MB9TD-WQW7K-WTV84", KeyType.Gvlk),
			new ProductKey ("Windows Preview Core Country Specific", "NDTWC-J28B6-RTR3C-CPRHW-2GV83", KeyType.Gvlk),
			new ProductKey ("Windows Preview Professional N", "G2DR7-7N9C8-KBK9V-D4W3M-H49R8", KeyType.Gvlk),
			new ProductKey ("Windows Preview Enterprise N", "N8BR7-D8XB9-939VB-FMFWD-KWXR9", KeyType.Gvlk),
			new ProductKey ("Windows Preview Standard Server", "YNBF9-GPVTG-FFHQC-MJR4B-B4CQX", KeyType.Gvlk),
			new ProductKey ("Windows Preview Multipoint Server Standard", "3CK7G-VRNKC-8QBFR-9G8HC-YKG8W", KeyType.Gvlk),
			new ProductKey ("Windows Preview Multipoint Server Premium", "GRMDJ-JNF7H-W9WTC-WKQRG-H8KW6", KeyType.Gvlk),
			new ProductKey ("Windows 10 Professional", "VK7JG-NPHTM-C97JM-9MPGT-3V66T", KeyType.StoreLicense ),
			new ProductKey ("Windows 10 Home", "YTMG3-N6DKC-DKB77-7M9GH-8HVX7", KeyType.StoreLicense ),
	};

		private void AddKeysToTreeViewItem(TreeViewItem treeViewItem, IEnumerable<ProductKey> keys)
		{
			foreach (var key in keys)
			{
				var keyItem = new TreeViewItem { Header = key, ToolTip = key.Key };
				treeViewItem.Items.Add(keyItem);
			}
		}

		private static readonly KmsGuid winBetaGuid = new KmsGuid("5f94a0bb-d5a0-4081-a685-5819418b2fe0");

		public ProductKeys(MainWindow mainWindow) : base(mainWindow)
		{
			InitializeComponent();
			TopElement.LayoutTransform = Scaler;
			DataContext = this;

			Loaded += (s, e) => Icon = this.GenerateImage(new Icons.InstallKey(), 16, 16);

			var treeViewItem = new TreeViewItem { Header = "Store license keys" };
			ProductTree.Items.Add(treeViewItem);
			AddKeysToTreeViewItem(treeViewItem, ProductKeyList.Where(k => k.KeyType == KeyType.StoreLicense));

			treeViewItem = new TreeViewItem { Header = "User-generated GVLKs" };
			ProductTree.Items.Add(treeViewItem);
			AddKeysToTreeViewItem(treeViewItem, KmsLists.SkuItemList.Where(s => s.IsGeneratedGvlk && s.Gvlk != null).Select(s => new ProductKey(s.ToString(), s.Gvlk, s.IsGeneratedGvlk ? KeyType.GvlkGenerated : KeyType.Gvlk)));

			treeViewItem = new TreeViewItem { Header = "Genuine GVLKs" };
			ProductTree.Items.Add(treeViewItem);

			foreach (var app in KmsLists.AppItemList)
			{
				var appitem = new TreeViewItem { Header = app };
				treeViewItem.Items.Add(appitem);

				foreach (var kmsId in app.KmsItems.OrderBy(k => k.DisplayName))
				{
					var kmsItem = new TreeViewItem { Header = kmsId };
					AddKeysToTreeViewItem(kmsItem, kmsId.SkuItems.Where(s => !s.IsGeneratedGvlk && s.Gvlk != null).OrderBy(s => s.DisplayName).Select(s => new ProductKey(s.ToString(), s.Gvlk, s.IsGeneratedGvlk ? KeyType.GvlkGenerated : KeyType.Gvlk)));

					if (kmsId == winBetaGuid)
					{
						AddKeysToTreeViewItem(kmsItem, ProductKeyList.Where(k => k.KeyType == KeyType.Gvlk).OrderBy(k => k.Name));
					}

					appitem.Items.Add(kmsItem);
				}
			}

			ProductTree.SelectedItemChanged += (sender, eventArgs) =>
			{
				InstallButton.IsEnabled = ((TreeViewItem)eventArgs.NewValue).ToolTip is string;

				var key = ((TreeViewItem)eventArgs.NewValue).Header as ProductKey;

				if (key != null)
				{
					TextBlockGenerated.Visibility = key.KeyType == KeyType.GvlkGenerated || key.KeyType == KeyType.RetailGenerated ? Visibility.Visible : Visibility.Collapsed;
				}
				else
				{
					TextBlockGenerated.Visibility = Visibility.Collapsed;
				}
			};
		}

		private void TreeViewItem_DoubleClick(object sender, MouseButtonEventArgs args)
		{
			var treeViewItem = ProductTree.SelectedItem as TreeViewItem;
			if (treeViewItem == null || !treeViewItem.HasHeader || !(treeViewItem.ToolTip is string)) return;
			AnalyzeButton_Click(null, null);
		}

		private void TreeViewItem_Collapse(object sender, RoutedEventArgs e) => ((ItemsControl)e.Source).ExpandAll(false);

		private void AnalyzeButton_Click(object sender, RoutedEventArgs e)
		{
			new ProductBrowser(MainWindow, GvlkKey.Text).Show();
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		private void CancelButton_Clicked(object sender, RoutedEventArgs e) => Close();
	}
}
