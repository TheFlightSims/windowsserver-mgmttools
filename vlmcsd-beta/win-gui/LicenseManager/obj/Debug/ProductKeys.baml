   M S B A M L   `   `   `  ????L  HLicenseManager, Version=5.5.0.2300, Culture=neutral, PublicKeyToken=nullK*clr-namespace:HGM.Hotbird64.LicenseManagerHGM.Hotbird64.LicenseManager  5   <   .    (HGM.Hotbird64.LicenseManager.ProductKeys   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    6G   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      9lm*clr-namespace:HGM.Hotbird64.LicenseManager     6<     ? Title$  Install a Generic Key??6r   $	??600??6?   $	??450??5      .??5      ? -   5         ?Name$ 
TopElement??5   	    ? RowDefinitions 5      ?? 5      $??* ?6   ?? 5      $
??Auto ?6   .+?5
      ]? $??5,5,5,5q?6   .+?5      ?? $	??Top=?6   $??Choose a License Key??6'   R? 5      -   5         R?Name$ ProductTree??5      $??0,2,0,0q?6&   ??5   
   ?? 5       ?? 
TargetType" Q?6   .?5      -   5        5      5   	   5      ?? 5      $??5,5,5,5q?6   $??1??6"   .+?5      ?? -   5        $??TextBlockGenerated??5   N   $
??Wrap=?6   
???  ??$y?	Collapsed=?67   MKWARNING! This key is user-generated keys and may cannot be used to activate6?   ?? 5      $
??Wrap=?6   $??0,7,0,2q?6&   Product Key6D   ?? 5      -   5         ??Name$ GvlkKey??5         StandardTextBox#??[  6v   ??6!   ??  ?? ElementName$ ProductTree?? ?? Path$+ %SelectedItem.(lm:ProductKeys.ToolTip)?6v   ? 5      $??0,5,0,5q?6   $??Right=?6#   .+?5   
   ?? -   5         ??Name$ InstallButton??5      $??5,5,5,5q?6(   $??70??69   	 ?? 	IsDefault	 . {?.  .??6?   Check6?   ?? 5   
   -   5        $??5,5,0,5q?5      $??70??6"   
 ?? IsCancel
 . .??6]   Cancel6e   5   	   5       5!      5#      