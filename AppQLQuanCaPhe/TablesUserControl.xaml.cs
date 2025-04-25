using AppQLQuanCaPhe.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppQLQuanCaPhe
{
    /// <summary>
    /// Interaction logic for TablesUserControl.xaml
    /// </summary>
    public partial class TablesUserControl : UserControl
    {
        SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True");
        SqlCommand cm = new SqlCommand();
        SqlCommand cm1 = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        string ID = "B";
        public TablesUserControl()
        {
            InitializeComponent();

            LoadTable();
            AutoID();
        }

        public class BanModel
        {
            public string MaBan { get; set; }
            public string TenBan { get; set; }
        }
        private void AutoID()
        {
            cn.Open();
            cm = new SqlCommand("SELECT COUNT(maBan) FROM [Ban]", cn);
            cm1 = new SqlCommand("SELECT maBan FROM [Ban]", cn);
            int i = Convert.ToInt32(cm.ExecuteScalar());
            i++;
            lbID.Text = ID + i.ToString().PadLeft(6, '0');

            SqlDataReader reader = cm1.ExecuteReader();
            while (reader.Read())
            {
                string id = reader["maBan"].ToString();
                if (id == lbID.Text)
                {
                    i++;
                    lbID.Text = ID + i.ToString().PadLeft(6, '0');
                }
            }
            cn.Close();
        }

        public void LoadTable()
        {
            cn.Open();
            cm = new SqlCommand("SELECT maBan, tenBan FROM Ban ORDER BY CAST(REPLACE(maBan, 'B', '') AS INT) ASC", cn);
            dr = cm.ExecuteReader();
            List<BanModel> banList = new List<BanModel>();
            while (dr.Read())
            {
                BanModel ban = new BanModel();
                ban.MaBan = dr["maBan"].ToString();
                ban.TenBan = dr["tenBan"].ToString();
                banList.Add(ban);
            }
            dgvBan.ItemsSource = banList;
            dr.Close();
            cn.Close();


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
            try
            {
                if (MessageBox.Show("Bạn có chắc muốn lưu bàn này không?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True"))
                    {
                        cn.Open();

                        using (SqlCommand cm = new SqlCommand("insert into Ban(maBan, tenBan, trangThai) values (@maBan, @tenBan, @trangThai)", cn))
                        {
                            cm.Parameters.AddWithValue("@maBan", lbID.Text);
                            cm.Parameters.AddWithValue("@tenBan", txtTenBan.Text);
                            cm.Parameters.AddWithValue("@trangThai", "Trống");
                            cm.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Bàn đã được thêm", "Thông Báo");
                    AutoID();
                    LoadTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvBan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvBan.SelectedItem != null)
            {
                BanModel selectedBan = (BanModel)dgvBan.SelectedItem;
                int rowIndex = dgvBan.SelectedIndex;
                lbID.Text = selectedBan.MaBan.ToString();
                tbmaBan.Text = selectedBan.MaBan.ToString();
                txtTenBan.Text = selectedBan.TenBan.ToString();
                LoadTable();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc muốn cập nhật bàn không?", "Cập Nhật Bản Ghi", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;TrustServerCertificate=True"))
                    {
                        cn.Open();
                        using (SqlCommand cm = new SqlCommand("UPDATE Ban SET tenBan = @tenBan WHERE maBan LIKE @maBan", cn))
                        {
                            cm.Parameters.AddWithValue("@tenBan", txtTenBan.Text);
                            cm.Parameters.AddWithValue("@maBan", tbmaBan.Text);
                            cm.ExecuteNonQuery();
                        }
                        cn.Close();
                    }

                    MessageBox.Show("Bàn đã được cập nhật", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadTable();
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
                    using (SqlCommand cm = new SqlCommand("DELETE FROM Ban WHERE maBan = @id", cn))
                    {
                        cm.Parameters.AddWithValue("@id", tbmaBan.Text);
                        cm.ExecuteNonQuery();
                    }
                    cn.Close();
                }
                MessageBox.Show("Bàn đã được xóa khỏi danh sách", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadTable();
            }
        }

        private void btnKhoiTao_Click(object sender, RoutedEventArgs e)
        {
            AutoID();
            tbmaBan.Text = "Mã tự động";
            txtTenBan.Text = "";
        }

        private void tbTimKiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = tbTimKiem.Text;
            ShowSearchResults(keyword);
        }

        private void ShowSearchResults(string keyword)
        {
            cn.Open();
            cm = new SqlCommand("SELECT maBan, tenBan FROM Ban WHERE tenBan LIKE @keyword", cn);
            cm.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
            dr = cm.ExecuteReader();

            List<BanModel> searchResults = new List<BanModel>();

            while (dr.Read())
            {
                BanModel ban = new BanModel();
                ban.MaBan = dr["maBan"].ToString();
                ban.TenBan = dr["tenBan"].ToString();
                searchResults.Add(ban);
            }

            cn.Close();

            dgvBan.ItemsSource = searchResults;
        }


    }
}
