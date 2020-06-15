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
        public RolesController r;
        public RolOperacionController rol_ope;

        // GET: Operaciones
        public ActionResult Index()
        {
            
            return View();
           
        }


        public OperacionesController()
        {
            this.repository = new GenericRepository<OPERACION>();
            this.r = new RolesController();
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
                    VALUE_ROL = x.ID_ROL,
                    FECHA_ALTA = x.FECHA_ALTA.ToShortDateString()

                }).Where(p=>p.ACTIVO == true).ToList();

                return Json(o, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return null;
            }
  
        }

        [HttpPost]
        public JsonResult FillCombos()
        {
            return r.List();
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
                    VALUE_ROL = x.ID_ROL,
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
                    var existe = repository.GetAll().Where(x => x.NOMBRE == ope.NOMBRE && x.ACTIVO == true && x.ID_ROL == ope.ID_ROL && x.ID > 0).FirstOrDefault();
                    if (existe != null)
                    {    
                        var nombreRol = "";
                        nombreRol = r.repository.GetAll().Where(x => x.ID == ope.ID_ROL && x.ACTIVO == true).FirstOrDefault().NOMBRE;
                        exist = ":( " + Environment.NewLine + existe.NOMBRE + Environment.NewLine + nombreRol;
                    }
                    else
                    {

                        ope.FECHA_ALTA = DateTime.Now;
                        ope.ACTIVO = true;
                        repository.Add(ope);

                        var id_operacion = repository.GetAll().Where(x => x.NOMBRE == ope.NOMBRE && x.ACTIVO == true).FirstOrDefault().ID;
                        ROL_OPERACION r_ope = new ROL_OPERACION();
                        r_ope.ID_ROL = ope.ID_ROL;
                        r_ope.ID_OPERACION = id_operacion;
                        rol_ope.Add(r_ope);

                    }

                }
                else
                {
                    //UPDATE
                    var existe = repository.GetAll().Where(x => x.ID != ope.ID && x.NOMBRE == ope.NOMBRE && x.ACTIVO == true && x.ID_ROL == ope.ID_ROL).FirstOrDefault();
                    if(existe!=null && existe.ACTIVO)
                    {
                        var nombreRol = "";
                        nombreRol = r.repository.GetAll().Where(x => x.ID == ope.ID_ROL && x.ACTIVO == true).FirstOrDefault().NOMBRE;
                        exist = ":( "  + Environment.NewLine + existe.NOMBRE + Environment.NewLine + nombreRol;

                    }
                    else
                    {
                       
                        //GET OBJECT OPERACION
                        var o = repository.GetAll().Where(x => x.ID == ope.ID).FirstOrDefault();

                        //UPDATE ROL_OPERACION TABLE N:N
                        var list = rol_ope.List();
                        ROL_OPERACION ro = new ROL_OPERACION();
                        ro.ID_OPERACION = o.ID;
                        ro.ID_ROL = o.ID_ROL;
                        rol_ope.Add(ro);


                            //UPDATE OPERACION TABLE
                            o.FECHA_MOD = DateTime.Now;
                            o.NOMBRE = ope.NOMBRE;
                            o.ID_ROL = ope.ID_ROL;
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
                    repository.Update(row);
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