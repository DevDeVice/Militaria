using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Militaria2.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string EAN { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal NetPrice { get; set; }
        public decimal VAT { get; set; }
        public int StockQuantity { get; set; }
        public string[] Images { get; set; }
        public List<ProductVariant> Variants { get; set; }
        public bool FlaggedForOffer { get; set; }
    }
    public class ProductVariant
    {
        public string VariantId { get; set; }
        public string CodeProducer { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
    }
}
