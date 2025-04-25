using AppQLQuanCaPhe.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
using static AppQLQuanCaPhe.TablesUserControl;

namespace AppQLQuanCaPhe
{
    /// <summary>
    /// Interaction logic for CategaryUserControl.xaml
    /// </summary>
    public partial class CategaryUserControl : UserControl
    {
        SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True");
        SqlCommand cm = new SqlCommand();
        SqlCommand cm1 = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        string ID = "LSP";
        public CategaryUserControl()
        {
            InitializeComponent();

            LoadLoai();
            AutoID();
        }
        public class LoaiModel
        {
            public string MaLoai { get; set; }
            public string TenLoai { get; set; }
        }
        private void AutoID()
        {
            cn.Open();
            cm = new SqlCommand("SELECT COUNT(maLoai) FROM [LoaiSanPham]", cn);
            cm1 = new SqlCommand("SELECT maLoai FROM [LoaiSanPham]", cn);
            int i = Convert.ToInt32(cm.ExecuteScalar());
            i++;
            lbID.Text = ID + i.ToString().PadLeft(6, '0');

            SqlDataReader reader = cm1.ExecuteReader();
            while (reader.Read())
            {
                string id = reader["maLoai"].ToString();
                if (id == lbID.Text)
                {
                    i++;
                    lbID.Text = ID + i.ToString().PadLeft(6, '0');
                }
            }
            cn.Close();
        }

        public void LoadLoai()
        {
            cn.Open();
            cm = new SqlCommand("SELECT maLoai, loaiSanPham FROM LoaiSanPham ORDER BY CAST(REPLACE(maLoai, 'LSP', '') AS INT) ASC", cn);
            dr = cm.ExecuteReader();
            List<LoaiModel> LoaiList = new List<LoaiModel>();
            while (dr.Read())
            {
                LoaiModel loai = new LoaiModel();
                loai.MaLoai = dr["maLoai"].ToString();
                loai.TenLoai = dr["loaiSanPham"].ToString();
                LoaiList.Add(loai);
            }
            dgvLoai.ItemsSource = LoaiList;
            dr.Close();
            cn.Close();


        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có chắc muốn lưu loại sản phẩm này không?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True"))
                    {
                        cn.Open();

                        using (SqlCommand cm = new SqlCommand("insert into LoaiSanPham(maLoai, loaiSanPham, trangThai) values (@maLoai, @loaiSanPham, @trangThai)", cn))
                        {
                            cm.Parameters.AddWithValue("@maLoai", lbID.Text);
                            cm.Parameters.AddWithValue("@loaiSanPham", txtTenLoai.Text);
                            cm.Parameters.AddWithValue("@trangThai", "Đang hoạt động");
                            cm.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Loại sản phẩm đã được thêm", "Thông Báo");
                    AutoID();
                    LoadLoai();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvLoai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvLoai.SelectedItem != null)
            {
                LoaiModel selectedBan = (LoaiModel)dgvLoai.SelectedItem;
                int rowIndex = dgvLoai.SelectedIndex;
                lbID.Text = selectedBan.MaLoai.ToString();
                tbmaLoai.Text = selectedBan.MaLoai.ToString();
                txtTenLoai.Text = selectedBan.TenLoai.ToString();
                LoadLoai();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc muốn cập nhật loại sản phẩm không?", "Cập Nhật Bản Ghi", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True"))
                    {
                        cn.Open();
                        using (SqlCommand cm = new SqlCommand("UPDATE LoaiSanPham SET loaiSanPham = @tenLoai WHERE maLoai = @maLoai", cn))
                        {
                            cm.Parameters.AddWithValue("@tenLoai", txtTenLoai.Text);
                            cm.Parameters.AddWithValue("@maLoai", tbmaLoai.Text);
                            cm.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Loại sản phẩm đã được cập nhật", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadLoai(); // Assuming LoadLoai() is a method to refresh the list of categories
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa bàn này không?", "Xóa Bản Ghi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True"))
                {
                    cn.Open();
                    using (SqlCommand cm = new SqlCommand("DELETE FROM LoaiSanPham WHERE maLoai = @id", cn))
                    {
                        cm.Parameters.AddWithValue("@id", tbmaLoai.Text);
                        cm.ExecuteNonQuery();
                    }
                    cn.Close();
                }
                MessageBox.Show("Loại sản phẩm đã được xóa khỏi danh sách", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadLoai();
            }
        }

        private void btnKhoiTao_Click(object sender, RoutedEventArgs e)
        {
            AutoID();
            tbmaLoai.Text = "Mã tự động";
            txtTenLoai.Text = "";
        }

        private void tbTimKiem_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string keyword = tbTimKiem.Text;
            ShowSearchResults(keyword);
        }
        private void ShowSearchResults(string keyword)
        {
            cn.Open();
            cm = new SqlCommand("SELECT maLoai, loaiSanPham FROM LoaiSanPham WHERE loaiSanPham LIKE @keyword", cn);
            cm.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
            dr = cm.ExecuteReader();

            List<LoaiModel> searchResults = new List<LoaiModel>();

            while (dr.Read())
            {
                LoaiModel loai = new LoaiModel();
                loai.MaLoai = dr["maLoai"].ToString();
                loai.TenLoai = dr["loaiSanPham"].ToString();
                searchResults.Add(loai);
            }

            cn.Close();

            dgvLoai.ItemsSource = searchResults;
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
    }

}
