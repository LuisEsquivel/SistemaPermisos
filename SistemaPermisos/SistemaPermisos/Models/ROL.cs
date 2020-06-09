namespace SistemaPermisos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ROL")]
    public partial class ROL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ROL()
        {
            ROL_OPERACION = new HashSet<ROL_OPERACION>();
            USUARIOs = new HashSet<USUARIO>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string NOMBRE { get; set; }

        public DateTime FECHA_ALTA { get; set; }

        public DateTime? FECHA_MOD { get; set; }

        public bool ACTIVO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ROL_OPERACION> ROL_OPERACION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO> USUARIOs { get; set; }
    }
}
