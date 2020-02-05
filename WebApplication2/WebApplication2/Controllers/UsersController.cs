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
    public class UsersController : Controller
    {
        SqlConnection sql = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetDataUsers()
        {
            var result = await sql.QueryAsync<UserVM>("EXEC SP_Retrive_User");
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Create(UserVM userVM)
        {
            var affectedRows = await sql.ExecuteAsync("EXEC SP_Insert_User @Email, @PhoneNumber, @UserName", 
                new { Email = userVM.Email, PhoneNumber = userVM.PhoneNumber, UserName = userVM.UserName});
            return Json(new { data = affectedRows }, JsonRequestBehavior.AllowGet);
        }
    }
}
