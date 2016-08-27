using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("Paciente")]
    [ImplementPropertyChanged]
    public class Paciente
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Paciente()
        {
            CuidadorPaciente = new HashSet<CuidadorPaciente>();
            Familiar = new HashSet<Familiar>();
        }

        public string Id { get; set; }
        public string PacNome { get; set; }

        public string PacSobrenome { get; set; }

        [Column(TypeName = "date")]
        public DateTime PacIdade { get; set; }

        public string PacMotivoCuidado { get; set; }

        public string PacNomeCompleto => PacNome + " " + PacSobrenome;

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuidadorPaciente> CuidadorPaciente { get; set; }

        public virtual MotivoCuidado MotivoCuidado { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Familiar> Familiar { get; set; }
    }
}