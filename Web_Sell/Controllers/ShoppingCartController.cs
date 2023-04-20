using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Web_Sell.Models;
using System.Diagnostics;

namespace Web_Sell.Controllers
{
    public class ShoppingCartController : Controller
    {
        PhoneManagerEntities _db = new PhoneManagerEntities();

        // GET: ShoppingCart
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public ActionResult AddtoCart(string id)
        {
            var product = _db.Products.SingleOrDefault(s => s.ProductID == id);
            if (product != null)
            {
                GetCart().Add(product);
            }
            //return RedirectToAction("ShowToCart", "ShoppingCart");
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ShowToCart()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("ShowToCart", "ShoppingCart");

            }
            Cart cart = Session["Cart"] as Cart;
            return View(cart);
        }
        public ActionResult Update_Quantity_Product(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            string id_product = form["Product_ID"];
            int quantity_prodcut = int.Parse(form["Product_Quantity"]);
            cart.update_quantity(id_product, quantity_prodcut);
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }
        public ActionResult RemoveProduct(string id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.remove_product(id);
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int _t_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null) 
            { 
                _t_item = cart.total_quantity();
            }
            ViewBag.infoCart = _t_item;
            return PartialView("BagCart");
        }


    }
}