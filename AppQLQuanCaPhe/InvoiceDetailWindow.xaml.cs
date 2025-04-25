using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppQLQuanCaPhe
{
    public partial class InvoiceDetailWindow : Window
    {

        private string maGiaoDich;
        private string tenBan;
        private decimal tongSoLuongMon;
        private decimal tongTienHang;
        private decimal tongTienSauGiamGia;
        private BillUserControl billUserControl;
        public InvoiceDetailWindow(string maGiaoDich, string tenBan, decimal tongSoLuongMon, decimal tongTienHang, decimal tongTienSauGiamGia)
        {
            InitializeComponent();
            this.maGiaoDich = maGiaoDich;
            this.tenBan = tenBan;
            this.tongSoLuongMon = tongSoLuongMon;
            this.tongTienHang = tongTienHang;
            this.tongTienSauGiamGia = tongTienSauGiamGia;
            LoadInvoiceDetails();
            DisplayInvoiceInfo();
            CenterWindowOnScreen();
            this.billUserControl = billUserControl;
        }
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;

            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        private void DisplayInvoiceInfo()
        {
            TextMaHoaDon.Text = maGiaoDich;
            TextTenBan.Text = tenBan;
            totalQuantityTextBlock.Text = tongSoLuongMon.ToString();
            totalAmountTextBlock.Text = tongTienHang.ToString("N0");
            discountedAmountTextBlock.Text = tongTienSauGiamGia.ToString("N0");
        }

        public class InvoiceDetail
        {
            public string MaSanPham { get; set; }
            public string TenSanPham { get; set; }
            public int SoLuong { get; set; }
            public string Gia { get; set; }
            public string Tong { get; set; }
            public string Size { get; set; }
        }

        private void LoadInvoiceDetails()
        {
            try
            {
                List<InvoiceDetail> details = new List<InvoiceDetail>();

                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                {
                    cn.Open();

                    string query = @"
                    SELECT c.maSanPham, s.tenSanPham, c.soLuong, s.gia, (s.gia * c.soLuong) AS tongTien, s.thuocTinh
                    FROM CTHoaDon c
                    INNER JOIN SanPham s ON c.maSanPham = s.maSanPham
                    WHERE c.maHoaDon = @maGiaoDich";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@maGiaoDich", maGiaoDich);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var soLuong = reader["soLuong"] != DBNull.Value ? Convert.ToInt32(reader["soLuong"]) : 0;
                                var gia = reader["gia"] != DBNull.Value ? Convert.ToDecimal(reader["gia"]) : 0;
                                var tongTien = reader["tongTien"] != DBNull.Value ? Convert.ToDecimal(reader["tongTien"]) : 0;

                                details.Add(new InvoiceDetail
                                {
                                    MaSanPham = reader["maSanPham"].ToString(),
                                    TenSanPham = reader["tenSanPham"].ToString(),
                                    Size = reader["thuocTinh"].ToString(),
                                    SoLuong = soLuong,
                                    Gia = gia.ToString("N0"),
                                    Tong = tongTien.ToString("N0")
                                });
                            }
                        }
                    }
                }

                detailDataGrid.ItemsSource = details;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
        }
        private void HuyDon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Update the database to set the invoice status to canceled (2)
                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                {
                    cn.Open();

                    string updateQuery = "UPDATE HoaDon SET trangThai = 2 WHERE maGiaoDich = @maGiaoDich";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, cn))
                    {
                        cmd.Parameters.AddWithValue("@maGiaoDich", maGiaoDich);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đã hủy đơn thành công!");
                            // Close this window after cancellation
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Không thể hủy đơn. Vui lòng thử lại!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}