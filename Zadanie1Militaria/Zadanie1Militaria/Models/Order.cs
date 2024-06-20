using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie1Militaria.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public int? ErpOrderId { get; set; }
        public int? InvoiceId { get; set; }
        public int StoreId { get; set; }
    }
}
