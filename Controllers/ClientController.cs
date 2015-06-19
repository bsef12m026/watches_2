using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchExpo.Models ;

namespace WatchExpo.Controllers
{
    public class ClientController : Controller

    {
        ICustomer icus;
        public ClientController(ICustomer repo)
        {
            icus = repo;
        }

        //
        // GET: /Client/

        WatchCenterEntities1 db = new WatchCenterEntities1();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ViewWatches()
        {
            return View();
        }
        
        public ActionResult BuyWatch()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult Rado()
        {
            return View();
        }

        public ActionResult Rolex()
        {
            return View();
        }

        public ActionResult Search()
        {

            return View();
        }

        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View(db.Customers.ToList());
        }

        public ActionResult LoginConfirm(Customer cus)
        {

            return RedirectToAction("Rado");
        }

        public ActionResult buyNew()
        {
            var brand = Request["brandd"];
            var model = Request["model"];

            var wid = db.Watches.Where(s => s.Brand == brand && s.Model == model).ToList();
            Watch w = new Watch() ;
            w = wid[0] ;
            var cid = (int)Session["id"];
            Customer c = new Customer();
            
            DateTime dt = DateTime.Now ;
            Purchase p = new Purchase();
            p.CusId = cid ;
            p.WatchId = w.Id;
            p.Date = dt;
            db.Purchases.Add(p);
            db.SaveChanges();


            return RedirectToAction("Rado");
        }

       [HttpPost]
        public ActionResult SaveCus(Customer cus)
        {
            for (int j = 0; j < Request.Files.Count; j++)
            {
                HttpPostedFileBase file = Request.Files[j];
                cus.Photo = "../../Content/img/" + file.FileName;
                // file.SaveAs(Server.MapPath(@"~\Files\" + file.FileName));
                file.SaveAs(Server.MapPath(@"~\Content\img\" + file.FileName));
            }
            

            icus.saveCus(cus);
            return RedirectToAction("Index");
        }

        public JsonResult Check()
        {
            String bran = Request["Brand"];

            List<Watch> list = new List<Watch>() ;
            list = db.Watches.Where(s => s.Brand == bran).ToList();

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

    
        public JsonResult CheckUserName()
        {
            bool value;
            String userName = Request["Uname"];

            //check from database
            

            var cus = db.Customers.Where(s => s.Username == userName).ToList();
            if (cus.Count==0)
            {
                value = false;
            }
            else
            {
                value = true;
            }


            return this.Json(value, JsonRequestBehavior.AllowGet);

        }

        public ActionResult validateCus()
        {

            String Username1 = Request["Uname"];
            String Password1 = Request["pass"];
            List<Customer> ls = new List<Customer>() ;
            Customer c = new Customer()  ;
            c = null;
            var cu = db.Customers.Where(s=>s.Username == Username1 && s.Password == Password1).ToList();
            for(int i = 0 ; i < cu.Count() ; i++)
            {
                c = cu[i] ;
                ls.Add(c) ;
            }
            var y = db.Customers.Where(x => x.Username == Username1 && x.Password == Password1).ToList();
            if (Username1 == "Admin" && Password1 == "admin123")
            {
                Session["Login"] = 1;
                Session["id"] = c.Id;
                Session["name"] = c.Name;
                Session["LoginAdmin"] = 1;
                return RedirectToAction("AdminPanel", "Admin");
            }
            else if( c!= null)
            {
                Session["Login"] = 1;
                Session["id"] = c.Id;
                Session["name"] = c.Name;
                Session["LoginAdmin"] = 1;
                return RedirectToAction("Index");
            }
            else if (y.Count== 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("ContactUs");
            }

        }

        public JsonResult GetCities(int id)
        {
            List<Watch> wt = new List<Watch>();
            //String s1 = ("4000");
            var x = db.Watches.Where(w => w.Id == id);
            if (id == 0 )
            {
                x = db.Watches.Where( e=>e.Price < 4000 );
            }
            else if(id == 1)
            {
                x = db.Watches.Where(e => e.Price > 4000 && e.Price < 8000 );
            }
            else if (id == 2)
            {
                x = db.Watches.Where(e => e.Price >8000 && e.Price < 12000);
            }
            else if (id == 3)
            {
                x = db.Watches.Where(e => e.Price >12000 && e.Price < 16000 );

            }
            else if (id == 4)
            {
                x = db.Watches.Where(e => e.Price >16000 );
            }
            return this.Json(x, JsonRequestBehavior.AllowGet);
        }

        public ActionResult signOut()
        {
            Session["Login"] = 0 ;
            
            Session["LoginAdmin"] =  0 ;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            
            return RedirectToAction("SignIn");
        }
        public ActionResult SearchResult()
        {

            
            return View();
        }


        public ActionResult SearchByBrand()
        {

            string brand = Request["SerByBrand"];
            List<Watch> watc = new List<Watch>();
            var w = db.Watches.Where(s => s.Brand == brand); 
            foreach (var x in w)
            {
                if (x.Brand == brand)
                {
                    watc.Add(x);
                    
                }

            }
            return RedirectToAction("SearchResult");
        }
    }
}
