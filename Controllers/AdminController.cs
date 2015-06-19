
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchExpo.Models;

namespace WatchExpo.Controllers
{
    public class AdminController : Controller
    {
        IAdminWatches iwatches;
       

        public AdminController(IAdminWatches user )
        {
            iwatches = user;
            
        }
        //
        // GET: /Admin/
        WatchCenterEntities1 db = new WatchCenterEntities1();

        public ActionResult AdminPanel()
        {
            if (Session["Login"] == null)
            {

                return RedirectToAction("Index", "Client");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ViewCustomers()
        {
            return View();
        }

        public ActionResult AddCustomers()
        {

            return View();
        }

        public ActionResult ViewPurchases()
        {
            return View(db.Purchases.ToList());
        }

        public ActionResult SignOut()
        {
            
           // Session.Clear();
          
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            Session["Login"] = null;
            Session["name"] = null;
            Session["LoginAdmin"] = null;
            
           return RedirectToAction("Index","Client");
        }

        [HttpPost]
        public ActionResult Save(Watch wat)
        {

            for (int j = 0; j < Request.Files.Count; j++)
		    {
     			HttpPostedFileBase file = Request.Files[j];
     			wat.Photo = "../../Content/img/" + file.FileName;
               // file.SaveAs(Server.MapPath(@"~\Files\" + file.FileName));
                file.SaveAs(Server.MapPath(@"~\Content\img\" + file.FileName));
		    }
             iwatches.saveWatch(wat);
            return RedirectToAction("AddCustomers");
        }

     //   [HttpPost]
        public ActionResult SaveCustomer()
        {
            Customer cus = new Customer();

            String UserName = Request["Uname"];
            String Pasword = Request["Password"];
            String Name1 = Request["Name"];
            String Email = Request["Email"];
            String Cellno = Request["CellNo"];
            String Address = Request["Address"];
            byte[] img = { };

            
            cus.Username = UserName;
            cus.Password = Pasword;
            cus.Name = Name1;
            cus.Email = Email;
            cus.CellNo = Cellno;
            cus.Address = Address;
            cus.Photo = "asfa";

          //  iwatches.saveCustomer(cus);

            db.Customers.Add(cus);
            db.SaveChanges();
            return RedirectToAction("AdminPanel");
        }

        public JsonResult viewByCusName()
        {
            String Name = Request["customerName"];

            List<Customer> list = new List<Customer>();
            list = db.Customers.Where(s => s.Name == Name).ToList();

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult viewByCusId()
        {
            String Name = Request["customerName1"];
            int id = int.Parse(Name);
            List<Customer> list = new List<Customer>();
            list = db.Customers.Where(s => s.Id == id).ToList();

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult viewByCusAll()
        {
            //String Name = Request["customerName"];

            List<Customer> list = new List<Customer>();
            
            list = db.Customers.Where(s => s.Name != null).ToList();

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult viewWatchjs()
        {
            //String Name = Request["customerName"];

            List<Watch> list = new List<Watch>();

            list = db.Watches.Where(s => s.Model != null).ToList();

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckPurchase()
        {
            //String Name = Request["customerName"];

            List<Purchase> list = new List<Purchase>();

            list = db.Purchases.Where(s => s.Id != null).ToList();

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetails()
        {
            //var s = (from x in db.Purchases
            //         where x.Date == DateTime.Today
            //         select x).ToList();
            //foreach (Purchase a in s)
            //{
                
            //}


            return RedirectToAction("ViewPurchases");
        }

        public ActionResult Delete()
        {
            string name = Request["customerDel"];
            var q = db.Customers.Where(s => s.Name == name).ToList();
            List<Customer> c = new List<Customer>();
               c =  db.Customers.Where(x=>x.Name == name).ToList();
               if (name == "" )
               {
                   return RedirectToAction("SearchResult1");
               }

               for (int i = 0; i < c.Count; i++)
               {
                    db.Customers.Remove(c[i]);
               }
            db.SaveChanges();

            return RedirectToAction("SearchResult");
        }
      
        public ActionResult SearchResult()
        {

            return View();
        }

        public ActionResult SearchResult1()
        {

            return View();
        }
    }
}
