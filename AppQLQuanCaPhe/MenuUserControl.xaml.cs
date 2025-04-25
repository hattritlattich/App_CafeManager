using AppQLQuanCaPhe.Model;
using Guna.UI2.WinForms.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static AppQLQuanCaPhe.CategaryUserControl;
using static AppQLQuanCaPhe.ProductsUserControl;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AppQLQuanCaPhe
{
    /// <summary>
    /// Interaction logic for MenuUserControl.xaml
    /// </summary>
    public partial class MenuUserControl : System.Windows.Controls.UserControl
    {

        public ObservableCollection<InvoiceItem> InvoiceItems { get; set; }
        private DispatcherTimer clockTimer;


        public MenuUserControl(string tableID)
        {
            InitializeComponent();
            StartClock();

            tableTextBlock.Text = tableID;
            InvoiceItems = new ObservableCollection<InvoiceItem>();
            var savedItems = TableStateStorage.GetTableState(tableID);
            if (savedItems != null)
            {
                foreach (var item in savedItems)
                {
                    InvoiceItems.Add(item);
                }
            }
            InvoiceItems.CollectionChanged += InvoiceItems_CollectionChanged;
            dgvSanPham.ItemsSource = InvoiceItems;
            DataContext = this;
            AddCategory();
            LoadProduct();
            dgvSanPham.AutoGeneratingColumn += DgvSanPham_AutoGeneratingColumn;
        }
        private void StartClock()
        {
            clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromSeconds(1);
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            clockTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        public class InvoiceItem : INotifyPropertyChanged
        {
            private int qty;
            public string MaSP { get; set; }
            public string TenSP { get; set; }
            public string Size { get; set; }
            public int Qty
            {
                get => qty;
                set
                {
                    if (qty != value)
                    {
                        qty = value;
                        OnPropertyChanged(nameof(Qty));
                        OnPropertyChanged(nameof(Amount));
                    }
                }
            }
            public string Gia { get; set; } // Use string for Gia
            public double GiaAsDouble { get { return double.TryParse(Gia, out var result) ? result : 0; } } // Convert Gia to double
            public string Amount { get { return (Qty * GiaAsDouble).ToString("N0"); } } // Format Amount with thousands separator

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private void InvoiceItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateTotalAmount();
        }
        private void AddCategory()
        {
            try
            {
                string connectionString = "Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("Select * from LoaiSanPham", connection);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    connection.Close();

                    Loaipanel.Children.Clear();

                    // Add "Tất cả" button
                    System.Windows.Controls.Button allButton = new System.Windows.Controls.Button
                    {
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDC7B7")),
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#123C69")),
                        Width = 90,
                        Height = 30,
                        Margin = new Thickness(7),
                        BorderThickness = new Thickness(0),
                        Effect = null,
                        FontSize = 10,
                        Content = "Tất cả"

                    };
                    allButton.Click += BtnThem_Click;
                    Loaipanel.Children.Add(allButton);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            System.Windows.Controls.Button b = new System.Windows.Controls.Button
                            {
                                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AC3B61")),
                                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDC7B7")),
                                Width = 90,
                                Height = 30,
                                Margin = new Thickness(7),
                                BorderThickness = new Thickness(0),
                                BorderBrush = null, // Set the BorderBrush to null to remove the border
                                FontSize = 10,

                                Content = row["loaiSanPham"].ToString()
                            };

                            b.Click += BtnThem_Click;

                            Loaipanel.Children.Add(b);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Controls.Button clickedButton = (System.Windows.Controls.Button)sender;
                string selectedCategory = clickedButton.Content.ToString().ToLower();

                // Reset all button colors to white
                foreach (var button in Loaipanel.Children.OfType<System.Windows.Controls.Button>())
                {
                    button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AC3B61"));
                    button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDC7B7"));
                }

                // Set the background of the selected button to a different color
                clickedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDC7B7"));
                clickedButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#123C69"));
                foreach (var item in Productpanel.Children)
                {
                    if (item is AddProductUserControl productControl)
                    {
                        string productCategory = productControl.loaiSanPham.ToLower();

                        // If the "Tất cả" button is clicked, show all products
                        bool isVisible = selectedCategory == "tất cả" || productCategory.Contains(selectedCategory);

                        productControl.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in btnThem_Click: " + ex.Message);
            }
        }

        private void LoadProduct()
        {
            try
            {
                string connectionString = "Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM SanPham INNER JOIN LoaiSanPham ON maLoai = maLoaiSanPham", connection);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    connection.Close();

                    Productpanel.Children.Clear();

                    foreach (DataRow productRow in dt.Rows)
                    {
                        string id = productRow["maSanPham"].ToString();
                        string name = productRow["tenSanPham"].ToString();
                        string cat = productRow["loaiSanPham"].ToString();
                        double price = Convert.ToDouble(productRow["gia"].ToString()); // Convert price to double
                        string size = productRow["thuocTinh"].ToString(); // Retrieve size information
                        string imagePath = "C://Đồ án CNPM//AppQLQuanCaPhe//AppQLQuanCaPhe//Hinhanh//" + productRow["anh"].ToString();

                        var addProductUserControl = new AddProductUserControl
                        {
                            maSanPham = id,
                            tenSanPham = name,
                            loaiSanPham = cat,
                            Gia = price.ToString("N0"), // Format price with thousands separator
                            Size = size, // Assign size to the user control
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                            VerticalAlignment = System.Windows.VerticalAlignment.Top,
                            Margin = new Thickness(10)
                        };

                        string fullImagePath = System.IO.Path.Combine(Environment.CurrentDirectory, imagePath);
                        if (System.IO.File.Exists(fullImagePath))
                        {
                            addProductUserControl.ImageValue = new System.Windows.Media.Imaging.BitmapImage(new Uri(fullImagePath));
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Image not found at: " + fullImagePath);
                            addProductUserControl.ImageValue = new System.Windows.Media.Imaging.BitmapImage(new Uri("default_image_path_here"));
                        }

                        Productpanel.Children.Add(addProductUserControl);
                        addProductUserControl.MouseLeftButtonDown += (sender, e) =>
                        {
                            var selectedProduct = sender as AddProductUserControl;
                            if (selectedProduct != null)
                            {
                                try
                                {
                                    var existingItem = InvoiceItems.FirstOrDefault(invoiceItem => invoiceItem.MaSP == selectedProduct.maSanPham);
                                    if (existingItem != null)
                                    {
                                        existingItem.Qty++;
                                        // Đảm bảo DataGrid cập nhật giao diện
                                        dgvSanPham.Items.Refresh();

                                    }
                                    else
                                    {
                                        var newItem = new InvoiceItem
                                        {
                                            MaSP = id,
                                            TenSP = name,
                                            Size = size,
                                            Qty = 1,
                                            Gia = price.ToString("N0") // Format price with thousands separator
                                        };
                                        InvoiceItems.Add(newItem);
                                    }
                                    UpdateTotalAmount();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error in AddItems: " + ex.Message);
                                }
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in LoadProduct: " + ex.Message);
            }
        }

        private void DgvSanPham_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            bool isEvenRow = e.Row.GetIndex() % 2 == 0;

            // Chọn màu sắc tùy thuộc vào dòng là chẵn hay lẻ
            if (isEvenRow)
            {
                e.Row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDC7B7")); // Màu cho dòng chẵn
            }
            else
            {
                e.Row.Background = Brushes.White; // Màu cho dòng lẻ
            }
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            e.Row.HeaderStyle = new Style(typeof(DataGridRowHeader));
            e.Row.HeaderStyle.Setters.Add(new Setter(VerticalContentAlignmentProperty, VerticalAlignment.Center));
            e.Row.HeaderStyle = new Style(typeof(DataGridRowHeader));
            e.Row.HeaderStyle.Setters.Add(new Setter(FontSizeProperty, 14.0));
        }

        private void DgvSanPham_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "MaSP")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
            else if (e.PropertyName == "SRNumber")
            {
                e.Cancel = true; // Tạm ngưng việc tạo cột này
            }
            else if (e.PropertyName == "Gia")
            {
                e.Column.CellStyle = (Style)this.FindResource("PriceColumnStyle"); // Áp dụng kiểu tùy chỉnh cho cột giá
            }
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Grid;
            if (button != null)
            {
                var item = button.DataContext as InvoiceItem;
                if (item != null)
                {
                    item.Qty++;
                    dgvSanPham.Items.Refresh();
                    // Cập nhật lại tổng tiền
                    UpdateTotalAmount();
                }
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Grid;
            if (button != null)
            {
                var item = button.DataContext as InvoiceItem;
                if (item != null && item.Qty > 1)
                {
                    item.Qty--;
                    dgvSanPham.Items.Refresh();
                    // Cập nhật lại tổng tiền
                    UpdateTotalAmount();
                }
            }
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            // Lấy thông tin về món hàng cần xóa
            var item = (sender as System.Windows.Controls.Button).DataContext as InvoiceItem;

            // Xóa món hàng khỏi danh sách
            InvoiceItems.Remove(item);
            dgvSanPham.Items.Refresh();
            UpdateTotalAmount();
        }

        private void UpdateTotalAmount()
        {
            // Get the list of products from the DataGrid
            var products = InvoiceItems.ToList();

            if (products == null || products.Count == 0)
            {
                // No products to calculate total amount
                totalQuantityTextBlock.Text = "0";
                totalAmountTextBlock.Text = $"{0:N0}"; // Format total amount as integer without decimals
                discountedAmountTextBlock.Text = $"{0:N0}"; // Format discounted amount as integer without decimals
                return;
            }

            // Calculate total quantity and amount
            int totalQuantity = products.Sum(item => item.Qty);
            decimal totalAmount = products.Sum(item => Convert.ToDecimal(item.GiaAsDouble) * item.Qty);

            // Update UI elements
            totalQuantityTextBlock.Text = $"{totalQuantity}";
            totalAmountTextBlock.Text = $"{totalAmount:N0}"; // Update total amount formatted as integer without decimals

            // Handle discount
            decimal discountedAmount = totalAmount;
            if (!string.IsNullOrEmpty(discountTextBox.Text))
            {
                if (decimal.TryParse(discountTextBox.Text, out decimal discountPercent))
                {
                    if (discountPercent >= 0 && discountPercent <= 100)
                    {
                        decimal discountAmount = totalAmount * (discountPercent / 100);
                        discountedAmount = totalAmount - discountAmount;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Vui lòng nhập giảm giá hợp lệ (từ 0 đến 100).");
                        return;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Vui lòng nhập giảm giá hợp lệ (số nguyên hoặc số thập phân).");
                    return;
                }
            }

            // Update discounted amount text
            discountedAmountTextBlock.Text = $"{discountedAmount:N0}"; // Update discounted amount formatted as integer without decimals
        }

        private void DiscountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Call UpdateTotalAmount to handle changes in discount
            UpdateTotalAmount();
        }
        private void DgvSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTotalAmount();
        }

        private void QuantityTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            if (textBox != null)
            {
                var item = textBox.DataContext as InvoiceItem;
                if (item != null)
                {
                    // If the TextBox is empty, set it to "1"
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        textBox.Text = "1";
                    }
                    else
                    {
                        // Validate input
                        if (int.TryParse(textBox.Text, out int newQty) && newQty > 0)
                        {
                            item.Qty = newQty;
                        }
                        else
                        {
                            // Invalid quantity entered, set to 1
                            textBox.Text = "1";
                        }
                    }

                    UpdateTotalAmount();
                }
            }
        }
        private string GetTableIdFromTableName(string tableName)
        {
            string tableId = "";
            try
            {
                string connectionString = "Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT maBan FROM Ban WHERE tenBan = @tenBan", connection);
                    command.Parameters.AddWithValue("@tenBan", tableName);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        tableId = reader["maBan"].ToString();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return tableId;
        }
        private void SaveInvoiceToDatabase(List<InvoiceItem> products, string maGiaoDich, string tableId, decimal discountPercent)
        {
            using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
            {
                cn.Open();

                // Kiểm tra sự tồn tại của maBan trong bảng Ban
                string checkTableIdQuery = "SELECT COUNT(*) FROM Ban WHERE maBan = @maBan";
                using (SqlCommand checkCmd = new SqlCommand(checkTableIdQuery, cn))
                {
                    checkCmd.Parameters.AddWithValue("@maBan", tableId);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count == 0)
                    {
                        System.Windows.MessageBox.Show("Bàn không tồn tại.");
                        return;
                    }
                }

                // Lưu hóa đơn vào bảng HoaDon
                string insertHoaDonQuery = "INSERT INTO HoaDon (maGiaoDich, maBan, maTaiKhoan, ngayBan, trangThai, giamGia) VALUES (@maGiaoDich, @maBan, @maTaiKhoan, @ngayBan, @trangThai, @giamGia)";
                using (SqlCommand cmd = new SqlCommand(insertHoaDonQuery, cn))
                {
                    cmd.Parameters.AddWithValue("@maGiaoDich", maGiaoDich);
                    cmd.Parameters.AddWithValue("@maBan", tableId);
                    cmd.Parameters.AddWithValue("@maTaiKhoan", "TK000006");
                    cmd.Parameters.AddWithValue("@ngayBan", DateTime.Now); // Lưu cả ngày và giờ hiện tại
                    cmd.Parameters.AddWithValue("@trangThai", 1); // giả sử trạng thái là 1
                    cmd.Parameters.AddWithValue("@giamGia", discountPercent);
                    cmd.ExecuteNonQuery();
                }

                // Lưu các sản phẩm vào bảng CTHoaDon
                string insertCTHoaDonQuery = "INSERT INTO CTHoaDon (maHoaDon, maSanPham, tongTien, soLuong) VALUES (@maHoaDon, @maSanPham, @tongTien, @soLuong)";
                foreach (var product in products)
                {
                    using (SqlCommand cmd = new SqlCommand(insertCTHoaDonQuery, cn))
                    {
                        cmd.Parameters.AddWithValue("@maHoaDon", maGiaoDich);
                        cmd.Parameters.AddWithValue("@maSanPham", product.MaSP);
                        cmd.Parameters.AddWithValue("@tongTien", Convert.ToDecimal(product.GiaAsDouble) * product.Qty);
                        cmd.Parameters.AddWithValue("@soLuong", product.Qty); // Lưu số lượng sản phẩm

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void ThanhToan_Click(object sender, RoutedEventArgs e)
        {
            // Lấy danh sách các sản phẩm trong DataGrid
            var products = InvoiceItems.ToList();

            if (products == null || products.Count == 0)
            {
                System.Windows.MessageBox.Show("Không có sản phẩm để thanh toán.");
                return;
            }

            // Tính tổng tiền
            decimal totalAmount = products.Sum(p => Convert.ToDecimal(p.GiaAsDouble) * p.Qty);
            decimal discountPercent = 0;

            if (!string.IsNullOrEmpty(discountTextBox.Text))
            {
                if (decimal.TryParse(discountTextBox.Text, out discountPercent))
                {
                    if (discountPercent > 0 && discountPercent <= 100)
                    {
                        decimal discountAmount = totalAmount * (discountPercent / 100);
                        totalAmount -= discountAmount;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Vui lòng nhập giảm giá hợp lệ (từ 1 đến 100).");
                        return;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Vui lòng nhập giảm giá hợp lệ (số nguyên hoặc số thập phân).");
                    return;
                }
            }

            // Lấy tên bàn từ tableTextBlock
            string tableName = tableTextBlock.Text;

            // Lấy mã bàn từ tên bàn
            string tableId = GetTableIdFromTableName(tableName);

            // Kiểm tra nếu không tìm thấy mã bàn
            if (string.IsNullOrEmpty(tableId))
            {
                System.Windows.MessageBox.Show("Bàn không tồn tại.");
                return;
            }

            // Tạo mã giao dịch tự động
            string maGiaoDich = GenerateInvoiceID();

            // Lưu hóa đơn vào cơ sở dữ liệu
            SaveInvoiceToDatabase(products, maGiaoDich, tableId, discountPercent);

            // Hiển thị thông báo thanh toán
            System.Windows.MessageBox.Show($"Tổng tiền: {totalAmount.ToString("N0")}", "Thanh Toán", MessageBoxButton.OK, MessageBoxImage.Information);

            // Xóa các sản phẩm khỏi bảng
            InvoiceItems.Clear();

            using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
            {
                cn.Open(); // Open the connection

                SqlCommand command = new SqlCommand("UPDATE Ban SET trangThai = @trangThai WHERE maBan = @maBan", cn);
                command.Parameters.AddWithValue("@trangThai", "Trống");
                command.Parameters.AddWithValue("@maBan", tableId);

                int rowsAffected = command.ExecuteNonQuery();

                Console.WriteLine($"Rows affected: {rowsAffected}");
            }
            OpenPOS();
        }
        private void OpenPOS()
        {

            // Mở MenuUserControl tương ứng với bàn đã chọn
            POSTablesUserControl pos = new POSTablesUserControl();
            // Hiển thị menuUserControl
            this.Content = pos;
        }

        // Giả sử bạn có một phương thức để quay lại trang bàn

        private string GenerateInvoiceID()
        {
            string prefix = "HD"; // Prefix for invoice ID
            int count;

            using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("SELECT COUNT(maGiaoDich) FROM [HoaDon]", cn);
                count = (int)cm.ExecuteScalar();
                count++;
            }

            string id = prefix + count.ToString().PadLeft(6, '0');
            return id;
        }
        public static class TableStateStorage
        {
            private static Dictionary<string, List<InvoiceItem>> tableStates = new Dictionary<string, List<InvoiceItem>>();

            public static void SaveTableState(string tableId, List<InvoiceItem> items)
            {
                if (tableStates.ContainsKey(tableId))
                {
                    tableStates[tableId] = items;
                }
                else
                {
                    tableStates.Add(tableId, items);
                }

                // Cập nhật trạng thái bàn
                UpdateTableStatus(tableId);
            }

            public static List<InvoiceItem> GetTableState(string tableId)
            {
                if (tableStates.ContainsKey(tableId))
                {
                    return tableStates[tableId];
                }
                return null;
            }

            public static void UpdateTableStatus(string tableId)
            {
                try
                {
                    string status = "Trống"; // Default to "Trống" if no items in InvoiceItems

                    // Check if there are items in InvoiceItems for the table
                    if (tableStates.ContainsKey(tableId) && tableStates[tableId].Count > 0)
                    {
                        status = "HD"; // Set status to "HD" if there are items
                    }

                    using (SqlConnection cn = new SqlConnection("Data Source=ALBERT\\SQLEXPRESS;Initial Catalog=QLQuanCaPhe;Integrated Security=True;"))
                    {
                        cn.Open(); // Open the connection

                        SqlCommand command = new SqlCommand("UPDATE Ban SET trangThai = @trangThai WHERE tenBan = @tenBan", cn);
                        command.Parameters.AddWithValue("@trangThai", status);
                        command.Parameters.AddWithValue("@tenBan", tableId);

                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine($"Rows affected: {rowsAffected}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

        }

        private void BackToPOSTable_Click(object sender, RoutedEventArgs e)
        {
            string tableId = tableTextBlock.Text;

            if (InvoiceItems != null && InvoiceItems.Count > 0)
            {
                TableStateStorage.SaveTableState(tableId, InvoiceItems.ToList());
            }
            else
            {
                TableStateStorage.SaveTableState(tableId, new List<InvoiceItem>());
            }

            OpenPOS();
        }
    }
}