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

        public IGenericRepository<ROL> repository = null;
        public OperacionesController ope;
        public RolOperacionController rol_ope;

        public RolesController()
        {
            this.repository = new GenericRepository<ROL>();
            this.ope = new OperacionesController();
            this.rol_ope = new RolOperacionController();
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
                        FECHA_ALTA =  x.FECHA_ALTA.ToShortDateString(),

                        VALUE_ROL = x.ID,
                        DISPLAY_ROL = x.NOMBRE

                    }
                    ).Where(p=> p.ACTIVO == true).ToList();

 
            } catch (Exception)
            {
                o = null;
            }

            return Json(o, JsonRequestBehavior.AllowGet);
      

        }



        [HttpPost]
        public JsonResult Filter(ROL rol)
        {

            object o = null;

            try
            {

                o = repository.GetAll().Select(
                    x => new
                    {
                        x.ID,
                        x.NOMBRE,
                        x.ACTIVO,
                        FECHA_ALTA = x.FECHA_ALTA.ToShortDateString(),

                        VALUE_ROL = x.ID,
                        DISPLAY_ROL = x.NOMBRE

                    }
                    ).Where(p => p.ACTIVO == true && p.ID == rol.ID).ToList();


            }
            catch (Exception)
            {
                o = null;
            }

            return Json(o, JsonRequestBehavior.AllowGet);
     
   
        }




        [HttpPost]
        public JsonResult Add(ROL rol)
        {

            bool exist = false;

            try
            {

                if (rol.ID == 0)
                {
                    //AGREGAR
                    var existe = repository.GetAll().Where(x=> x.NOMBRE == rol.NOMBRE && x.ACTIVO == true).FirstOrDefault();

                    if (existe!=null && existe.ACTIVO) {exist = true;}
                    else
                    {
                        rol.FECHA_ALTA = DateTime.Now;
                        rol.ACTIVO = true;
                        repository.Add(rol);
                    }

                    }
                    else
                    {
                    //EDITAR   
                        var existe = repository.GetAll().Where(x=> x.NOMBRE == rol.NOMBRE && x.ID != rol.ID && x.ACTIVO == true).FirstOrDefault();
                        if (existe!=null && existe.ACTIVO) { exist = true; }
                        else
                        {
                            var o = repository.GetAll().Where(x => x.ID == rol.ID).FirstOrDefault();
                            o.NOMBRE = rol.NOMBRE;
                            repository.Update(o);
                        }

                    }
                
            }
            catch (Exception EX)
            {
                var AJAS =EX.ToString();
                return null;
            }

            if (exist) { return Json(new { success = exist }); } //if data exist
            
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
                }

            }
            catch (Exception)
            {
                return null;
            }
            return List();
    }



        public JsonResult LoadCheckBox()
        {

            try
            {


                var o = (from op in ope.repository.GetAll()
                         join rop in rol_ope.repository.GetAll()
                         on op.ID equals rop.ID_OPERACION
                         into tabla
                         from sub in tabla.DefaultIfEmpty()
                       
                         select new
                         {
                             op.ID,
                             op.NOMBRE,
                             CHECKED = sub?.ID ?? 0

                         }
                         ).ToList();


                return Json(o, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }

        }

    }

}