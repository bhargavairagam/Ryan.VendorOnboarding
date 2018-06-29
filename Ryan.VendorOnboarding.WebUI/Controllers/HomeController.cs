using Ryan.VendorOnboarding.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;

namespace Ryan.VendorOnboarding.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private const string URL = "https://sub.domain.com/objects.json";

        public ActionResult Index()
        {
            // LoadCountries();
            var vmodel = new VendorViewModel();
            vmodel.States = LoadStates();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json";
            
            using (Stream webStream = request.GetRequestStream())
           

            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream1 = webResponse.GetResponseStream())
                {
                    if (webStream1 != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream1))
                        {
                            string response = responseReader.ReadToEnd();
                            
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }

                return View(vmodel);
        }

        public ActionResult ThankYou()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Index(VendorViewModel vm , HttpPostedFileBase upload)
        {
            if(ModelState.IsValid)
            {
                var f = vm;
                 

                using (var cl = new HttpClient())
                {
                    cl.BaseAddress = new Uri(URL);



                }

                return RedirectToAction("ThankYou");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Missing Details");
                vm.States = LoadStates();
                return View(vm);
            }

            

          


           
        }

        private IEnumerable<SelectListItem> LoadStates()
        {
            //List<CircleOptionModel> lstCircleOption = new List<CircleOptionModel>()
            //{
            //    new CircleOptionModel("ASD)
            //};
            
           
            

            List<SelectListItem> li = new List<SelectListItem>();
            
            li.Add(new SelectListItem { Text = "ASB", Value = "ASB" });
            li.Add(new SelectListItem { Text = "901MARQ", Value = "901MARQ" });
            li.Add(new SelectListItem { Text = "UNION", Value = "UNION" });
            li.Add(new SelectListItem { Text = "TCF", Value = "TCF" });
            li.Add(new SelectListItem { Text = "ARBOR LKS", Value = "ARBORLKS" });
            li.Add(new SelectListItem { Text = "FCO", Value = "FCO" });
            li.Add(new SelectListItem { Text = "MIREF", Value = "MIREF" });
            li.Add(new SelectListItem { Text = "ARTIS", Value = "ARTIS" });
            li.Add(new SelectListItem { Text = "JDE", Value = "JDE" });


            return li;
            
        }


    }
}