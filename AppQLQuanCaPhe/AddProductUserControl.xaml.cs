using System;
using System.Collections.Generic;
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

namespace AppQLQuanCaPhe
{
    /// <summary>
    /// Interaction logic for AddProductUserControl.xaml
    /// </summary>
    public partial class AddProductUserControl : UserControl
    {
        public event EventHandler onSelect = null;

        public AddProductUserControl()
        {
            InitializeComponent();
        }

        public string maSanPham { get; set; }
       
        public string loaiSanPham { get; set; }
        public string Gia
        {
            get { return txtGia.Text; }
            set { txtGia.Text = decimal.Parse(value).ToString("N0"); }
        }
        public string Size
        {
            get { return txtSize.Text; }
            set { txtSize.Text = value; }
        }

        public string tenSanPham
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }

        public ImageSource ImageValue
        {
            get { return txtimg.Source; }
            set { txtimg.Source = value; }
        }

        private void txtimg_Click(object sender, RoutedEventArgs e)
        {
            onSelect?.Invoke(this, e);
        }
    }
}