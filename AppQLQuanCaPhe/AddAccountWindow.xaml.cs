using AppQLQuanCaPhe.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppQLQuanCaPhe
{
    /// <summary>
    /// Interaction logic for AddAccountWindow.xaml
    /// </summary>
    public partial class AddAccountWindow : Window
    {

        SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True");
        SqlCommand cm = new SqlCommand();
        SqlCommand cm1 = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        string ID = "TK";
        private AccCountUserControl accCountUserControl;
        public AddAccountWindow(AccCountUserControl userControl)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            accCountUserControl = userControl;
            AutoID();
        }
        public class TaiKhoanModel
        {
            public string MaTK { get; set; }
            public string TenTK { get; set; }
            public string MatKhau { get; set; }
            public string HoTen { get; set; }
            public int CCCD { get; set; }
            public string ChucVu { get; set; }
            public string GioiTinh { get; set; }
            public int SDT { get; set; }
            public string Email { get; set; }
        }
        private void AutoID()
        {
            cn.Open();
            cm = new SqlCommand("SELECT COUNT(id) FROM [TaiKhoan]", cn);
            cm1 = new SqlCommand("SELECT id FROM [TaiKhoan]", cn);
            int i = Convert.ToInt32(cm.ExecuteScalar());
            i++;
            lbID.Text = ID + i.ToString().PadLeft(6, '0');

            SqlDataReader reader = cm1.ExecuteReader();
            while (reader.Read())
            {
                string id = reader["id"].ToString();
                if (id == lbID.Text)
                {
                    i++;
                    lbID.Text = ID + i.ToString().PadLeft(6, '0');
                }
            }
            cn.Close();
        }


        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            string selectedValue = cbcv.SelectedValue?.ToString();
            string selectedValue1 = cbgt.SelectedValue?.ToString();
            try
            {
                string password = tbmakhau.Password;
                string confirmPassword = tbmakhau1.Password;

                // Kiểm tra mật khẩu có đủ yêu cầu không
                if (!IsStrongPassword(password))
                {
                    MessageBox.Show("Mật khẩu phải chứa ít nhất một chữ hoa, một chữ thường, một số và một ký tự đặc biệt.", "Thông Báo");
                    return;
                }

                // Kiểm tra mật khẩu nhập lại
                if (password != confirmPassword)
                {
                    MessageBox.Show("Mật khẩu nhập lại không khớp. Vui lòng nhập lại mật khẩu.", "Thông Báo");
                    return;
                }

                // Kiểm tra các trường thông tin đã nhập đầy đủ
                if (string.IsNullOrWhiteSpace(tbttk.Text) ||
                    string.IsNullOrWhiteSpace(tbht.Text) ||
                    string.IsNullOrWhiteSpace(tbsdt.Text) ||
                    string.IsNullOrWhiteSpace(tbEmail.Text) ||
                    string.IsNullOrWhiteSpace(tbCCCD.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin tài khoản.", "Thông Báo");
                    return;
                }

                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True"))
                {
                    string checkQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE taiKhoan = @taiKhoan";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, cn))
                    {
                        checkCmd.Parameters.AddWithValue("@taiKhoan", tbttk.Text.Trim());
                        cn.Open();
                        int existingCount = (int)checkCmd.ExecuteScalar();
                        cn.Close();

                        if (existingCount > 0)
                        {
                            MessageBox.Show("Tên tài khoản đã tồn tại. Vui lòng chọn tên tài khoản khác.", "Thông Báo");
                            return; // Không tiếp tục thêm mới nếu tên tài khoản đã tồn tại
                        }
                    }

                    // Thực hiện INSERT tài khoản mới
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO TaiKhoan (id, taiKhoan, matKhau, chucVu, tenDayDu, gioiTinh, Email, SDT, CCCD) VALUES (@id, @taiKhoan, @matKhau, @chucVu, @tenDayDu, @gioiTinh, @Email, @SDT, @CCCD)", cn))
                    {
                        cmd.Parameters.AddWithValue("@id", lbID.Text);
                        cmd.Parameters.AddWithValue("@taiKhoan", tbttk.Text);
                        cmd.Parameters.AddWithValue("@matKhau", EncryptString(password));
                        cmd.Parameters.AddWithValue("@chucVu", cbcv.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@tenDayDu", EncryptString(tbht.Text));
                        cmd.Parameters.AddWithValue("@gioiTinh", EncryptString(cbgt.SelectedValue.ToString()));
                        cmd.Parameters.AddWithValue("@Email", EncryptString(tbEmail.Text));
                        cmd.Parameters.AddWithValue("@SDT", EncryptString(tbsdt.Text));
                        cmd.Parameters.AddWithValue("@CCCD", EncryptString(tbCCCD.Text));

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                        AutoID();
                    }
                }

                accCountUserControl.LoadAccount();

                MessageBox.Show("Thêm tài khoản thành công", "Thông Báo");

                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    parentWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private bool IsStrongPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    

        private static readonly string EncryptionKey = "123456789012345678901234"; 

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


        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
