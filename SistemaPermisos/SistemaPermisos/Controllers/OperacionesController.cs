using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaPermisos.Interface;
using SistemaPermisos.Models;
using SistemaPermisos.Repository;

namespace SistemaPermisos.Controllers
{
    public class OperacionesController : Controller
    {

        private  IGenericRepository<OPERACION> repository = null;
        private IGenericRepository<ROL_OPERACION> rol_ope = null;
        private IGenericRepository<ROL> rol = null;
        private RolesController r;

        // GET: Operaciones
        public ActionResult Index()
        {
            return View();
        }


        public OperacionesController()
        {
            this.repository = new GenericRepository<OPERACION>();
            this.rol_ope = new GenericRepository<ROL_OPERACION>();
            this.r = new RolesController();
        }

        public OperacionesController(IGenericRepository<OPERACION> repo, IGenericRepository<ROL_OPERACION> rol_ope, IGenericRepository<ROL> rol)
        {
            this.repository = repo;
            this.rol_ope = rol_ope;
            this.rol = rol;

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
                    var existe = repository.GetAll().Where(x => x.NOMBRE == ope.NOMBRE && x.ACTIVO == true && x.ID_ROL == ope.ID_ROL).FirstOrDefault();
                    if (existe != null) { exist = ":( " + existe.NOMBRE + " " + rol.GetAll().Where(x => x.ID == existe.ID_ROL).FirstOrDefault().NOMBRE; }
                    else
                    {

                        ope.FECHA_ALTA = DateTime.Now;
                        ope.ACTIVO = true;
                        repository.Add(ope);

                        var id_operacion = repository.GetAll().Where(x => x.NOMBRE == ope.NOMBRE && x.ACTIVO == true).FirstOrDefault().ID_OPERACION;
                        ROL_OPERACION r_ope = new ROL_OPERACION();
                        r_ope.ID_ROL = ope.ID_ROL;
                        r_ope.ID_OPERACION = ope.ID;
                        rol_ope.Add(r_ope);

                    }

                }
                else
                {
                    //UPDATE
                    var existe = repository.GetAll().Where(x => x.ID != ope.ID && x.NOMBRE == ope.NOMBRE && x.ACTIVO == true && x.ID_ROL == ope.ID_ROL).FirstOrDefault();
                    if(existe!=null && existe.ACTIVO) { exist = ":( "+existe.NOMBRE +" "+rol.GetAll().Where(x=>x.ID==ope.ID_ROL).FirstOrDefault().NOMBRE; }
                    else
                    {
                        //UPDATE OPERACION TABLE
                        var o = repository.GetAll().Where(x => x.ID == ope.ID).FirstOrDefault();
                        o.FECHA_MOD = DateTime.Now;
                        o.NOMBRE = ope.NOMBRE;
                        o.ID_ROL = ope.ID_ROL;
                        repository.Update(o);

                           //UPDATE ROL_OPERACION TABLE N:N
                            var ro = rol_ope.GetAll().Where(x=> x.ID_OPERACION == o.ID_OPERACION).FirstOrDefault();                         
                            ROL_OPERACION r_ope = new ROL_OPERACION();
                            ro.ID_ROL = ope.ID_ROL;
                            ro.ID_OPERACION = ope.ID;
                            rol_ope.Update(ro);
                      

                    }
                }

            }
            catch (Exception)
            {
                return null;
            }


            if (exist!=null || exist!= string.Empty) { return Json(new { success = exist }); }


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