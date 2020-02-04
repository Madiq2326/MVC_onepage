using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Dapper;
using WebApplication2.ModelView;

namespace WebApplication2.Controllers
{
    public class RolesController : Controller
    {
        SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

        public ActionResult Index()
        {
            return View(GetDataRoles());
        }

        public async Task<JsonResult> GetDataRoles()
        {
            var result = await sql.QueryAsync<RoleVM>("EXEC SP_Retrive_Role");
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Create(RoleVM roleVM)
        {
            var affectedRows = await sql.ExecuteAsync("EXEC SP_Insert_Role @Name", new { Name = roleVM.Name });
            return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(int Id)
        {
            var result = await sql.QueryAsync<RoleVM>("EXEC SP_Retrive_By_Id @Id", new { Id = Id });
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Edit(RoleVM roleVM)
        {
            var affectedRows = await sql.QueryAsync<RoleVM>("EXEC SP_Update_Role @Id, @Name", new { Id = roleVM.Id, Name = roleVM.Name });
            return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(int Id)
        {
            var affectedRows = await sql.QueryAsync<RoleVM>("EXEC SP_Delete_Role @Id", new { Id = Id });
            return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        }
    }  
}