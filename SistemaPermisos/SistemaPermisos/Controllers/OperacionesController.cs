using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SistemaPermisos.Interface;
using SistemaPermisos.Models;
using SistemaPermisos.Repository;

namespace SistemaPermisos.Controllers
{
    public class OperacionesController : Controller
    {

        public IGenericRepository<OPERACION> repository = null;
        public RolOperacionController rol_ope;

        // GET: Operaciones
        public ActionResult Index()
        {
            
            return View();
           
        }


        public OperacionesController()
        {
            this.repository = new GenericRepository<OPERACION>();
            this.rol_ope = new RolOperacionController();
          
        }

        public OperacionesController(IGenericRepository<OPERACION> repo)
        {
            this.repository = repo;
        }


        public JsonResult List()
        {
            try
            {
                var o = repository.GetAll().Select(
                x=> new
                {
                    x.ID,
                    x.NOMBRE,
                    x.ACTIVO,
                    FECHA_ALTA = x.FECHA_ALTA.ToShortDateString()

                }).Where(p=>p.ACTIVO == true).ToList();

                return Json(o, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return null;
            }
  
        }





        public JsonResult Filter(OPERACION ope)
        {
            try
            {
                var o = repository.GetAll().Select(
                x => new
                {
                    x.ID,
                    x.NOMBRE,
                    x.ACTIVO,
                    FECHA_ALTA = x.FECHA_ALTA.ToShortDateString()

                }).Where(p => p.ACTIVO == true && p.ID == ope.ID).ToList();

                return Json(o, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return null;
            }

        }


        public JsonResult Add(OPERACION ope)
        {
            string exist = "";

            try
            {

                if (ope.ID == 0)
                {
                    //ADD
                    var existe = repository.GetAll().Where(x => x.NOMBRE == ope.NOMBRE && x.ACTIVO == true  && x.ID > 0).FirstOrDefault();
                    if (existe != null)
                    {    
                        exist = ":( " + Environment.NewLine + existe.NOMBRE;
                    }
                    else
                    {

                        ope.FECHA_ALTA = DateTime.Now;
                        ope.ACTIVO = true;
                        repository.Add(ope);

                    }

                }
                else
                {
                    //UPDATE
                    var existe = repository.GetAll().Where(x => x.ID != ope.ID && x.NOMBRE == ope.NOMBRE && x.ACTIVO == true).FirstOrDefault();
                    if(existe!=null && existe.ACTIVO)
                    {
                        exist = ":( "  + Environment.NewLine + existe.NOMBRE;
                    }
                    else
                    {
                       
                        //GET OBJECT OPERACION
                        var o = repository.GetAll().Where(x => x.ID == ope.ID).FirstOrDefault();


                            //UPDATE OPERACION TABLE
                            o.FECHA_MOD = DateTime.Now;
                            o.NOMBRE = ope.NOMBRE;
                            repository.Update(o);

                    }
                }

            }
            catch (Exception ex)
            {
                var ajas = ex.ToString();
                return null;
            }


            if (exist!=null && exist!= "") { return Json(new { exist }); }


            return List();
        }


        public JsonResult Delete(int id)
        {
            try
            {
                var row = repository.GetAll().Where(x => x.ID == id).FirstOrDefault();
                if (row != null && row.ACTIVO)
                {
                 
                    //Delete ROL_OPERACIONES where ID == ID_OPERACION && ID_ROL == ID_ROL
                    var listOpe = rol_ope.repository.GetAll().Where(x => x.ID_OPERACION == row.ID).ToList();
                    foreach(var item in listOpe)
                    {
                        rol_ope.repository.Delete(item.ID);
                    }


                    //DELETE OPERACION
                    repository.Delete(row.ID);

                }
                else { return null; }

            }
            catch (Exception)
            {
                return null;
            }

            return List();

        }


    }
}