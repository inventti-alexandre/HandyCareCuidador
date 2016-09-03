using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCareCuidador.Model
{
    [Table("FotoFamiliar")]
    public class FotoFamiliar
    {
        public string Id { get; set; }
        public string FamId { get; set; }

        public string FotId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        [Column(TypeName = "timestamp")]
        public byte[] Version { get; set; }

        public bool? Deleted { get; set; }
    }
}