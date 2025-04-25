using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
//using static AppQLQuanCaPhe.ProductUserControl;

namespace AppQLQuanCaPhe
{
    /// <summary>
    /// Interaction logic for POSTablesUserControl.xaml
    /// </summary>
    public partial class POSTablesUserControl : UserControl
    {

        public ObservableCollection<int> NumberRange { get; set; }

        public int SelectedNumber { get; set; }
        public POSTablesUserControl()
        {
            InitializeComponent();
            NumberRange = new ObservableCollection<int>(Enumerable.Range(-100, 201));
            SelectedNumber = NumberRange.FirstOrDefault(); // Lấy chỉ mục đầu tiên
            DataContext = this;
            LoadTable();
        }
        #region Method
        void LoadTable()
        {
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button()
                {
                    Width = TableDAO.TableWidth,
                    Height = TableDAO.TableHeight,
                    Margin = new Thickness(15),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center
                };

                // Sử dụng StackPanel để chứa biểu tượng và nội dung văn bản
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                // Thêm biểu tượng bàn
                var icon = new PackIcon
                {
                    Kind = PackIconKind.TableFurniture,
                    Width = 30,
                    Height = 30,
                    Margin = new Thickness(0, 0, 0, 5), // Khoảng cách giữa biểu tượng và nội dung văn bản
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                // Thêm TextBlock cho tên bàn
                TextBlock textBlock = new TextBlock
                {
                    Text = item.Name,
                    TextAlignment = TextAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                stackPanel.Children.Add(icon);
                stackPanel.Children.Add(textBlock);

                btn.Content = stackPanel;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống":
                     
                        btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDC7B7"));
                        btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#123C69"));
                        break;

                    default:
                        btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AC3B61"));
                        btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDC7B7"));
                        break;
                }

                btn.Click += Btn_Click;
                flpTable.Children.Add(btn);
            }
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = (Button)sender;
            Table tableClicked = (Table)clickedBtn.Tag;

            // Hiển thị tên của bàn trong TextBlock
            MenuUserControl menuUserControl = new MenuUserControl(tableClicked.Name);
            menuUserControl.tableTextBlock.Text = tableClicked.Name;
            
            OpenMenuPage(tableClicked);
        }

        private void OpenMenuPage(Table table)
        {
            // Mở MenuUserControl tương ứng với bàn đã chọn
            MenuUserControl menuUserControl = new MenuUserControl(table.Name);
            // Hiển thị menuUserControl
            this.Content = menuUserControl;
        }
        #endregion
                    
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
