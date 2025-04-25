using AppQLQuanCaPhe.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using static AppQLQuanCaPhe.ProductUserControl;

namespace AppQLQuanCaPhe
{
    /// <summary>
    /// Interaction logic for AccCountUserControl.xaml
    /// </summary>
    public partial class AccCountUserControl : UserControl
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlCommand cm1 = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        public AccCountUserControl()
        {
            InitializeComponent();
            LoadAccount();
        }
        public class TaiKhoanModel
        {
            public string Id { get; set; }
            public string TaiKhoan { get; set; }
            public string TenDayDu { get; set; }
            public string ChucVu { get; set; }
            public string GioiTinh { get; set; }
            public string Email { get; set; }
            public string SDT { get; set; }
            public string CCCD { get; set; }
        }
        public void LoadAccount()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                {
                    cn.Open();
                    using (SqlCommand cm = new SqlCommand("SELECT id, taiKhoan, chucVu, tenDayDu, gioiTinh, Email, SDT, CCCD FROM TaiKhoan", cn))
                    {
                        SqlDataReader dr = cm.ExecuteReader();
                        List<TaiKhoanModel> accounts = new List<TaiKhoanModel>();

                        while (dr.Read())
                        {
                            TaiKhoanModel account = new TaiKhoanModel
                            {
                                Id = dr["id"].ToString(),
                                TaiKhoan = dr["taiKhoan"].ToString(),
                                TenDayDu = DecryptString(dr["tenDayDu"].ToString()),
                                ChucVu = dr["chucVu"].ToString(),
                                GioiTinh = DecryptString(dr["gioiTinh"].ToString()),
                                Email = DecryptString(dr["Email"].ToString()), 
                                SDT = DecryptString(dr["SDT"].ToString()), 
                                CCCD = DecryptString(dr["CCCD"].ToString())
                            };
                            accounts.Add(account);
                        }

                        dgvTaiKhoan.ItemsSource = accounts;
                    }
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading accounts: " + ex.Message);
            }
        }
        private static readonly string EncryptionKey = "123456789012345678901234";
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
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            AddAccountWindow accCountUserControl = new AddAccountWindow(this);
            accCountUserControl.ShowDialog();
        }
        private void detailDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            bool isEvenRow = e.Row.GetIndex() % 2 == 0;

            
            if (isEvenRow)
            {
                e.Row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDC7B7")); 
            }
            else
            {
                e.Row.Background = Brushes.White;
            }

            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            e.Row.HeaderStyle = new Style(typeof(DataGridRowHeader));
            e.Row.HeaderStyle.Setters.Add(new Setter(VerticalContentAlignmentProperty, VerticalAlignment.Center));
            e.Row.HeaderStyle.Setters.Add(new Setter(FontSizeProperty, 14.0));
        }
        private void dgvTaiKhoan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
