﻿using SistemaPermisos.Interface;
using SistemaPermisos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SistemaPermisos.Repository
{
  
        public class GenericRepository<T> : IGenericRepository<T> where T : class
        {
            private ApplicationDbContext _context = null;
            private DbSet<T> table = null;

            public GenericRepository()
            {
                this._context = new ApplicationDbContext();
                table = _context.Set<T>();
            }

            public GenericRepository(ApplicationDbContext _context)
            {
                this._context = _context;
                table = _context.Set<T>();
            }

            public IEnumerable<T> GetAll()
            {
                return table.ToList();
            }

            public T GetById(object id)
            {
               
              return  table.Find(id);
  
            }

            public void Add(T obj)
            {
                table.Add(obj);
                Save();
            }

            public void  Update(T obj)
            {
                table.Attach(obj);
                _context.Entry(obj).State = EntityState.Modified;
                Save();
            }

            public void Delete(object id)
            {
                T existing = table.Find(id);

               if(existing != null)
            {
                table.Remove(existing);
                Save();
            }
               
            }

            public void Save()
            {
                _context.SaveChanges();
            }
        }

}