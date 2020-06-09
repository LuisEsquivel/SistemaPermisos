namespace SistemaPermisos.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=ApplicationDbContext")
        {
        }

        public virtual DbSet<OPERACION> OPERACION { get; set; }
        public virtual DbSet<ROL> ROL { get; set; }
        public virtual DbSet<ROL_OPERACION> ROL_OPERACION { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OPERACION>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<OPERACION>()
                .HasMany(e => e.ROL_OPERACION)
                .WithRequired(e => e.OPERACION)
                .HasForeignKey(e => e.ID_OPERACION)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ROL>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<ROL>()
                .HasMany(e => e.ROL_OPERACION)
                .WithRequired(e => e.ROL)
                .HasForeignKey(e => e.ID_ROL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ROL>()
                .HasMany(e => e.USUARIOs)
                .WithRequired(e => e.ROL)
                .HasForeignKey(e => e.ID_ROL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);
        }
    }
}
