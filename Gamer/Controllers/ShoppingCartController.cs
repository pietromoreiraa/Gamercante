using Gamer.Models;
using Gamer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;

namespace Gamer.Controllers
{
    public class ShoppingCartController : Controller
    {
        Context storeDB = new Context();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedGame = storeDB.Games
                .Single(game => game.GameID == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedGame);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string gameName = storeDB.Carts
                .Single(item => item.RecordId == id).Game.Nome;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(gameName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }

        //Work with Paypal Payment
        private Payment payment;

        //Create a payment using an APIContext
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var cartv = ShoppingCart.GetCart(this.HttpContext);
            var listItems = new ItemList(){items = new List<Item>()};
            List<Cart> listCarts = cartv.GetCartItems();
            
            foreach (var cart in listCarts)
            {
                Context bd = new Context();
                var game = bd.Games.Find(cart.GameId);
                cart.Game = game;
                double preco = Convert.ToDouble(cart.Game.Preco);
                listItems.items.Add(new Item()
                {
                     
                    name = cart.Game.Nome,
                    currency = "BRL",
                    price = preco.ToString(),
                    quantity = cart.Count.ToString(),
                    sku = "sku"

                });

                                    
            }
            var payer = new Payer() { payment_method = "paypal"};

            //Do the configuration RedirectURLs here with redirectURLs object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };
           
            double stotal = Convert.ToDouble(cartv.GetTotal());

            //Create details object
            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = stotal.ToString()
            };

            //Create amount object
            var amount = new Amount()
            {
                currency = "BRL",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(), //tax + shipping + subtotal
                details = details
            };

            //Create transaction
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Testing transaction",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = listItems
            });

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return payment.Create(apiContext);
;
        }

        //Create ExecutePayment Method
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
           this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        // Create PaymentWithPaypal method
        public ActionResult PaymentWithPaypal()
        {
            // Getting context from the paypal bases on clientID and clientSecret for payment
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //Creating a payment
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ShoppingCart/PaymentwithPaypal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //Get links returned from paypal response to create
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;

                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = link.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    //This one will be executed when  we have  received all the payment params from previous call
                    string guid = Request.Params["guid"];
                    string paymentID = Request.Params["paymentId"];
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentID);
                    if(executedPayment.state.ToLower() != "approved")
                    {
                        return View("Failure");
                    }
                }
            }
            catch(Exception ex)
            {
                PaypalLogger.log("Error: " + ex.Message);
                return View("Failure");
            }
            return View("Success");
        }
    }
}