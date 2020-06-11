using SistemaPermisos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaPermisos.Controllers
{
    public class UsuariosController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult List()
        {

            var bd = new ApplicationDbContext();

            var o = bd.USUARIO.Select(

                p => new
                {
                    p.ID,
                    p.NOMBRE,
                    p.ACTIVO
                }

                ).ToList().Where(r => r.ACTIVO == true);


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



            return Json(o, JsonRequestBehavior.AllowGet);
            //}

        }





        [HttpPost]
        public JsonResult Filter(USUARIO user)
        {

            using (var bd = new ApplicationDbContext())
            {
                var o = (from x in bd.USUARIO
                         join r in bd.ROL
                         on x.ID_ROL equals r.ID
                         where x.ID == user.ID
                             select new
                             {
                                 x.ID,
                                 x.NOMBRE,
                                 SELECT_VALUE_ROL = r.ID,
                                 SELECT_DISPLAY_ROL = r.NOMBRE
                             }
                        ).ToList();


                return Json(o, JsonRequestBehavior.AllowGet);
            }

        }




        [HttpPost]
        public JsonResult Add(USUARIO u)
        {


            try
            {

                using (var bd = new ApplicationDbContext())
                {

                    if (u.ID == 0)
                    {
                        //AGREGAR
                        u.FECHA_ALTA = DateTime.Now;
                        u.ACTIVO = true;
                        bd.USUARIO.Add(u);
                        bd.SaveChanges();

                    }
                    else
                    {
                        //EDITAR
                        var o = bd.ROL.ToList().Find(r => r.ID == u.ID);
                        o.NOMBRE = u.NOMBRE;
                        o.FECHA_MOD = DateTime.Now;
                        bd.ROL.Add(o);
                        bd.Entry(o).State = EntityState.Modified;
                        bd.SaveChanges();

                    }

                }
            }
            catch (Exception EX)
            {
                var AJAS = EX.ToString();
                return null;
            }

            return List();
        }


        public JsonResult Delete(int id)
        {

            try
            {

                if (id > 0)
                {
                    using (var bd = new ApplicationDbContext())
                    {
                        var row = bd.USUARIO.ToList().Where(r => r.ID == id).First();
                        bd.USUARIO.Remove(row);
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