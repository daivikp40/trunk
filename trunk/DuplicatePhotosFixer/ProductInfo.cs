using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer
{
    public class Price
    {
        public double gross { get; set; }
        public double net { get; set; }
        public double tax { get; set; }
    }

    public class ListPrice
    {
        public double gross { get; set; }
        public double net { get; set; }
        public double tax { get; set; }
    }

    public class Product
    {
        public int product_id { get; set; }
        public string product_title { get; set; }
        public string currency { get; set; }
        public bool vendor_set_prices_included_tax { get; set; }
        public Price price { get; set; }
        public ListPrice list_price { get; set; }
        public List<string> applied_coupon { get; set; }
    }

    public class Response
    {
        public string customer_country { get; set; }
        public List<Product> products { get; set; }
    }

    public class ProductInfo
    {
        public bool success { get; set; }
        public Response response { get; set; }
    }

    public class ProductRequest
    {
        public string ProductName { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }
}
