   M S B A M L   `   `   `  ????L  HLicenseManager, Version=5.5.0.2300, Culture=neutral, PublicKeyToken=nullK*clr-namespace:HGM.Hotbird64.LicenseManagerHGM.Hotbird64.LicenseManager  5   ?   ,    &HGM.Hotbird64.LicenseManager.ExportIds   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    6H   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     6?   @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6B   <local*clr-namespace:HGM.Hotbird64.LicenseManager     6?     ? Title$  Export GUIDs??5      $	??768??6+   $
??1024??68    ^? ShowDuration$ 30000??6E   .??5      ? -   5         ?Name$ 
TopElement??5       ? RowDefinitions 5   
   ?? 5      $
??Auto ?6   ?? 5      $??* ?6   ?? 5      $
??Auto ?6    ? ColumnDefinitions 5
   
   ?? 5      -   5         ??Name$ 
LeftColumn??5      $	??50* ?63   ?? 5      -   5        $ MiddleColumn??5      $	??40* ?65   ?? 5      -   5        $ RightColumn??5      $	??40* ?64   .+?5   
   ?? -   5         ??Name$ GroupBoxGuidType??5      $??0??6-   $??2??6:   $??2,0,2,0q?6J   $??	GUID Type??6[   $??Stretch=?6n   .??5      ? $??4,4,2,4q?6   .+?5      	? -   5         	?Name$ RadioButtonSkuIds??5   :   $??0,2,0,2q?6   $??80??6/   $??_SKU IDs??6U    d? 	IsChecked$
 TrueY?6h   	 ? ToolTip$!	 Click to export the SKU IDs??6?   	? 5      -   5        $ RadioButtonKmsIds??5   ?   $y?	Collapsed=?6?   $??_KMS IDs??5      $!	 Click to export the KMS IDs??6    ??5      ?? 
 ?? ElementName$
 RadioButtonSkuIds?? ?? Path$ Margin???6\   ?? $
 RadioButtonSkuIds??$ Width?5   F   5      5      ?? 5   
   -   5        $ GroupBoxOptions??5      $??0??6,   $??1??69   $??2,0,2,0q?6I   $??Options??6Z   $??Stretch=?6k   .??5      ? $??4,4,2,4q?6   .+?5      ?? -	   5         ??Name$ CheckBoxAddComment??5      $
 TrueY?5   ?   $y?	Collapsed=?6?   $??_Add string GUID??5   )   $`	 ZIf checked all guids are additionally included in their string representation as a comment??6S   ??5      ?? $
 RadioButtonSkuIds??$ Margin???6Y   ?? $
 RadioButtonVlmcsd??$ Width?5   S   ?? 5      -
   5        $ CheckBoxMultiLine??5      $ FalseY?5   ?   $y?Visible=?6?   $??_Multi-line attributes??5   (   $:	 4If checked each attribute appears on a seperate line??6X   ??5      ?? $
 RadioButtonSkuIds??$ Margin???6Y   ?? $
 RadioButtonVlmcsd??$ Width?5   X   ?? 5      -   5        $ CheckBoxBlankLines??5      $
 TrueY?5   ?   $y?Visible=?6?   $??Insert _blank lines??5   )   $C	 =If unchecked does not insert blank lines between XML elements??6V   ??5      ?? $
 RadioButtonSkuIds??$ Margin???6Y   ?? $
 RadioButtonVlmcsd??$ Width?5   V   ?? 5       -   5        $ CheckBoxUseTabs??5!      $
 TrueY?5    ?   $y?Visible=?6?   $??	Use _tabs??5!   &   $@	 :If unchecked uses 4 spaces for indentation instead of tabs??6:   ??5       ?? $
 RadioButtonSkuIds??$ Margin???6Y   ?? $
 RadioButtonVlmcsd??$ Width?5!      ?? 5"      -   5        $ CheckBoxIncludeSku??5#      $
 TrueY?5"   ?   $y?	Collapsed=?6?   $??Include _SKU IDs??5#   )   $/	 )If unchecked SKU IDs will not be exported??6D   ??5"      ?? $
 RadioButtonSkuIds??$ Margin???6Y   ?? $
 RadioButtonVlmcsd??$ Width?5#   x   ?? 5$      -   5        $ CheckBoxIncludeBetaSku??5%      $
 TrueY?5$   ?   $y?	Collapsed=?6?   $??Include _Beta SKU IDs??5%   -   $@	 :If unchecked Beta and Preview SKU IDs will not be exported??6M   ??5$      ?? $
 RadioButtonSkuIds??$ Margin???6Y   ?? $
 RadioButtonVlmcsd??$ Width?5%   ?   ?? 5&      -   5        $ CheckBoxIncludeKms??5'      $
 TrueY?5&   ?   $y?	Collapsed=?6?   $??Include all _KMS IDs??5'   )   $:	 4If unchecked only essential KMS IDs will be exported??6H   ??5&      ?? $
 RadioButtonSkuIds??$ Margin???6Y   ?? $
 RadioButtonVlmcsd??$ Width?5'   ?   ?? 5(      -   5        $ CheckBoxIncludeApp??5)      $
 TrueY?5(   ?   $y?	Collapsed=?6?   $??Include A_PP IDs??5)   )   $/	 )If unchecked APP IDs will not be exported??6D   ??5(      ?? $
 RadioButtonSkuIds??$ Margin???6Y   ?? $
 RadioButtonVlmcsd??$ Width?5)   x   ?? 5*      -   5        $ CheckBoxNoDescription??5+      $ FalseY?5*   ?   $y?	Collapsed=?6?   $??No _description??5+   ,   $8	 2If checked no product description will be included??6F   ??5*      ?? $
 RadioButtonSkuIds??$ Margin???6Y   ?? $
 RadioButtonVlmcsd??$ Width?5+   ?   5,      5-      ?? 5/   
   $??0??6   $??0??6    $??2,0,2,0q?60   $??Format??6A   .??50      ? $??4,4,2,4q?6   .+?51      	? -   5        $ RadioButtonVlmcsd??51   h   $	??180??6\   $??_vlmcsd??6?   $I	 CClick for a format that can be pasted into the kms.c file of vlmcsd??52      ??51      ?? $
 RadioButtonSkuIds??$ Margin?52      	? 53      -   5        $ RadioButtonPyKms??53   ?   $??_py-kms classic??6?   $f	 `Click for a format that can be pasted into the kmsBase.py file of the original py-kms by cyrozap??54      ??53      ?? $
 RadioButtonSkuIds??$ Margin???6\   ?? $
 RadioButtonVlmcsd??$ Width?54   x   	? 55      -   5        $ RadioButtonGenericC??55   ?   $??Generic _C/C++??6?   $&	  Click for a generic C/C++ format??56   0   ??55      ?? $
 RadioButtonSkuIds??$ Margin???6\   ?? $
 RadioButtonVlmcsd??$ Width?56   0   	? 57      -   5        $ RadioButtonGenericCs??57   ?   $??_Generic C♯??6?   $#	 Click for a generic C# format??58   1   ??57      ?? $
 RadioButtonSkuIds??$ Margin???6\   ?? $
 RadioButtonVlmcsd??$ Width?58   1   	? 59      -   5        $ RadioButtonXml??59   ;   $	??228??6   $
 TrueY?6*   $&?? _License Manager database/py-kms??5:      $M	 GClick for a KmsDataBase.xml file as used by License Manager and py-kms ??6V   5;      5<      ?? 5>   
   $??1??6   $??3??6    $??0??64   $??Output??6D   .??5?      ?? -   5         ??Name$ TextBoxOutput??5?      $??Consolas)?6-   $??Stretch=?6C   $??Stretch=?6a    ?? TextWrapping$ NoWrap=?6}   $ ?? HorizontalScrollBarVisibility$
 Auto=?6?   " ?? VerticalScrollBarVisibility$
 Auto=?5@       ?? 
IsReadOnly . $??4,4,2,4q?6@   5A      ? 5C   
   $??8,0,8,4q?6   $??2??6%   $??3??62   $??0??6F   $??Center=?6V   $??Stretch=?6q   $
??Auto??6?    ? Orientation$ 
Horizontal=?6?   .+?5D      ?? -   5         ??Name$ ButtonSaveAsAscii??5D      $	??120??60   $??24??6?   $??0,0,5,0q?6K   $??Save To _File??6\   $ 	 Saves the output to a file??6?   ? 5E      -   5         ?Name$ EncodingPanel??5E      $??Center=?6/   $??Stretch=?6J   $
??Auto??6h   $ 
Horizontal=?6v   .+?5F      ?? $??	Encoding:??6   $??Center=?6,   	? 5G      -   5        $ RadioButtonAnsi??5G      $
 TrueY?67   $
??ANSI??6H   $??Center=?6W   $??0,3,8,0q?6r   $3	 -Encodes the file in the current ANSI codepage??5H      	? 5I      -   5        $ RadioButtonUtf8??5I      $??UTF-8??67   $??Center=?6G   $??0,3,8,0q?6b   $	 Encodes the file in UTF-8??6?   	? 5J      -   5        $ RadioButtonUtf16Le??5J      $??	UTF-16 LE??6:   $??Center=?6N   $??0,3,8,0q?6i   $.	 (Encodes the file in UTF-16 little-endian??6?   5K      5L      5M      5N      