using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie1Militaria.Models
{
    public class BillingEntry
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string EntryType { get; set; }
        public decimal Amount { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
