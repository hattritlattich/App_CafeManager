using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace AppQLQuanCaPhe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //LoadForm();
        }
        public void OpenChildWindow(UserControl childControl)
        {
            contentControl.Content = childControl;
        }
        public void LoadForm()
        {
            var childControl = new GeneralUserControl();
            OpenChildWindow(childControl);
        }
        private void itmTongQuan_Click(object sender, RoutedEventArgs e)
        {
            var childControl = new GeneralUserControl();
            OpenChildWindow(childControl);
        }

        private void itmDanhMuc_Click(object sender, RoutedEventArgs e)
        {
            var childControl = new ProductsUserControl();
            OpenChildWindow(childControl);
        }

        private void itmLoaisanpham_Click(object sender, RoutedEventArgs e)
        {
            var childControl = new CategaryUserControl();
            OpenChildWindow(childControl);
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void itmHoaDon_Click(object sender, RoutedEventArgs e)
        {
            var childControl = new BillUserControl();
            OpenChildWindow(childControl);
        }

        private void itmBan_Click(object sender, RoutedEventArgs e)
        {
            var childControl = new TablesUserControl();
            OpenChildWindow(childControl);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
           
            EmployeeWindow em = new EmployeeWindow("Admin");

            
            this.Close();

            em.ShowDialog();
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var childControl = new AccCountUserControl();
            OpenChildWindow(childControl);
           
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }


}
