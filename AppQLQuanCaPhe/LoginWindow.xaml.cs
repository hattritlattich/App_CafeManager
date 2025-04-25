using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Shapes;
using System.Xml.Linq;
using AppQLQuanCaPhe.Model;
using System.Security.Cryptography;

namespace AppQLQuanCaPhe
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True");
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        public LoginWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tbTaiKhoan.Focus();
        }
        private static readonly string EncryptionKey = "123456789012345678901234"; // Khóa 24 bytes, thay đổi thành khóa bảo mật của bạn

        public static string EncryptString(string input)
        {
            try
            {
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider())
                {
                    tripleDES.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                    tripleDES.Mode = CipherMode.ECB;
                    tripleDES.Padding = PaddingMode.PKCS7;

                    ICryptoTransform transform = tripleDES.CreateEncryptor();

                    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                    byte[] encryptedBytes = transform.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                    return Convert.ToBase64String(encryptedBytes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while encrypting: " + ex.Message);
            }
        }

        public static string DecryptString(string input)
        {
            try
            {
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider())
                {
                    tripleDES.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                    tripleDES.Mode = CipherMode.ECB;
                    tripleDES.Padding = PaddingMode.PKCS7;

                    ICryptoTransform transform = tripleDES.CreateDecryptor();

                    byte[] inputBytes = Convert.FromBase64String(input);
                    byte[] decryptedBytes = transform.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while decrypting: " + ex.Message);
            }
        }

        public string _pass = "";
        private void btnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            string _username = "", _name = "", _role = "";
            string _accountId ;
            try
            {
                bool found;
                cn.Open();
                cm = new SqlCommand("select * from TaiKhoan where taikhoan=@taikhoan and matKhau=@matkhau", cn);
                cm.Parameters.AddWithValue("@taikhoan", tbTaiKhoan.Text);
                cm.Parameters.AddWithValue("@matKhau", EncryptString(pbMatKhau.Password));
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    found = true;
                    _accountId = dr["id"].ToString();
                    _username = dr["taikhoan"].ToString();
                    _name = DecryptString(dr["tenDayDu"].ToString());
                    _role = dr["chucVu"].ToString();
                    _pass = DecryptString(dr["matKhau"].ToString());
                }
                else
                {
                    found = false;
                }
                dr.Close();
                cn.Close();

                if (found)
                {
                    if (_role == "Nhân Viên")
                    {
                        MessageBox.Show("Chào Mừng " + _name, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        tbTaiKhoan.Clear();
                        pbMatKhau.Clear();
                        this.Hide();
                        EmployeeWindow employee = new EmployeeWindow(_role);
                        employee.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Chào Mừng " + _name, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        tbTaiKhoan.Clear();
                        pbMatKhau.Clear();
                        this.Hide();
                        MainWindow admin = new MainWindow();
                        admin.ShowDialog();
                    }


                }
                else
                {
                    MessageBox.Show("Sai mật khẩu hoặc tên đăng nhập", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát ứng dụng", "Thông Báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
