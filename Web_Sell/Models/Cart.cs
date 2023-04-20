using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Sell.Models
{
    public class CartItem
    {
        public Products _shopping_products { get; set; }
        public int _shopping_quanity { get; set; }
    }
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items {
            get { return items; }
        }
        public void Add(Products _product,int _quantity = 1) 
        {
            var item = items.FirstOrDefault(s => s._shopping_products.ProductID == _product.ProductID);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    _shopping_products = _product,
                    _shopping_quanity = _quantity
                });
            }
            else
            {
                //fix here
                item._shopping_quanity += _quantity;
            }
        }
        public void update_quantity(string id,int _quantity)
        {
            var item = items.Find(s=>s._shopping_products.ProductID==id);
            if (item != null)
            {
                item._shopping_quanity = _quantity;
            }

        }

        public double total_money()
        {
            var total = items.Sum(s => (double)(s._shopping_products.UnitPrice) * s._shopping_quanity);
            return total;
        }
        public void remove_product(string id)
        {
            items.RemoveAll(s => s._shopping_products.ProductID == id);
        }

        public int total_quantity()
        {
            return items.Count();
        }
    }
}