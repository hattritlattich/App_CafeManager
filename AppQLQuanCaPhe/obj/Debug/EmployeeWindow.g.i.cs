﻿#pragma checksum "..\..\EmployeeWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "35B7A9F8862BB6A29979919742C27787E215EF5EEF7FEB1D2AEF8BC6D123A6A8"
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
    /// EmployeeWindow
    /// </summary>
    public partial class EmployeeWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\EmployeeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem itmBan;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\EmployeeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem itmHoaDon;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\EmployeeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem itmQuayLai;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\EmployeeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentControl contentControl;
        
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
            System.Uri resourceLocater = new System.Uri("/AppQLQuanCaPhe;component/employeewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\EmployeeWindow.xaml"
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
            this.itmBan = ((System.Windows.Controls.MenuItem)(target));
            
            #line 20 "..\..\EmployeeWindow.xaml"
            this.itmBan.Click += new System.Windows.RoutedEventHandler(this.itmBan_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.itmHoaDon = ((System.Windows.Controls.MenuItem)(target));
            
            #line 25 "..\..\EmployeeWindow.xaml"
            this.itmHoaDon.Click += new System.Windows.RoutedEventHandler(this.itmHoaDon_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 31 "..\..\EmployeeWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Logout_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.itmQuayLai = ((System.Windows.Controls.MenuItem)(target));
            
            #line 36 "..\..\EmployeeWindow.xaml"
            this.itmQuayLai.Click += new System.Windows.RoutedEventHandler(this.QuayLai_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.contentControl = ((System.Windows.Controls.ContentControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

