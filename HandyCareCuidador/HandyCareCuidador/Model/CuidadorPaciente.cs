using System;
using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("CuidadorPaciente")]
    [ImplementPropertyChanged]
    public class CuidadorPaciente
    {
        public string Id { get; set; }

        public string CuiId { get; set; }

        public string PacId { get; set; }

        public string CuiPeriodoTratamento { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        [Column(TypeName = "timestamp")]
        public byte[] Version { get; set; }

        public bool? Deleted { get; set; }
        public virtual Cuidador Cuidador { get; set; }

        public virtual Paciente Paciente { get; set; }

        public virtual PeriodoTratamento PeriodoTratamento { get; set; }
    }
}