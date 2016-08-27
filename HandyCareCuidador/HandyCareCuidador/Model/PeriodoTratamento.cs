using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("PeriodoTratamento")]
    [ImplementPropertyChanged]
    public class PeriodoTratamento
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PeriodoTratamento()
        {
            CuidadorPaciente = new HashSet<CuidadorPaciente>();
        }

        public string Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime PerInicio { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PerTermino { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuidadorPaciente> CuidadorPaciente { get; set; }
    }
}