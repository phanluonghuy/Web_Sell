using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Web_Sell.Models;
using System.Net.Mail;

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
                return RedirectToAction("Index", "Home");

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
        public ActionResult Checkout_Success()
        {
            return View();
        }
        public ActionResult Checkout_Fail() {
            return View();
        }

        public ActionResult CheckOut(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            Orders _orders = new Orders();
            if (Session["AgentID"] != null)
            {
                string payment = form["Payment"];
                if (payment == "VNPay")
                    return RedirectToAction("Payment", "Home", new { amount = form["Total_money"].ToString() });
                else return RedirectToAction("CheckOut_Import", "ShoppingCart", new { amount = form["Total_money"].ToString() });
            }
            else
            {
                return RedirectToAction("Checkout_Fail", "ShoppingCart");
            }

        }

        public ActionResult CheckOut_Import(string amount)
        {
            Cart cart = Session["Cart"] as Cart;
            Orders _orders = new Orders();
            if (Session["AgentID"] != null)
            {
                try
                {

                    _orders.OrderID = DateTime.Now.ToString("yyyyMMddHHmmss");
                    _orders.AgentID = (string)Session["AgentID"];
                    _orders.OrderDate = DateTime.Now;

                    _orders.TotalAmount = decimal.Parse(amount);
                    //_orders.TotalAmount = decimal.Parse(form["Total_money"]);
                    _orders.PaymentMethod = "VNPay";
                    _orders.Status = "Pending";
                    _db.Orders.Add(_orders);
                    foreach (var item in cart.Items)
                    {
                        OrderDetails _orderDetails = new OrderDetails();
                        _orderDetails.OrderID = _orders.OrderID;
                        _orderDetails.ProductID = item._shopping_products.ProductID;
                        _orderDetails.Quantity = item._shopping_quanity;
                        _db.OrderDetails.Add(_orderDetails);
                    }
                    _db.SaveChanges();
                    cart.clearCart();


                //    try
                //    {
                //        MailMessage mail = new MailMessage();
                //        mail.From = new MailAddress("sender@example.com");
                //        mail.To.Add("phanhuy0406@example.com");
                //        mail.Subject = "Test Email";
                //        mail.Body = "This is a test email sent from ASP.NET MVC.";

                    //        SmtpClient smtp = new SmtpClient();
                    //        smtp.Host = "smtp.example.com";
                    //        smtp.Port = 587;
                    //        smtp.Credentials = new System.Net.NetworkCredential("sender@example.com", "password");
                    //        smtp.EnableSsl = true;

                    //        smtp.Send(mail);
                    //        ViewBag.Message = "Email sent successfully.";
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        ViewBag.Error = "Error occurred while sending email. Error message: " + ex.Message;
                    //    }



                        return RedirectToAction("Checkout_Success", "ShoppingCart");
                    //    //return RedirectToAction("ShowToCart", "ShoppingCart");
                    }
                catch
                {
                    return Content("Error");
                }
            }
            else
            {
                return RedirectToAction("Checkout_Fail", "ShoppingCart");
            }

        }


    }
}