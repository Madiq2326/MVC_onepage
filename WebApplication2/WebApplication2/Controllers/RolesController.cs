using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using API.Models;
using Dapper;
using Newtonsoft.Json;
using WebApplication2.ModelView;

namespace WebApplication2.Controllers
{
    //public class RolesController : Controller
    //{
    //    SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

    //    public ActionResult Index()
    //    {
    //        return View(GetDataRoles());
    //    }

    //    public async Task<JsonResult> GetDataRoles()
    //    {
    //        var result = await sql.QueryAsync<RoleVM>("EXEC SP_Retrive_Role");
    //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
    //    }

    //    public async Task<ActionResult> Create(RoleVM roleVM)
    //    {
    //        var affectedRows = await sql.ExecuteAsync("EXEC SP_Insert_Role @Name", new { Name = roleVM.Name });
    //        return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
    //    }

    //    public async Task<ActionResult> Details(int Id)
    //    {
    //        var result = await sql.QueryAsync<RoleVM>("EXEC SP_Retrive_By_Id @Id", new { Id = Id });
    //        return Json(new { data = result }, JsonRequestBehavior.AllowGet);
    //    }

    //    public async Task<ActionResult> Edit(RoleVM roleVM)
    //    {
    //        var affectedRows = await sql.QueryAsync<RoleVM>("EXEC SP_Update_Role @Id, @Name", new { Id = roleVM.Id, Name = roleVM.Name });
    //        return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
    //    }

    //    public async Task<ActionResult> Delete(int Id)
    //    {
    //        var affectedRows = await sql.QueryAsync<RoleVM>("EXEC SP_Delete_Role @Id", new { Id = Id });
    //        return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
    //    }


    //}

    public class RolesController : Controller
    {
        readonly HttpClient client = new HttpClient();

        public RolesController()
        {
            client.BaseAddress = new Uri("http://localhost:3893/API/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult Index()
        {
            return View(List());
        }

        public JsonResult List()
        {
            IEnumerable<RoleVM> roles = null;
            var responseTask = client.GetAsync("Roles");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<RoleVM>>();
                readTask.Wait();
                roles = readTask.Result;
            }
            else
            {
                roles = Enumerable.Empty<RoleVM>();
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(new { data = roles }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Create(RoleVM roleVM)
        {
            var myContent = JsonConvert.SerializeObject(roleVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var ByteContent = new ByteArrayContent(buffer);
            ByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var roles = client.PostAsync("Roles", ByteContent).Result;
            return Json(new { data = roles }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Details(int Id)
        { 
            var cek = client.GetAsync("Roles/" + Id).Result;
            var read = cek.Content.ReadAsAsync<RoleVM>().Result;
            return Json(new { data = read }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(int Id, RoleVM roleVM)
        {
            var myContent = JsonConvert.SerializeObject(roleVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var ByteContent = new ByteArrayContent(buffer);
            ByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var roles = client.PutAsync("Roles/" + Id, ByteContent).Result;
            return Json(new { data = roles }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int Id)
        {
            var roles = client.DeleteAsync("Roles/"+Id).ToString();
            return Json(new { data = roles }, JsonRequestBehavior.AllowGet);
        }
    }
}

