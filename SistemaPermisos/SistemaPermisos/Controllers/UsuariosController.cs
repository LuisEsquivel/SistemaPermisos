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
        private readonly Repository<USUARIO> _repository;
        private readonly Repository<ROL> _roles;
        RolesController r;


        public UsuariosController(Repository<USUARIO> repositoriy)
        {
            _repository = repositoriy;
            r = new RolesController(_roles);

        }

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult List()
        {

            using (var bd = new ApplicationDbContext())
            {
                var o = (from x in bd.USUARIO
                         where x.ACTIVO == true
                         orderby x.ID descending
                         select new
                         {
                             x.ID,
                             x.NOMBRE,
                             VALUE_ROL = x.ID_ROL
                         }
                        ).ToList();


                return Json(o, JsonRequestBehavior.AllowGet);
            }

        }





        [HttpPost]
        public JsonResult Filter(USUARIO user)
        {

            using (var bd = new ApplicationDbContext())
            {
                var o = (from x in bd.USUARIO   
                         where x.ACTIVO == true && x.ID == user.ID
                         orderby x.ID descending
                         select new
                             {
                                 x.ID,
                                 x.NOMBRE,
                                 VALUE_ROL = x.ID_ROL
                             }
                        ).ToList().OrderByDescending(p=>p.ID);


                return Json(o, JsonRequestBehavior.AllowGet);
            }

        }




        [HttpPost]
        public JsonResult FillCombos()
        {
            return r.List();
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
                        var o = bd.USUARIO.ToList().Find(r => r.ID == u.ID);
                        o.NOMBRE = u.NOMBRE;
                        o.ID_ROL = u.ID_ROL;
                        o.FECHA_MOD = DateTime.Now;
                        bd.USUARIO.Add(o);
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