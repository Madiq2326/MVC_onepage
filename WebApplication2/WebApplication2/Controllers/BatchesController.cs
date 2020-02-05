using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using WebApplication2.ViewModel;

namespace WebApplication2.Controllers
{
    public class BatchesController : Controller
    {
        readonly HttpClient client = new HttpClient();
        public BatchesController()
        {
            client.BaseAddress = new Uri("https://brmapi.azurewebsites.net/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult Index()
        {
            return View(List());
        }

        public JsonResult List()
        {
            IEnumerable<BatchesVM> batches = null;
            var responseTask = client.GetAsync("Batches");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<BatchesVM>>();
                readTask.Wait();
                batches = readTask.Result;
            }
            else
            {
                batches = Enumerable.Empty<BatchesVM>();
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(new { data = batches }, JsonRequestBehavior.AllowGet);
        }
    }
}