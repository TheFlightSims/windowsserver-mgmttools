#pragma checksum "..\..\InstallKmsKeys.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "2B37B5D3A3AA344AFC3386551D520B517A5B6C1AFBF26C69AE07AD830CC9BC40"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using HGM.Hotbird64.LicenseManager;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace HGM.Hotbird64.LicenseManager {
    
    
    /// <summary>
    /// InstallKmsKeys
    /// </summary>
    public partial class InstallKmsKeys : HGM.Hotbird64.LicenseManager.ScalableWindow, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 7 "..\..\InstallKmsKeys.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid TopElement;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\InstallKmsKeys.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataGrid;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\InstallKmsKeys.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn PartialProductKeyColumn;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\InstallKmsKeys.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn LicenseStatusColumn;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\InstallKmsKeys.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn SelectColumn;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\InstallKmsKeys.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn ResultColumn;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\InstallKmsKeys.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button InstallButton;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\InstallKmsKeys.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CancelButton;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\InstallKmsKeys.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock LabelStatus;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\InstallKmsKeys.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar ProgressBar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/LicenseManager;component/installkmskeys.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\InstallKmsKeys.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.TopElement = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.DataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 17 "..\..\InstallKmsKeys.xaml"
            this.DataGrid.LoadingRow += new System.EventHandler<System.Windows.Controls.DataGridRowEventArgs>(this.DataGrid_LoadingRow);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PartialProductKeyColumn = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 4:
            this.LicenseStatusColumn = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 5:
            this.SelectColumn = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 8:
            this.ResultColumn = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 9:
            this.InstallButton = ((System.Windows.Controls.Button)(target));
            
            #line 96 "..\..\InstallKmsKeys.xaml"
            this.InstallButton.Click += new System.Windows.RoutedEventHandler(this.InstallButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.CancelButton = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.LabelStatus = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            this.ProgressBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 6:
            
            #line 66 "..\..\InstallKmsKeys.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.Install_Click);
            
            #line default
            #line hidden
            break;
            case 7:
            
            #line 68 "..\..\InstallKmsKeys.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Click += new System.Windows.RoutedEventHandler(this.Install_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

