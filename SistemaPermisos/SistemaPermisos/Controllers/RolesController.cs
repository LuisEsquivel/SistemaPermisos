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
    public class RolesController : Controller
    {

        private IGenericRepository<ROL> repository = null;

        public RolesController()
        {
            this.repository = new GenericRepository<ROL>();
        }

        public RolesController(IGenericRepository<ROL> repository)
        {
            this.repository = repository;
        }


        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {

            object o = null;

            try
            {

                o = repository.GetAll().Select(
                    x=> new
                    {
                        x.ID,
                        x.NOMBRE,
                        x.ACTIVO,

                        VALUE_ROL = x.ID,
                        DISPLAY_ROL = x.NOMBRE

                    }
                    ).ToList().Where(p=> p.ACTIVO == true).ToList();

 
            } catch (Exception)
            {
                o = null;
            }

            return Json(o, JsonRequestBehavior.AllowGet);
      

        }



        [HttpPost]
        public JsonResult Filter(ROL rol)
        {

            var roles = repository.GetAll().Where(x=> x.ID == rol.ID).ToList(); 
            return Json(roles, JsonRequestBehavior.AllowGet);
   
        }




        [HttpPost]
        public JsonResult Add(ROL rol)
        {

            try
            {

                    if (rol.ID == 0)
                    {
                        //AGREGAR
                        rol.FECHA_ALTA = DateTime.Now;
                        rol.ACTIVO = true;
                        repository.Add(rol);
                        repository.Save();

                    }
                    else
                    {
                    //EDITAR
                        var o = repository.GetAll().Where(x=> x.ID == rol.ID).First();
                        o.NOMBRE = rol.NOMBRE;
                        repository.Update(o);
                        repository.Save();

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

                        var row = repository.GetById(id);
                        repository.Delete(row);
                        repository.Save();
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