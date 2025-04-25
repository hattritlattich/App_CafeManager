using System.Windows;
using System.Windows.Controls;

namespace AppQLQuanCaPhe
{
    public partial class EmployeeWindow : Window
    {
        private string currentUserChucVu;

        public EmployeeWindow(string chucVu)
        {
            InitializeComponent();
            currentUserChucVu = chucVu;
            CheckAdminPermission();
        }
      

        private void LoadTables()
        {
            OpenChildWindow(new POSTablesUserControl());
        }

        private void itmBan_Click(object sender, RoutedEventArgs e)
        {

            OpenChildWindow(new POSTablesUserControl());
        }

     

        private void itmHoaDon_Click(object sender, RoutedEventArgs e)
        {
            OpenChildWindow(new BillUserControl());
        }

        public void ShowMenuUserControl(string tableID)
        {
            OpenChildWindow(new MenuUserControl("DefaultTableID"));
        }

        public void ShowPOSTablesUserControl()
        {
            OpenChildWindow(new POSTablesUserControl());
        }

        private void OpenChildWindow(UserControl childControl)
        {
            contentControl.Content = childControl;
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
        private void CheckAdminPermission()
        {
            if (currentUserChucVu == "Admin")
            {
                itmQuayLai.Visibility = Visibility.Visible;
            }
            else
            {
                itmQuayLai.Visibility = Visibility.Collapsed;
            }
        }

        private void QuayLai_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); 
            mainWindow.Show(); 

            this.Close();
        }

    }
}
