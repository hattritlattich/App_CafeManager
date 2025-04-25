using AppQLQuanCaPhe.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace AppQLQuanCaPhe
{
    /// <summary>
    /// Interaction logic for ProductsUserControl.xaml
    /// </summary>
    public partial class ProductsUserControl : UserControl
    {

            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand();
            SqlCommand cm1 = new SqlCommand();
            DBConnect dBConnect = new DBConnect();
            SqlDataReader dr;
            public ProductsUserControl()
            {
                InitializeComponent();
                LoadLoai();
                LoadProduct();
            }
        public class SanPhamModel
        {
            public string MaSP { get; set; }
            public string TenSP { get; set; }
            public string LoaiSP { get; set; }
            public string ThuocTinh { get; set; }
            public string Gia { get; set; }
            public string Anh { get; set; }

        }
        public void LoadProduct()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                {
                    cn.Open();
                    using (SqlCommand cm = new SqlCommand("SELECT s.maSanPham AS MaSP, s.tenSanPham AS TenSP, l.loaiSanPham AS LoaiSP, s.thuocTinh AS ThuocTinh, s.gia AS Gia, s.anh AS Anh  FROM SanPham s INNER JOIN LoaiSanPham l ON l.maLoai = s.maLoaiSanPham ORDER BY CAST(REPLACE(s.maSanPham, 'SP', '') AS INT)", cn))
                    {
                        SqlDataReader dr = cm.ExecuteReader();
                        List<SanPhamModel> sanPhams = new List<SanPhamModel>();

                        while (dr.Read())
                        {
                            SanPhamModel sanPham = new SanPhamModel
                            {
                                MaSP = dr["MaSP"].ToString(),
                                TenSP = dr["TenSP"].ToString(),
                                LoaiSP = dr["LoaiSP"].ToString(),
                                ThuocTinh = dr["ThuocTinh"].ToString(),
                                Gia = Convert.ToDecimal(dr["Gia"]).ToString("#,##0"),
                                Anh = dr["Anh"].ToString()
                            };
                            sanPhams.Add(sanPham);
                        }

                        dgvSanPham.ItemsSource = sanPhams;
                    }
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sản phẩm: " + ex.Message);
            }
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

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            ProductModuleWindow productModule = new ProductModuleWindow(this);
            productModule.btnLuu.Visibility = Visibility.Visible;
            productModule.btnCapNhat.Visibility = Visibility.Collapsed;
            productModule.btnXoa.Visibility = Visibility.Collapsed;
            productModule.ShowDialog();
        }
        private void dgvSanPham_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgvSanPham.SelectedItem != null)
            {
                SanPhamModel selectedProduct = (SanPhamModel)dgvSanPham.SelectedItem;

                ProductModuleWindow productModule = new ProductModuleWindow(this);
                productModule.TbID1Value = selectedProduct.MaSP;
                productModule.TbTenSPValue = selectedProduct.TenSP;
                productModule.TbLoaiSPValue = selectedProduct.LoaiSP;
                productModule.TbThuocTinhValue = selectedProduct.ThuocTinh;
                productModule.TbGiaValue = selectedProduct.Gia;

                string imagePath = "..//..//..//AppQLQuanCaPhe//Hinhanh//" + selectedProduct.Anh;
                productModule.selectedFilePath = selectedProduct.Anh;
                ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
                ImageSource imageSource = (ImageSource)imageSourceConverter.ConvertFromString(imagePath);
                productModule.ImageValue = imageSource;

                productModule.btnLuu.Visibility = Visibility.Collapsed;
                productModule.btnCapNhat.Visibility = Visibility.Visible;
                productModule.btnXoa.Visibility = Visibility.Visible;
                productModule.ShowDialog();
            }
        }
        private void dgvSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tbTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {

            string keyword = tbTimKiem.Text;
            ShowSearchResults(keyword);
        }
        private void ShowSearchResults(string keyword)
        {
            using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
            {
                cn.Open();
                cm = new SqlCommand("SELECT s.maSanPham, s.tenSanPham, l.loaiSanPham, s.thuocTinh, s.gia FROM SanPham s INNER JOIN LoaiSanPham l ON l.maLoai = s.maLoaiSanPham WHERE tenSanPham LIKE @keyword", cn);
                cm.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                dr = cm.ExecuteReader();

                List<SanPhamModel> searchResults = new List<SanPhamModel>();

                while (dr.Read())
                {
                    SanPhamModel sanpham = new SanPhamModel();
                    sanpham.MaSP = dr["maSanPham"].ToString();
                    sanpham.TenSP = dr["tenSanPham"].ToString();
                    sanpham.LoaiSP = dr["loaiSanPham"].ToString();
                    sanpham.ThuocTinh = dr["thuocTinh"].ToString();
                    sanpham.Gia = Convert.ToDecimal(dr["gia"]).ToString("#,##0");
                    searchResults.Add(sanpham);
                }
                cn.Close();
                dgvSanPham.ItemsSource = searchResults;
            }


        }
        public class LoaiSanPham
        {
            public string maLoai { get; set; }
            public string loaiSanPham { get; set; }
        }
        public void LoadLoai()
        {
            List<LoaiSanPham> loaiSanPhams = new List<LoaiSanPham>();

            // Thêm lựa chọn "Tất cả"
            loaiSanPhams.Add(new LoaiSanPham { maLoai = "0", loaiSanPham = "Tất cả loại sản phẩm" });

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

            cbtkl.ItemsSource = loaiSanPhams;
            cbtkl.DisplayMemberPath = "loaiSanPham";
            cbtkl.SelectedValuePath = "maLoai";
            cbtkl.SelectedIndex = 0; // Chọn "Tất cả loại sản phẩm" làm giá trị mặc định
        }
        private void cbtkl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbtkl.SelectedItem != null)
            {
                LoaiSanPham selectedLoaiSP = (LoaiSanPham)cbtkl.SelectedItem;

                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                {
                    cn.Open();

                    string query;
                    SqlCommand cm;

                    if (selectedLoaiSP.maLoai == "0") // Nếu chọn "Tất cả"
                    {
                        query = "SELECT s.maSanPham, s.tenSanPham, l.loaiSanPham, s.thuocTinh, s.gia FROM SanPham s INNER JOIN LoaiSanPham l ON l.maLoai = s.maLoaiSanPham";
                        cm = new SqlCommand(query, cn);
                    }
                    else // Nếu chọn một loại cụ thể
                    {
                        query = "SELECT s.maSanPham, s.tenSanPham, l.loaiSanPham, s.thuocTinh, s.gia FROM SanPham s INNER JOIN LoaiSanPham l ON l.maLoai = s.maLoaiSanPham WHERE l.loaiSanPham = @loaiSanPham";
                        cm = new SqlCommand(query, cn);
                        cm.Parameters.AddWithValue("@loaiSanPham", selectedLoaiSP.loaiSanPham);
                    }

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        List<SanPhamModel> searchResults = new List<SanPhamModel>();

                        while (dr.Read())
                        {
                            SanPhamModel sanpham = new SanPhamModel();
                            sanpham.MaSP = dr["maSanPham"].ToString();
                            sanpham.TenSP = dr["tenSanPham"].ToString();
                            sanpham.LoaiSP = dr["loaiSanPham"].ToString();
                            sanpham.ThuocTinh = dr["thuocTinh"].ToString();
                            sanpham.Gia = Convert.ToDecimal(dr["gia"]).ToString("#,##0");
                            searchResults.Add(sanpham);
                        }

                        dgvSanPham.ItemsSource = searchResults;
                    }
                }
            }
        }
    }
}