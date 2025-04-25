using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using static AppQLQuanCaPhe.InvoiceDetailWindow;

namespace AppQLQuanCaPhe
{
    public partial class BillUserControl : UserControl
    {

        public BillUserControl()
        {
            InitializeComponent();
            LoadInvoiceData();
        }
        private void tbSearchInvoice_TextChanged(object sender, TextChangedEventArgs e)
        {
            string maHoaDon = tbSearchInvoice.Text.Trim();


            ReloadInvoiceDetails(maHoaDon);
        }
        private void ReloadInvoiceDetails(string maGiaoDich)
        {
            try
            {
                List<Invoice> filteredInvoices = new List<Invoice>();

                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                {
                    cn.Open();

                    string query = @"
                SELECT h.maGiaoDich, h.maBan, b.tenBan, h.giamGia, h.ngayBan, h.trangThai
                FROM HoaDon h
                INNER JOIN Ban b ON h.maBan = b.maBan
                WHERE h.maGiaoDich LIKE @maGiaoDich";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@maGiaoDich", $"%{maGiaoDich}%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                filteredInvoices.Add(new Invoice
                                {
                                    MaGiaoDich = reader["maGiaoDich"].ToString(),
                                    TenBan = reader["tenBan"].ToString(),
                                    TotalAmount = 0, // Tổng tiền sẽ được tính từ bảng CTHoaDon
                                    DiscountPercent = Convert.ToDecimal(reader["giamGia"]) / 100,
                                    NgayBan = Convert.ToDateTime(reader["ngayBan"]),
                                    TrangThai = Convert.ToInt32(reader["trangThai"]) == 1 ? "Đã thanh toán" : "Đơn đã hủy"
                                });
                            }
                        }
                    }
                }

                // Tính tổng tiền cho mỗi hóa đơn
                foreach (var invoice in filteredInvoices)
                {
                    decimal totalAmount = 0;

                    using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                    {
                        cn.Open();

                        string query = @"
                    SELECT SUM(c.tongTien) AS totalAmount
                    FROM CTHoaDon c
                    INNER JOIN SanPham s ON c.maSanPham = s.maSanPham
                    WHERE c.maHoaDon = @maGiaoDich";

                        using (SqlCommand cmd = new SqlCommand(query, cn))
                        {
                            cmd.Parameters.AddWithValue("@maGiaoDich", invoice.MaGiaoDich);

                            var result = cmd.ExecuteScalar();
                            totalAmount = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                        }
                    }

                    decimal discountedAmount = totalAmount * (1 - invoice.DiscountPercent);

                    invoice.TotalAmount = discountedAmount;
                }

                invoiceDataGrid.ItemsSource = filteredInvoices; // Assign filtered invoices to DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
        }
        private void FilterByDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime? fromDate = dpFromDate.SelectedDate;
                DateTime? toDate = dpToDate.SelectedDate;

                List<Invoice> filteredInvoices = new List<Invoice>();

                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                {
                    cn.Open();

                    // Lấy thông tin hóa đơn từ bảng HoaDon
                    string queryInvoice = @"
                SELECT h.maGiaoDich, h.maBan, h.giamGia, h.ngayBan, h.trangThai, b.tenBan
                FROM HoaDon h
                INNER JOIN Ban b ON h.maBan = b.maBan
                WHERE (@FromDate IS NULL OR h.ngayBan >= @FromDate)
                AND (@ToDate IS NULL OR h.ngayBan <= @ToDate)";

                    using (SqlCommand cmd = new SqlCommand(queryInvoice, cn))
                    {
                        cmd.Parameters.AddWithValue("@FromDate", fromDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", toDate ?? (object)DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var invoice = new Invoice
                                {
                                    MaGiaoDich = reader["maGiaoDich"].ToString(),
                                    TenBan = reader["tenBan"].ToString(),
                                    DiscountPercent = Convert.ToDecimal(reader["giamGia"]) / 100,
                                    NgayBan = Convert.ToDateTime(reader["ngayBan"]),
                                    TrangThai = Convert.ToInt32(reader["trangThai"]) == 1 ? "Đã thanh toán" : "Đơn đã hủy"
                                };

                                // Tính tổng tiền cho hóa đơn
                                decimal totalAmount = 0;

                                using (SqlConnection cnDetail = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                                {
                                    cnDetail.Open();

                                    string queryDetail = @"
                                SELECT SUM(c.tongTien) AS totalAmount
                                FROM CTHoaDon c
                                WHERE c.maHoaDon = @maGiaoDich";

                                    using (SqlCommand cmdDetail = new SqlCommand(queryDetail, cnDetail))
                                    {
                                        cmdDetail.Parameters.AddWithValue("@maGiaoDich", invoice.MaGiaoDich);

                                        var result = cmdDetail.ExecuteScalar();
                                        totalAmount = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                                    }
                                }

                                // Áp dụng giảm giá
                                decimal discountedAmount = totalAmount * (1 - invoice.DiscountPercent);
                                invoice.TotalAmount = discountedAmount;

                                filteredInvoices.Add(invoice);
                            }
                        }
                    }
                }

                invoiceDataGrid.ItemsSource = filteredInvoices;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi lọc hóa đơn: {ex.Message}");
            }
        }

        private void LoadInvoiceData()
        {
            try
            {
                List<Invoice> invoices = new List<Invoice>();

                using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                {
                    cn.Open();

                    string query = @"
                SELECT h.maGiaoDich, h.maBan, b.tenBan, h.giamGia, h.ngayBan, h.trangThai
                FROM HoaDon h
                INNER JOIN Ban b ON h.maBan = b.maBan";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                invoices.Add(new Invoice
                                {
                                    MaGiaoDich = reader["maGiaoDich"].ToString(),
                                    TenBan = reader["tenBan"].ToString(),
                                    TotalAmount = 0, // Tổng tiền sẽ được tính từ bảng CTHoaDon
                                    DiscountPercent = Convert.ToDecimal(reader["giamGia"]) / 100,
                                    NgayBan = Convert.ToDateTime(reader["ngayBan"]),
                                    TrangThai = Convert.ToInt32(reader["trangThai"]) == 1 ? "Đã thanh toán" : "Đơn đã hủy"
                                });
                            }
                        }
                    }
                }

                // Tính tổng tiền cho mỗi hóa đơn
                foreach (var invoice in invoices)
                {
                    decimal totalAmount = 0;

                    using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                    {
                        cn.Open();

                        string query = @"
                    SELECT SUM(c.tongTien) AS totalAmount
                    FROM CTHoaDon c
                    INNER JOIN SanPham s ON c.maSanPham = s.maSanPham
                    WHERE c.maHoaDon = @maGiaoDich";

                        using (SqlCommand cmd = new SqlCommand(query, cn))
                        {
                            cmd.Parameters.AddWithValue("@maGiaoDich", invoice.MaGiaoDich);

                            var result = cmd.ExecuteScalar();
                            totalAmount = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                        }
                    }

                    decimal discountedAmount = totalAmount * (1 - invoice.DiscountPercent);

                    invoice.TotalAmount = discountedAmount;
                }

                invoiceDataGrid.ItemsSource = invoices;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void InvoiceDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedInvoice = invoiceDataGrid.SelectedItem as Invoice;
            if (selectedInvoice != null)
            {
                // Calculate total quantity, total amount, discounted amount
                decimal totalQuantity = 0;
                decimal totalAmount = 0;

                // Load details of the selected invoice
                try
                {
                    List<InvoiceDetail> details = new List<InvoiceDetail>();

                    using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                    {
                        cn.Open();

                        string query = @"
                            SELECT c.maSanPham, s.tenSanPham, s.gia, c.soLuong, (s.gia * c.soLuong) AS tongTien, s.thuocTinh
                            FROM CTHoaDon c
                            INNER JOIN SanPham s ON c.maSanPham = s.maSanPham
                            WHERE c.maHoaDon = @maGiaoDich";

                        using (SqlCommand cmd = new SqlCommand(query, cn))
                        {
                            cmd.Parameters.AddWithValue("@maGiaoDich", selectedInvoice.MaGiaoDich);
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

                                    totalQuantity += soLuong;
                                    totalAmount += tongTien;
                                }
                            }
                        }

                        // Calculate discounted amount
                        decimal discountedAmount = totalAmount * (1 - selectedInvoice.DiscountPercent);

                        // Open the InvoiceDetailWindow with necessary parameters
                        var detailWindow = new InvoiceDetailWindow(selectedInvoice.MaGiaoDich, selectedInvoice.TenBan, totalQuantity, totalAmount, discountedAmount);
                        detailWindow.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
                }
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
    }

    public class Invoice
    {
        public string MaGiaoDich { get; set; }
        public string TenBan { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime NgayBan { get; set; }
        public string TrangThai { get; set; }
    }
   
}