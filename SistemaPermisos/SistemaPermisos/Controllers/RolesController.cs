using SistemaPermisos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaPermisos.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {



            using (var bd = new ApplicationDbContext())
            {
                var roles = (from r in bd.ROL
                         select new
                         {
                             r.ID,
                             r.NOMBRE
                         }
                        ).ToList();


              
                return Json(roles, JsonRequestBehavior.AllowGet);
            }
           
        }



        [HttpPost]
        public JsonResult Filter(ROL rol)
        {

            using (var bd = new ApplicationDbContext())
            {
                var roles = (from r in bd.ROL
                             where r.ID == rol.ID
                             select new
                             {
                                 r.ID,
                                 r.NOMBRE
                             }
                        ).ToList();

              
                return Json(roles, JsonRequestBehavior.AllowGet);
            }

        }




        [HttpPost]
        public int Add(ROL rol)
        {

            int nfilasAfectadas = 0;

            try
            {

                using (var bd = new ApplicationDbContext())
                {

                    if (rol.ID == 0)
                    {
                        //AGREGAR
                        rol.FECHA_ALTA = DateTime.Now;
                        rol.ACTIVO = true;
                        bd.ROL.Add(rol);
                        bd.SaveChanges();
                        nfilasAfectadas = 1;

                    }
                    else
                    {
                        //EDITAR
                        var o = bd.ROL.ToList().Find(r => r.ID == rol.ID);
                        o.NOMBRE = rol.NOMBRE;
                        bd.ROL.Add(o);
                        bd.Entry(o).State = EntityState.Modified;
                        bd.SaveChanges();
                        nfilasAfectadas = 1;
                    }

                }
            }
            catch (Exception EX)
            {
                var AJAS =EX.ToString();
                return nfilasAfectadas;
            }

            return nfilasAfectadas;
        }

    }
}