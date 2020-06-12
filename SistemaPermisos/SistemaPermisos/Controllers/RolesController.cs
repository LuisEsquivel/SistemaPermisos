using SistemaPermisos.Models;
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

        Repository<ROL> _repository;

        public RolesController(Repository<ROL> repositoriy)
        { 
            _repository = repositoriy;
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

                o = _repository.ListAll();
                List<ROL> rol = new List<ROL>();
                rol = (List<ROL>)o;

                o = rol.Select(
                    p => new
                    {
                        p.ID,
                        p.NOMBRE,
                        p.ACTIVO,

                        VALUE_ROL = p.ID,
                        DISPLAY_ROL = p.NOMBRE
                    }
                    ).ToList().Where(r => r.ACTIVO == true).ToList();
  
            } catch (Exception)
            {
                o = null;
            }

            return Json(o, JsonRequestBehavior.AllowGet);
      

        }



        [HttpPost]
        public JsonResult Filter(ROL rol)
        {

            using (var bd = new ApplicationDbContext())
            {
                var roles = (from r in bd.ROL
                             where r.ID == rol.ID
                             select new
                             {
                                 r.ID,
                                 r.NOMBRE
                             }
                        ).ToList().OrderByDescending(p => p.ID); ;

              
                return Json(roles, JsonRequestBehavior.AllowGet);
            }

        }




        [HttpPost]
        public JsonResult Add(ROL rol)
        {


            try
            {

                using (var bd = new ApplicationDbContext())
                {

                    if (rol.ID == 0)
                    {
                        //AGREGAR
                        rol.FECHA_ALTA = DateTime.Now;
                        rol.ACTIVO = true;
                        bd.ROL.Add(rol);
                        bd.SaveChanges();

                    }
                    else
                    {
                        //EDITAR
                        var o = bd.ROL.ToList().Find(r => r.ID == rol.ID);
                        o.NOMBRE = rol.NOMBRE;
                        bd.ROL.Add(o);
                        bd.Entry(o).State = EntityState.Modified;
                        bd.SaveChanges();

                    }

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
                    using (var bd = new ApplicationDbContext())
                    {
                        var row = bd.ROL.ToList().Where(r=>r.ID == id).First();
                        bd.ROL.Remove(row);
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