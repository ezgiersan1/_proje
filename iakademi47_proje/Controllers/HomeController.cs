using Microsoft.AspNetCore.Mvc;
using iakademi47_proje.Models;
using PagedList.Core.Mvc; //paket indirildi
using PagedList.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Specialized;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace iakademi47_proje.Controllers
{
    public class HomeController : Controller
    {

        Cls_Product p = new Cls_Product(); //new diyerek nesne oluşturdum
        MainPageModel mpm = new MainPageModel();
        iakademi47Context context = new iakademi47Context();
        Cls_Order cls_order = new Cls_Order();


        public IActionResult Index()
        {
            mpm.SliderProducts = p.ProductSelect("slider", "", 0);
            //new = ana sayfa, ""=alt sayfa için parametre  0 = ajax için parametre
            mpm.NewProducts = p.ProductSelect("new", "", 0);
            mpm.Productofday = p.ProductDetails("Productofday");
            mpm.SpecialProducts = p.ProductSelect("Special", "", 0);
            mpm.DiscountedProducts = p.ProductSelect("Discounted", "", 0);
            mpm.HighlightedProducts = p.ProductSelect("Highlighted", "", 0);
            mpm.TopSellerProducts = p.ProductSelect("TopSeller", "", 0);
            mpm.StarProducts = p.ProductSelect("Star", "", 0);
            mpm.FeaturedProducts = p.ProductSelect("Featured", "", 0);
            mpm.NotableProducts = p.ProductSelect("Notable", "", 0);

            return View(mpm);
        }


        public IActionResult NewProducts()
        {
            mpm.NewProducts = p.ProductSelect("new", "new", 0);
            return View(mpm);
        }

        public PartialViewResult _partialNewProducts(string nextpagenumber)
        {
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.NewProducts = p.ProductSelect("new", "new", pagenumber);
            return PartialView(mpm);
        }




        public IActionResult SpecialProducts()
        {
            mpm.SpecialProducts = p.ProductSelect("Special", "Special", 0);
            return View(mpm);
        }

        public PartialViewResult _partialSpecialProducts(string nextpagenumber)
        {
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.SpecialProducts = p.ProductSelect("Special", "Special", pagenumber);
            return PartialView();
        }


        public IActionResult DiscountedProducts()
        {
            mpm.DiscountedProducts = p.ProductSelect("Discounted", "Discounted", 0);
            return View(mpm);
        }

        public PartialViewResult _partialDiscountedProducts(string nextpagenumber)
        {
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.DiscountedProducts = p.ProductSelect("Discounted", "Discounted", pagenumber);
            return PartialView();
        }

        public IActionResult HighlightedProducts()
        {
            mpm.HighlightedProducts = p.ProductSelect("Highlighted", "Highlighted", 0);
            return View(mpm);
        }

        public PartialViewResult _partialHighlightedProducts(string nextpagenumber)
        {
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.HighlightedProducts = p.ProductSelect("Highlighted", "Highlighted", pagenumber);
            return PartialView();
        }

        public IActionResult TopSellerProducts(int page = 1, int pageSize = 4)
        {
            PagedList<Product> model = new PagedList<Product>(context.Products.OrderByDescending(p => p.TopSeller), page, pageSize);
            return View("TopSellerProducts", model);
        }


        public string CartProcess(int id)
        {

            //10=1& -- 10 numaralı üründen 1 tane 20=1& 20 numaralı üründen 1 tane
            //ürün detayına tıklanınca, sepete eklenince Highlighted kolonunun değerini 1 arttıracağız
            Cls_Product.Highlighted_Increase(id);

            cls_order.ProductID = id;
            cls_order.Quantity = 1;

            var cookieOptions = new CookieOptions();
            //tarayıcıdan okuma
            var cookie = Request.Cookies["sepetim"];
            if (cookie == null)
            {
                //sepet boş
                cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(7); //7 günlük çerez süresi
                cookieOptions.Path = "/";
                cls_order.MyCart = "";
                cls_order.AddToMyCart(id.ToString());
                Response.Cookies.Append("sepetim", cls_order.MyCart, cookieOptions);
                HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi");
                TempData["Message"] = "Ürün Sepetinize Eklendi";
            }
            //string url = Request.Headers["Referer"].ToString();
            //return Redirect(url);
            return Request.Headers["Referer"].ToString();

        }


        public IActionResult Cart()
        {
            List<Cls_Order> MyCart;

            //silme butonu ile geldim
            if (HttpContext.Request.Query["scid"].ToString() != "")
            {
                int scid = Convert.ToInt32(HttpContext.Request.Query["scid"].ToString());
                cls_order.MyCart = Request.Cookies["sepetim"];
                cls_order.DeleteFromMyCart(scid.ToString());

                var cookieOptions = new CookieOptions();
                Response.Cookies.Append("sepetim", cls_order.MyCart, cookieOptions);
                cookieOptions.Expires = DateTime.Now.AddDays(7);
                TempData["Message"] = "Ürün Sepetinizden Silindi";
                MyCart = cls_order.SelectMyCart();
                ViewBag.MyCart = MyCart;
                ViewBag.MyCart_Table_Details = MyCart;
            }
            else
            {
                //sag üst kösedeki Sepet sayfama git butonu ile geldim
                var cookie = Request.Cookies["sepetim"];
                if (cookie == null)
                {
                    //sepette hic ürün olmayabilir
                    var cookieOptions = new CookieOptions();
                    cls_order.MyCart = "";
                    MyCart = cls_order.SelectMyCart();
                    ViewBag.MyCart = MyCart;
                    ViewBag.MyCart_Table_Details = MyCart;
                }
                else
                {
                    //sepette ürün var
                    var cookieOptions = new CookieOptions();
                    cls_order.MyCart = Request.Cookies["sepetim"];
                    MyCart = cls_order.SelectMyCart();
                    ViewBag.MyCart = MyCart;
                    ViewBag.MyCart_Table_Details = MyCart;
                }
            }

            if (MyCart.Count == 0)
            {
                ViewBag.MyCart = null;
            }

            return View();
        }

        public IActionResult Details(int id)
        {
            //ürün detayına tıklanınca, sepete eklenince Highlighted kolonunun değerini 1 arttıracağız
            Cls_Product.Highlighted_Increase(id);
            return View();
        }


        [HttpGet]
        public IActionResult Order()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                User? user = Cls_User.SelectMemberInfo(HttpContext.Session.GetString("Email").ToString());
                return View(user);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        //metod overload = aynı parametre sırasıyla, aynı isimli metodu yazamayız
        //metod overload etmek için parametre sırası farklı olmalı
        [HttpPost]
        public IActionResult Order(IFormCollection frm)
        {
            //string? kredikartno = Request.Form["kredikartno"]; //IFormCollection olmadan
            string kredikartno = frm["kredikartno"].ToString(); //IFormCollection zorunlu
            string kredikartay = frm["kredikartay"].ToString();
            string kredikartyil = frm["kredikartyil"].ToString();
            string kredikartcvs = frm["kredikartcvs"].ToString();

            //bankaya git, eğer true gelirse(onay) Order tablosuna kayıt atacağız
            //Order tablosuna kayıt atacağız
            //digital-planet (e-fatura) bilgilerini gönder

            //payu -- 100tlnin 1.90 alır - perşembe
            //iyzico -- 100tlnin 1 tlsi -- 40 gün


            string txt_tckimlikno = frm["txt_tckimlikno"].ToString();
            string txt_vergino = frm["txt_vergino"].ToString();

            if (txt_tckimlikno != "")
            {
                WebServiceController.tckimlikno = txt_tckimlikno;
                //fatura bilgilerini digital-planet şirketine gönderirsiniz(xml formatında)
                //sizin e-faturanızı oluşturacak
            }
            else
            {
                WebServiceController.vergino = txt_vergino;
            }

            // .Net yazarken , payu windowsForm ile yazılmış proje gönderiyor
            NameValueCollection data = new NameValueCollection();
            string url = "https://www.sedattefci.com/backref";
            data.Add("BACK_REF", url);
            data.Add("CC_CVV", kredikartcvs);
            data.Add("CC_NUMBER", kredikartno);
            data.Add("EXP_MONTH", kredikartay);
            data.Add("EXP_YEAR", kredikartyil);

            var deger = "";
            foreach (var item in data)
            {
                var value = item as string;
                var byteCount = Encoding.UTF8.GetBytes(data.Get(value));
                deger += byteCount + data.Get(value);
            }

            var signatureKey = "payu üyeliğinde size verilen SECRET_KEY burada olacak";
            var hash = HashWithSignature(deger, signatureKey);

            data.Add("ORDER_HASH", hash);

            var x = POSTFormPAYU("https://secure.payu.com.tr/Order/...", data);

            //sanal kart
            if (x.Contains("<STATUS><SUCCESS></STATUS>") && x.Contains("<RETURN_CODE><3DS_ENROLLED></RETURN_CODE>"))
            {
                //sanal kart OK
            }
            else
            {
                //gerçek kredi kartı
            }

            return RedirectToAction("backref");
        }

        public static string POSTFormPAYU(string url, NameValueCollection data)
        {
            return "";
        }

        public static string HashWithSignature(string deger, string signatureKey)
        {
            return "";
        }

        public IActionResult backref()
        {
            Confirm_Order();
            return RedirectToAction("Confirm");
        }

        public static string OrderGroupGUID = "";




		public IActionResult Confirm_Order()
		{

			//siparis tablosuna kaydet
			//cookie sepetini silecegiz
			//e-fatura olusturacagız, e-fatura olusturan xml metodu cagıracagız
			var cookieOptions = new CookieOptions();
			var cookie = Request.Cookies["sepetim"];  //10=1&20=1=30=3
			if (cookie != null)
			{
				cls_order.MyCart = cookie; //tarayıcıdaki sepet bilgilerini,property ye koydum
				OrderGroupGUID = cls_order.WriteToOrderTable(HttpContext.Session.GetString("Email"));
				cookieOptions.Expires = DateTime.Now.AddDays(7);
				Response.Cookies.Delete("sepetim");

				bool result = Cls_User.SendSms(OrderGroupGUID);
				if (result == false)
				{
					//Orders tablosunda sms kolonuna false degeri basılır,admin panele menü yapılır
					//Orders tablosunda sms kolonu = false olan siparişleri getir
				}

				//Cls_User.SendEMail(OrderGroupGUID);

				//1) sedattefci.com sitesinde müsteriden kredi kart bilgiler alınır
				//2) bu bilgiler payu yada iyzico (bu 2 site banka ile haberlesir) sitesine gönderilir
				//3) kredi kart bilgileri bankaya geldiginde,banka kullanıcıya sms onayı gönderir
				//4) banka backref metoduna geri dönüş yapar, banka kredi karta ok verimişse,
				//siz bir sms firmasıyla (netgsm) anlaştınız, SendSms metodu müşteriye siparişiniz onaylanı sms gönderir
				//biz sms iceriklerini  sms firmasına (netgsm) gönderecegiz (xml formatında), o firma sms gönderme işlemi yapacak
				//digitalplanet müsteriye e-fatura gönderir.ben digital planet şirketine siparişin icerigini gönderirir(xml formatında)
			}
			return RedirectToAction("Confirm");
		}


		public IActionResult Confirm()
		{
			ViewBag.OrderGroupGUID = OrderGroupGUID;

			return View();
		}


		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Register(User user)
		{
			if (Cls_User.loginEmailControl(user) == false)
			{
				bool answer = Cls_User.AddUser(user);

				if (answer)
				{
					TempData["Message"] = "Kaydedildi.";
					return RedirectToAction("Login");
				}
				TempData["Message"] = "Hata.Tekrar deneyiniz.";
			}
			else
			{
				TempData["Message"] = "Bu Email Zaten Mevcut.Başka Deneyiniz.";
			}
			return View();
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(User user)
		{
			var answer = Cls_User.MemberControl(user);

			if (answer.Message == "error")
			{
				TempData["Message"] = "Hata. Email ve/veya Şifre Yanlış";
			}
			else if (answer.Message == "admin")
			{
				//email ve şifre doğru ve admin
				HttpContext.Session.SetString("Admin", "Admin");
				HttpContext.Session.SetString("Admin", answer.Message);
				HttpContext.Session.SetString("Email", answer.Email);
				return RedirectToAction("Order", "Home");
			}
			else
			{
				//email şifre doğru ve sıradan bir kullanıcı
				HttpContext.Session.SetString("Email", answer.Email);
				return RedirectToAction("Index", "Home");
			}
			return View();
		}
	
        public IActionResult MyOrders()
		{
			if (HttpContext.Session.GetString("Email") != null)
			{
				List<vw_MyOrders> orders = cls_order.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
				return View(orders);
			}
			else
			{
				return RedirectToAction("Login");
			}
		}

        public IActionResult DetailedSearch()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Suppliers = context.Suppliers.ToList();

            return View();
        }



	}









    }

