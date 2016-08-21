using PropertyChanged;

namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Paciente")]
    [ImplementPropertyChanged]
    public partial class Paciente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuidadorPaciente> CuidadorPaciente { get; set; }

        public virtual MotivoCuidado MotivoCuidado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Familiar> Familiar { get; set; }
    }
}
