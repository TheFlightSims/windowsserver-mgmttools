   M S B A M L   `   `   `  ????L  HLicenseManager, Version=5.5.0.2300, Culture=neutral, PublicKeyToken=nullK*clr-namespace:HGM.Hotbird64.LicenseManagerHGM.Hotbird64.LicenseManager  5   	   2    ,HGM.Hotbird64.LicenseManager.ProductSelector   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9lm*clr-namespace:HGM.Hotbird64.LicenseManager     5   	     ? Title$  Select Product??5   	   $	??600??6    $	??450??6-   .??5	      ? -   5         ?Name$ 
TopElement??5	       ? RowDefinitions 5
   
   ?? 5      $??* ?6   ?? 5      $??25 ?6   .+?5   
   ?? $??Available Products??6   $??3,3,3,3q?6/   .??5      R? -   5         R?Name$ ProductTree??5      
?????? $??0q?6F   ??5      ?? 5       ?? 
TargetType" Q?6   .?5      -   5        5      5      5      ?? 5   
    ?? Orientation$ 
Horizontal=?6   $??1??6.   $??Right=?6;   $
??Auto??6W   .+?5      ?? -   5         ??Name$ ButtonOk??5      {?.   ?? 	IsDefault . $??20??6J   $
??Auto??6V   $??Ok??6c   $??60??6p   $??0,0,5,0q?6~   6?   ?? 5       ?? IsCancel . $??20??6%   $
??Auto??61   $??Cancel??6>   $??Right=?6O   $??60??6k   $??0,0,4,0q?6y   5      5      5      