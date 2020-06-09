namespace SistemaPermisos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USUARIO")]
    public partial class USUARIO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string NOMBRE { get; set; }

        public DateTime FECHA_ALTA { get; set; }

        public DateTime? FECHA_MOD { get; set; }

        public int ID_ROL { get; set; }

        public bool ACTIVO { get; set; }

        public virtual ROL ROL { get; set; }
    }
}
