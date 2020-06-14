using SistemaPermisos.Interface;
using SistemaPermisos.Models;
using SistemaPermisos.Repository;
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

        private IGenericRepository<USUARIO> repository = null;
        private RolesController r;
        
        public UsuariosController()
        {
            this.repository = new GenericRepository<USUARIO>();
            r = new RolesController();
        }

        public UsuariosController(IGenericRepository<USUARIO> repository)
        {
            this.repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult List()
        {

            try
            {
                var o = repository.GetAll().Select(
                    x => new
                    {
                        x.ID,
                        x.NOMBRE,
                        VALUE_ROL = x.ID_ROL,
                        x.ACTIVO
                    }
                    ).Where(p => p.ACTIVO == true).ToList();


                return Json(o, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
             
     
        }





        [HttpPost]
        public JsonResult Filter(USUARIO user)
        {

            var o = repository.GetAll().Select(

                x => new
                {
                    x.ID,
                    x.NOMBRE,
                    VALUE_ROL = x.ID_ROL,
                    x.ACTIVO
                }

                ).Where(p=>p.ID == user.ID && p.ACTIVO == true).ToList();

                return Json(o, JsonRequestBehavior.AllowGet);
        
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
                    if (u.ID == 0)
                    {
                        //AGREGAR
                        u.FECHA_ALTA = DateTime.Now;
                        u.ACTIVO = true;
                        repository.Add(u);

                    }
                    else
                    {
                    //EDITAR
                        var o = repository.GetAll().Where(x=> x.ID == u.ID).First();
                        o.NOMBRE = u.NOMBRE;
                        o.ID_ROL = u.ID_ROL;
                        o.FECHA_MOD = DateTime.Now;
                        repository.Update(o);

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
                        var row = repository.GetById(id);
                        repository.Delete(row);  
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
