   M S B A M L   `   `   `  ????L  HLicenseManager, Version=5.5.0.2300, Culture=neutral, PublicKeyToken=nullK*clr-namespace:HGM.Hotbird64.LicenseManagerHGM.Hotbird64.LicenseManager  5   L   /    )HGM.Hotbird64.LicenseManager.OwnKeyWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    6K   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     6<   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      <local*clr-namespace:HGM.Hotbird64.LicenseManager     6L     ? Title$   Keys found in the Registry??5      $
??Auto??6(   $
??Auto??66    ? SizeToContent$ WidthAndHeight=?6C    ? 	Resources 5         	GridStyle ?? 
TargetType ?? Property ?? Value??%Q   &	        ?? 5      " ??6   .?5      ??  ? ??$ 2q?6!   5	      5
       J? CommandBindings 5      )  #System.Windows.Input.CommandBinding  5      -   5        /   )HGM.Hotbird64.LicenseManager.OwnKeyWindow   
InstallKey   Command# Z 5      6k   .??5      ?? 	 ?? Orientation$	 Vertical=?6   .+?5      ?? -   5        
 ??Name$
 
TopElement??5      $??Keys in the Registry??6#   $??5,5,5,5q?6A   .??5      &   System.Windows.Controls.DataGrid  -   5          Name$ DataGridKeys??5   )   $
??Auto??6?      AutoGenerateColumns .     GridLinesVisibility$	 All=?6i      HorizontalGridLinesBrush
 ?????   VerticalGridLinesBrush
 ?????   SelectionMode$ Single=?5   h      SelectionUnit$ CellOrRowHeader=?6      RowHeaderWidth$ 20Q?5       ? DataContext 5      ??  ? ContextMenu 5   
   ?? 5      .@?5      m?  m? Command$
 Copy??6   m? 5      # Z 6   5         RowStyle 5   
   ?? 5      )  #System.Windows.Controls.DataGridRow"  6   .?5      ??  ?@^?ShowDuration$ 15000??6<   ?? 5        ?@?ToolTip 5!      ?? 5"      ??6   ?? *
HiveName+5$      5%         Columns 5(   
   0  *System.Windows.Controls.DataGridTextColumn  5)      1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  6   ,  &System.Windows.Controls.DataGridColumn   Header$ Install Date??6I      SortDirection$ 	AscendingX?6      SortMemberPath$ InstallDate??5*         
IsReadOnly .    Binding 5)   _   ?? *DisplayDate+5*   ,     5+      # [  6   $ Product Name??6I    .  6_   ?? *ProductName+6     5,      # [  6   $  Digital Product 3rd ID Key??6I    .  6m   ?? *Id3+6?     5-      # [  6   $  Digital Product 4th ID Key??6I    .  6m   ?? *Id4+6?   50   	   52      ? 53      $??	0,0,10,10q?6    ? Orientation$ 
Horizontal=?6#   $??Right=?6<   .+?54      ?? -   5         ??Name$ InstallButton??54      $??
Check _Key??6&   $??24??6;   $??60??6G   $??5,0,5,0q?6U   $??5,0,5,0q?6g   {?.  55      ?? 56      -   5        $ CancelButton??56      $??_Cancel??6%     ?? IsCancel  . $??24??6G   $??60??6S   $??5,0,5,0q?6a   $??5,0,5,0q?6s   6?   57      58      59      