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

            var bd = new ApplicationDbContext();

            var roles = bd.ROL.Select(

                p => new
                {
                    p.ID,
                    p.NOMBRE,
                    p.ACTIVO,

                    VALUE = p.ID,
                    DISPLAY = p.NOMBRE
                }

                ).ToList().Where(r=> r.ACTIVO==true).ToList();

            
            //using (var bd = new ApplicationDbContext())
            //{
            //    var roles = (from r in bd.ROL
            //                 where r.ACTIVO == true
            //                 select new
            //                 {
            //                     r.ID,
            //                     r.NOMBRE
            //                 }
            //            ).ToList();



            return Json(roles, JsonRequestBehavior.AllowGet);
            //}

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
        public JsonResult Add(ROL rol)
        {


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

                    }
                    else
                    {
                        //EDITAR
                        var o = bd.ROL.ToList().Find(r => r.ID == rol.ID);
                        o.NOMBRE = rol.NOMBRE;
                        bd.ROL.Add(o);
                        bd.Entry(o).State = EntityState.Modified;
                        bd.SaveChanges();

                    }

                }
            }
            catch (Exception EX)
            {
                var AJAS =EX.ToString();
                return null;
            }

            return List();
        }


        public JsonResult Delete(int id)
        {

            try
            {

                if(id > 0)
                {
                    using (var bd = new ApplicationDbContext())
                    {
                        var row = bd.ROL.ToList().Where(r=>r.ID == id).First();
                        bd.ROL.Remove(row);
                        bd.SaveChanges();
                    }
                }

            }
            catch (Exception)
            {
                return null;
            }
            return List();
    }




    }

}