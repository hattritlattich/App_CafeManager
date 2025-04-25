using AppQLQuanCaPhe.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace AppQLQuanCaPhe
{
    /// <summary>
    /// Interaction logic for ProductModuleWindow.xaml
    /// </summary>

    public partial class ProductModuleWindow : Window
    {
        SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True");
        SqlCommand cm = new SqlCommand();
        SqlCommand cm1 = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        string ID = "SP";
        private ProductsUserControl productUserControl;
        public ProductModuleWindow(ProductsUserControl userControl)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            productUserControl = userControl;
            LoadLoai();
            AutoID();
            this.productUserControl = productUserControl;
        }
        public string TbID1Value
        {
            get { return tbID1.Text; }
            set { tbID1.Text = value; }
        }
        public ImageSource ImageValue
        {
            get { return txtimg.Source; }
            set { txtimg.Source = value; }
        }
        public string TbTenSPValue
        {
            get { return tbtsp.Text; }
            set { tbtsp.Text = value; }
        }
        public string TbLoaiSPValue
        {
            get { return cblsp.Text; }
            set { cblsp.Text = value; }
        }
        public string TbThuocTinhValue
        {
            get { return cbkt.Text; }
            set { cbkt.Text = value; }
        }
        public string TbGiaValue
        {
            get { return cbgia.Text; }
            set { cbgia.Text = value; }
        }
       

        public class LoaiSanPham
        {
            public string maLoai { get; set; }
            public string loaiSanPham { get; set; }
        }
        public void LoadLoai()
        {
            List<LoaiSanPham> loaiSanPhams = new List<LoaiSanPham>();

            DataTable dt = dBConnect.getTable("select * from LoaiSanPham");

            foreach (DataRow row in dt.Rows)
            {
                LoaiSanPham loai = new LoaiSanPham
                {
                    maLoai = row["maLoai"].ToString(),
                    loaiSanPham = row["loaiSanPham"].ToString(),
                };

                loaiSanPhams.Add(loai);
            }
            cblsp.ItemsSource = loaiSanPhams;
            cblsp.DisplayMemberPath = "loaiSanPham";
            cblsp.SelectedValuePath = "maLoai";
        }
       private void AutoID()
        {
            cn.Open();
            cm = new SqlCommand("SELECT COUNT(maSanPham) FROM [SanPham]", cn);
            cm1 = new SqlCommand("SELECT maSanPham FROM [SanPham]", cn);
            int i = Convert.ToInt32(cm.ExecuteScalar());
            i++;
            lbID.Text = ID + i.ToString().PadLeft(6, '0');

            SqlDataReader reader = cm1.ExecuteReader();
            while (reader.Read())
            {
                string id = reader["maSanPham"].ToString();
                if (id == lbID.Text)
                {
                    i++;
                    lbID.Text = ID + i.ToString().PadLeft(6, '0');
                }
            }
            cn.Close();
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc muốn hủy không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.No)
            {
                this.Close();
            }
        }

        public string selectedFilePath;

        private void btnTaiLen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                Uri fileuri = new Uri(ofd.FileName);
                txtimg.Source = new BitmapImage(fileuri);
                string sourcefile = ofd.FileName;
                string resourceUri = $"C://Đồ án CNPM//AppQLQuanCaPhe//AppQLQuanCaPhe//Hinhanh/{System.IO.Path.GetFileName(ofd.FileName)}";

                // Check if the file already exists in the destination folder
                if (!System.IO.File.Exists(resourceUri))
                {
                    System.IO.File.Copy(sourcefile, resourceUri, true);
                }

                // Store the selected file path in the class-level variable
                selectedFilePath = System.IO.Path.GetFileName(ofd.FileName);
            }
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            string selectedValue = (string)((ComboBoxItem)cbkt.SelectedItem).Content;
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True"))
                {
                    using (SqlCommand cm = new SqlCommand("INSERT INTO SanPham (maSanPham, tenSanPham, maLoaiSanPham, gia, thuocTinh, anh) VALUES (@maSanPham, @tenSanPham, @maLoaiSanPham, @gia, @thuocTinh, @anh)", cn))
                    {
                        cm.Parameters.AddWithValue("@maSanPham", lbID.Text);
                        cm.Parameters.AddWithValue("@tenSanPham", tbtsp.Text);
                        cm.Parameters.AddWithValue("@maLoaiSanPham", cblsp.SelectedValue.ToString());
                        cm.Parameters.AddWithValue("@gia", double.Parse(cbgia.Text));
                        cm.Parameters.AddWithValue("@thuocTinh", selectedValue);
                        cm.Parameters.AddWithValue("@anh", selectedFilePath);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        AutoID();
                    }
                }
               productUserControl.LoadProduct();
                MessageBox.Show("Sản phẩm đã được lưu thành công", "Thông Báo");
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            string selectedValue = (string)((ComboBoxItem)cbkt.SelectedItem).Content;
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True"))
                {
                    using (SqlCommand cm = new SqlCommand("UPDATE SanPham SET tenSanPham = @tenSanPham, maLoaiSanPham = @maLoaiSanPham, gia = @gia, thuocTinh = @thuocTinh, anh = @anh  WHERE maSanPham = @maSanPham", cn))
                    {
                        cm.Parameters.AddWithValue("@maSanPham", tbID1.Text);
                        cm.Parameters.AddWithValue("@tenSanPham", tbtsp.Text);
                        cm.Parameters.AddWithValue("@maLoaiSanPham", cblsp.SelectedValue.ToString());
                        cm.Parameters.AddWithValue("@gia", double.Parse(cbgia.Text));
                        cm.Parameters.AddWithValue("@thuocTinh", selectedValue);
                        cm.Parameters.AddWithValue("@anh", selectedFilePath);
                        


                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        AutoID();
                    }
                }
                productUserControl.LoadProduct();
                MessageBox.Show("Sản phẩm đã được cập nhật thành công", "Thông Báo");
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc muốn xóa bản ghi này không?", "Xóa Bản Ghi", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                cn.Open();
                cm = new SqlCommand("delete from SanPham where maSanPham like '" + tbID1.Text + "'", cn);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Sản phẩm đã được xóa khỏi danh sách", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            productUserControl.LoadProduct();
            this.Close();

        }

    }
}