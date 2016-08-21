namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FotoFamiliar")]
    public partial class FotoFamiliar
    {
        [Column(Order = 0)]
        public string FamId { get; set; }

        [Column(Order = 1)]
        public string FotId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        [Column(TypeName = "timestamp")]
        public byte[] Version { get; set; }

        public bool? Deleted { get; set; }
    }
}
