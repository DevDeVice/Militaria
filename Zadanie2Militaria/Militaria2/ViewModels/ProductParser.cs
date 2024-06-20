using Militaria2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

public static class ProductParser
{
    public static List<Product> ParseAllSuppliers()
    {
        var allProducts = new List<Product>();

        // Parsowanie plików XML dostawców
        allProducts.AddRange(ParseSupplier1("data\\dostawca1plik1.xml"));
        allProducts.AddRange(ParseSupplier1("data\\dostawca1plik2.xml"));
        allProducts.AddRange(ParseSupplier2("data\\dostawca2plik1.xml"));
        allProducts.AddRange(ParseSupplier2("data\\dostawca2plik2.xml"));
        allProducts.AddRange(ParseSupplier3("data\\dostawca3plik1.xml"));

        return allProducts;
    }

    public static List<Product> ParseSupplier1(string filePath)
    {
        var products = new List<Product>();
        var doc = XDocument.Load(filePath);
        var cultureInfo = new CultureInfo("en-US");

        foreach (var element in doc.Descendants("product"))
        {
            var product = new Product
            {
                Id = element.Attribute("id")?.Value,
                EAN = element.Element("ean")?.Value,
                SKU = element.Element("sku")?.Value,
                Name = element.Element("description")?.Element("name")?.Value,
                Description = element.Element("description")?.Element("long_desc")?.Value,
                GrossPrice = decimal.Parse(element.Element("price")?.Attribute("gross")?.Value ?? "0", cultureInfo),
                NetPrice = decimal.Parse(element.Element("price")?.Attribute("net")?.Value ?? "0", cultureInfo),
                VAT = decimal.Parse(element.Element("price")?.Attribute("vat")?.Value ?? "0", cultureInfo),
                StockQuantity = int.Parse(element.Descendants("stock").FirstOrDefault()?.Attribute("quantity")?.Value ?? "0"),
                Images = element.Descendants("image").Select(img => img.Attribute("url")?.Value).Where(url => url != null).ToArray(),
                Variants = element.Descendants("size").Select(size => new ProductVariant
                {
                    VariantId = size.Attribute("id")?.Value,
                    CodeProducer = size.Attribute("code_producer")?.Value,
                    Code = size.Attribute("code")?.Value,
                    Quantity = int.Parse(size.Descendants("stock").FirstOrDefault()?.Attribute("quantity")?.Value ?? "0")
                }).ToList()
            };

            products.Add(product);
        }

        return products;
    }

    public static List<Product> ParseSupplier2(string filePath)
    {
        var products = new List<Product>();
        var doc = XDocument.Load(filePath);
        var cultureInfo = new CultureInfo("en-US");

        foreach (var element in doc.Descendants("product"))
        {
            var product = new Product
            {
                Id = element.Element("id")?.Value,
                EAN = element.Element("ean")?.Value,
                SKU = element.Element("sku")?.Value,
                Name = element.Element("name")?.Value,
                Description = element.Element("desc")?.Value,
                GrossPrice = decimal.Parse(element.Element("priceAfterDiscountNet")?.Value ?? "0", cultureInfo),
                NetPrice = decimal.Parse(element.Element("priceAfterDiscountNet")?.Value ?? "0", cultureInfo) * 0.77M, // Assuming VAT of 23%
                VAT = 23.0M,
                StockQuantity = int.Parse(element.Element("qty")?.Value ?? "0"),
                Images = element.Descendants("photo").Select(photo => photo.Value).Where(url => url != null).ToArray(),
                Variants = new List<ProductVariant>()
            };

            products.Add(product);
        }

        return products;
    }

    public static List<Product> ParseSupplier3(string filePath)
    {
        var products = new List<Product>();
        var doc = XDocument.Load(filePath);
        var cultureInfo = new CultureInfo("en-US");

        foreach (var element in doc.Descendants("produkt"))
        {
            var product = new Product
            {
                Id = element.Element("id")?.Value,
                EAN = element.Element("ean")?.Value,
                SKU = element.Element("kod")?.Value,
                Name = element.Element("nazwa")?.Value,
                Description = element.Element("dlugi_opis")?.Value,
                GrossPrice = decimal.Parse(element.Element("cena_zewnetrzna_hurt")?.Value ?? "0", cultureInfo),
                NetPrice = decimal.Parse(element.Element("cena_zewnetrzna_hurt")?.Value ?? "0", cultureInfo) / (1 + decimal.Parse(element.Element("vat")?.Value ?? "0", cultureInfo)),
                VAT = decimal.Parse(element.Element("vat")?.Value ?? "0", cultureInfo),
                StockQuantity = 0, // Supplier 3 does not provide stock quantity in the given XML
                Images = element.Descendants("zdjecie").Select(img => img.Attribute("url")?.Value).Where(url => url != null).ToArray(),
                Variants = new List<ProductVariant>()
            };

            products.Add(product);
        }

        return products;
    }
}
