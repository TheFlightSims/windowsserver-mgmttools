   M S B A M L   `   `   `  ????L  HLicenseManager, Version=5.5.0.2300, Culture=neutral, PublicKeyToken=nullK*clr-namespace:HGM.Hotbird64.LicenseManagerHGM.Hotbird64.LicenseManager  5   ?   1    +HGM.Hotbird64.LicenseManager.InstallKmsKeys   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    6M   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     6?   @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6B   <local*clr-namespace:HGM.Hotbird64.LicenseManager     6?     ? Title$  Install Multiple GVLKs??5       ? SizeToContent$ WidthAndHeight=?65    ? 
ResizeMode$ NoResize=?6T    ? 	Resources 5      ??%    5      .??5      ? -   5         ?Name$ 
TopElement??5   	    ? RowDefinitions 5      ?? 5	      $??* ?6   ?? 5
      $
??Auto ?6   ?? 5      $??24 ?6   .+?5      ?? $??KMS Client Products??6   $??5,5,5,5q?6,   $??0??6=   $??0??6J   .??5      &   System.Windows.Controls.DataGrid  -   5          Name$ DataGrid??5   a   $   HorizontalScrollBarVisibility$
 Auto=?6)      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6N   	   GridLinesVisibility$	 
Horizontal=?6s   
   HorizontalGridLinesBrush

 ?????   VerticalGridLinesBrush
 ?????   AutoGenerateColumns .  $
??Auto??5   J   $
??Auto??6W      SelectionMode$ Single=?6e      SelectionUnit$ CellOrRowHeader=?6|      HeadersVisibility$ Column=?6?      CanUserDeleteRows .     CanUserAddRows .   ? DataContext 5      ??    	CellStyle 5   
   ?? 5       ?? 
TargetType*  $System.Windows.Controls.DataGridCell"  6    ?? Triggers 5      P? 5       P? Property ?@ 
IsSelected P? Value . . ?5      ??  ?? Property ? ?? ?? Value ?	#000000ff6/   ?? 5       ? ?? ?	#2000000060   5      5       ? ContextMenu 5   
   ?? 5      .@?5       m?  m? Command$
 Copy??6   5!         Columns 5$   
   0  *System.Windows.Controls.DataGridTextColumn  5%      ,  &System.Windows.Controls.DataGridColumn   Header$ Product??63      
Foreground
 ?   ?   
IsReadOnly .     SortMemberPath$  DisplayName??6i   !   SortDirection$! 	AscendingX?6?   1  +System.Windows.Controls.DataGridBoundColumn"   Binding" 6   ?? 6?     5&      -   5        PartialProductKeyColumn??
 ?   ?$ Installed GVLK??5&   y    . $  PartialProductKey??6?   " 6@   ?? *PartialProductKey+6?   4  .System.Windows.Controls.DataGridTemplateColumn  5(      $ Available GVLK??6#    . $
  Gvlk??6M   #   CellTemplate# 5)      ?? 5*      $ ?? DataType$$ local:KmsLicense??6   .R?5+      ?? 
???   ?$??5,0,5,0q?6/   % ^? ShowDuration$% 15000??6?   ??6@   ?? *Gvlk+??6V   ?? *	GvlkColor+& ? ToolTip& 6w   ?? *GvlkToolTip+6?   5,      5.        50      -   5        LicenseStatusColumn??$ License Status??50   @    . $  LicenseStatusText??6j   # 51      ?? 52      $$ local:KmsLicense??6   .R?53      ?? $??5,0,5,0q?6   ??6-   ?? *LicenseStatusText+??6P   ?? *LicenseColor+54      56        58      -   5        SelectColumn??$ Install??58   9   # 59      ?? 5:      $$ local:KmsLicense??6   .R?5;      ??  5<      ?? 5=      .@?5>      m? $
 Copy??6!   {?.  6D   5?      .+?5A      ?? -   5        {?5A      ?? *IsControlEnabled+' ?? Mode$' TwoWay=?( ?? UpdateSourceTrigger$( PropertyChanged=?) d? 	IsChecked) 5B      ?? *IsCheckBoxChecked+$' TwoWay=?$( PropertyChanged=?y?6q   ?? *CheckBoxVisibility+6?   	? 5C      -   5        {?5C       ?? *IsControlEnabled+$' TwoWay=?$( PropertyChanged=?) 5D      ?? *IsRadioButtonChecked+$' TwoWay=?$( PropertyChanged=?y?6t   ?? *RadioButtonVisibility+6?   5E      5F      5H        5J      $ Result??6#   *   
Visibility$* 	Collapsed=?63   -   5        ResultColumn??$  InstallMessage??5J   `   # 5K      ?? 5L      $$ local:KmsLicense??6   .R?5M      ?? $??5,0,5,0q?6   $% 30000??6?   ??6-   ?? *InstallMessage+??6M   ?? *InstallMessageColor+& 6x   ?? *InstallToolTip+6?   5N      5P      5S   	   5U      ?? 5W      + ?? Orientation$+ 
Horizontal=?6   $??1??6*   $??Right=?67   $??0,6,0,10q?6S    5X      ??%?   '??       ?? 5Y   
   " ??6   .?5Z      ??  ? ??$ 24??6%   ?? 5[       ? ??$ 80??6'   ?? 5\       ? ??$ 0,0,10,0q?6%   ?? 5]       ? ??$ 8,0,8,0q?6&   5^      5_   	   .+?5`      ?? -	   5        , ??Name$, InstallButton??5`      $??_Install GVLKs??6&   - ?? 	IsDefault- . 6P   ?? 5a      -
   5        $, CancelButton??5a      $??_Cancel??6%   . ?? IsCancel. . 67   5b      ?? 5d      $??2??6   ???#f0f0f06   ]? 5e      $??2??6   .+?5f      ?? -   5        / ??Name$/ LabelStatus??5f      $??5,0,0,0q?6'   $??Center=?68   Ready6Z   ? 5g      -   5        0 ?Name$0 ProgressBar??5g      1 ? IsIndeterminate1 .  $y?	Collapsed=?6A   $??5,4,5,4q?6X   5h      5i      5j      