using SistemaPermisos.Interface;
using SistemaPermisos.Models;
using SistemaPermisos.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaPermisos.Controllers
{
    public class RolOperacionController : Controller
    {

        public IGenericRepository<ROL_OPERACION> repository = null;
        // GET: RolOperacion
        public ActionResult Index()
        {
            return View();
        }

        public RolOperacionController()
        {
            this.repository = new GenericRepository<ROL_OPERACION>();
        }

        public RolOperacionController(IGenericRepository<ROL_OPERACION> repository)
        {
            this.repository = repository;
        }

        public JsonResult Add(ROL_OPERACION rol_ope)
        {
            try
            {

                if(rol_ope.ID == 0)
                {
                    //ADD
                    var exists = repository.GetAll().Where(x => x.ID_OPERACION == rol_ope.ID && x.ID_ROL == rol_ope.ID_ROL).FirstOrDefault();
                    if (exists != null) { return null; }
                    else
                    {
                        repository.Add(rol_ope);
                    }
                }
                else
                {
                    //UPDATE
                    var exist = repository.GetAll().Where(x => x.ID != rol_ope.ID && x.ID_OPERACION == rol_ope.ID_OPERACION && x.ID_ROL == rol_ope.ID_OPERACION).FirstOrDefault();
                    if(exist != null) { return null; }
                    else
                    {
                        repository.Update(rol_ope);
                    }
                }
            
             
            }
            catch (Exception)
            {
                return null;
            }

            return Json(repository.GetAll(), JsonRequestBehavior.AllowGet);
        }

        public List<ROL_OPERACION> List()
        {
            return repository.GetAll().ToList();
        }
    }
}