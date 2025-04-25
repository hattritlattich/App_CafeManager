using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQLQuanCaPhe
{
    public class TableInvoice
    {
        public string TableID { get; set; }
        public ObservableCollection<MenuUserControl.InvoiceItem> InvoiceItems { get; set; }

        public TableInvoice(string tableID)
        {
            TableID = tableID;
            InvoiceItems = new ObservableCollection<MenuUserControl.InvoiceItem>();
        }
    }

}
