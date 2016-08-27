using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCareCuidador.Model
{
    [Table("FotoFamiliar")]
    public class FotoFamiliar
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