﻿#pragma checksum "..\..\ProductUserControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "536B694EE0C88366C12F1ADB2D208B3766D4A8E537D624442B38100C6A655944"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AppQLQuanCaPhe;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
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


namespace AppQLQuanCaPhe {
    
    
    /// <summary>
    /// ProductUserControl
    /// </summary>
    public partial class ProductUserControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\ProductUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbtkl;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\ProductUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbTimKiem;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\ProductUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnThem;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\ProductUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgvSanPham;
        
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
            System.Uri resourceLocater = new System.Uri("/AppQLQuanCaPhe;component/productusercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ProductUserControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.cbtkl = ((System.Windows.Controls.ComboBox)(target));
            
            #line 37 "..\..\ProductUserControl.xaml"
            this.cbtkl.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbtkl_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tbTimKiem = ((System.Windows.Controls.TextBox)(target));
            
            #line 51 "..\..\ProductUserControl.xaml"
            this.tbTimKiem.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.tbTimKiem_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnThem = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\ProductUserControl.xaml"
            this.btnThem.Click += new System.Windows.RoutedEventHandler(this.btnThem_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.dgvSanPham = ((System.Windows.Controls.DataGrid)(target));
            
            #line 78 "..\..\ProductUserControl.xaml"
            this.dgvSanPham.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dgvSanPham_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 78 "..\..\ProductUserControl.xaml"
            this.dgvSanPham.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.dgvSanPham_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

