using API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{

    public class RolesController : ApiController
    {
        ApplicationDbContext myContext = new ApplicationDbContext();

        [HttpGet]
        public IQueryable<Roles> GetRoles()
        {
            return myContext.Roles;
        }

        [HttpGet]
        //[ResponseType(typeof(Roles))]
        public IHttpActionResult GetRolesById(int Id)
        {
            Roles roles = myContext.Roles.Find(Id);
            if(roles != null)
            {
                return Ok(roles);
            }
            return NotFound();
        }

        [HttpPost]
        //[ResponseType(typeof(Roles))]
        public IHttpActionResult Post(Roles roles)
        {
            if(!string.IsNullOrWhiteSpace(roles.Name))
            {
                myContext.Roles.Add(roles);
                var result = myContext.SaveChanges();
                if (result > 0)
                {
                    return Ok(roles);
                }
                //return CreatedAtRoute("DefaultApi", new { Id = roles.Id }, roles);
            }
            return BadRequest();
        }

        [HttpPut]
        //[ResponseType(typeof(void))]
        public IHttpActionResult Put(Roles roles, int Id)
        {
            var put = myContext.Roles.Find(Id);
            if (put != null)
            {
                if (!string.IsNullOrWhiteSpace(roles.Name))
                {
                    put.Name = roles.Name;
                    myContext.Entry(put).State = EntityState.Modified;
                    var result = myContext.SaveChanges();
                    if (result > 0)
                    {
                        return Ok(roles);
                    }
                    //return StatusCode(HttpStatusCode.NoContent)
                }
                return BadRequest();
            }
            return NotFound();
        }

        [HttpDelete]
        //[ResponseType(typeof(Roles))]
        public IHttpActionResult Delete(int Id)
        {
            var delete = myContext.Roles.Find(Id);
            if(delete != null)
            {
                myContext.Roles.Remove(delete);
                var result = myContext.SaveChanges();
                if(result > 0)
                {
                    return Ok();
                }
            }
            return NotFound();
            //return StatusCode(HttpStatusCode.OK);
        }
    }
}
