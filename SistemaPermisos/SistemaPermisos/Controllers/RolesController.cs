using SistemaPermisos.Interface;
using SistemaPermisos.Models;
using SistemaPermisos.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;

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
        public JsonResult Add(ROL rol, object [] checkbox)
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
                        using (var scope = new TransactionScope())
                        {
                            rol.FECHA_ALTA = DateTime.Now;
                            rol.ACTIVO = true;
                            repository.Add(rol);

                            if (UpdateOperaciones(rol.ID, checkbox) == false) return null;

                            scope.Complete();
                        }

                    }

                    }
                    else
                    {
                    //EDITAR   
                        var existe = repository.GetAll().Where(x=> x.NOMBRE == rol.NOMBRE && x.ID != rol.ID && x.ACTIVO == true).FirstOrDefault();
                        if (existe!=null && existe.ACTIVO) { exist = true; }
                        else
                        {

                        using (var scope = new TransactionScope())
                        {
                            var o = repository.GetAll().Where(x => x.ID == rol.ID).FirstOrDefault();
                            o.NOMBRE = rol.NOMBRE;
                            repository.Update(o);

                            if (UpdateOperaciones(rol.ID, checkbox) == false) return null;

                            scope.Complete();
                        }
  

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


       [HttpPost]
        public JsonResult Delete(int id)
        {

            try
            {

                if(id > 0)
                {

                    using (var scope = new TransactionScope())
                    {

                        if (UpdateOperaciones(id, null, true) == false) return null;
                        repository.Delete(id);
                   
                        scope.Complete();
                    }

                }

            }
            catch (Exception)
            {
                return null;
            }
            return List();
    }



        [HttpPost]
        public JsonResult LoadCheckBox(int id)
        {

            object o = null;

            try
            {

                if (id > 0)
                {


                    //get operaciones All
                    var op = ope.repository.GetAll()
                        .Select
                        (
                        x => new
                        {
                            x.ID,
                            x.NOMBRE,
                            CHECKED = 0
                        }
                        )
                        .ToList()
                        ;

                  //get operaciones by rol 
                   var rop =   rol_ope.repository.GetAll()
                        .Select
                        (
                         x=> new
                         {
                             x.ID_ROL,
                             x.ID_OPERACION
                         }
                        )
                        .Where
                        (
                        x => x.ID_ROL == id
                        )
                        .ToList();
                        ;


                    //join operaciones - rol
                    o = (from oper in op
                         join ro in rop
                         on oper.ID equals ro.ID_OPERACION

                         into tabla
                         from t in tabla.DefaultIfEmpty()

                         select new
                         {
                             oper.ID,
                             oper.NOMBRE,
                             CHECKED  = t?.ID_ROL ?? 0

                         }
                         ).ToList();

  
                }
                else
                {
                    o = ope.repository.GetAll()
                        .Select
                        (x => new
                        {
                            x.ID,
                            x.NOMBRE,
                            CHECKED = 0
                        }
                        ).OrderBy(p=> p.ID).ToList();

                }

                return Json(o, JsonRequestBehavior.AllowGet);

            }

            catch (Exception)
            {
                return null;
            }
        
        }


        public bool UpdateOperaciones(int id, object[] checkbox, bool delete = false)
        {

          
            try
            {
                if (delete == false)
                {

                    string[] split = null;

                    if (checkbox != null){
                        foreach (object item in checkbox)
                        {
                            split = item.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        }


                        foreach (string item in split)
                        {
                            var operacion = item;

                            if (operacion != null && operacion != "0")
                            {
                                var rop = rol_ope.repository.GetAll()
                                     .Where
                                     (
                                      x => x.ID_ROL == id
                                      &&
                                      x.ID_OPERACION == int.Parse(item)
                                     ).FirstOrDefault();


                                var o = new ROL_OPERACION();
                                o.ID_ROL = id;
                                o.ID_OPERACION = int.Parse(item);
                                rol_ope.repository.Add(o);


                            }

                        }

                    }
                }
                else
                {
                    foreach (var row in rol_ope.repository.GetAll().Where(x => x.ID_ROL == id).ToList())
                    {
                        rol_ope.repository.Delete(row.ID);
                    }
                }



            }
            catch (Exception)
            {
                return false;
            }


            return true;
        }


    }
    

    public class CheckBox
    {
        public int checkbox { get; set; }
    }


}