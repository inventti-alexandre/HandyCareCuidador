using PropertyChanged;

namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PeriodoTratamento")]
    [ImplementPropertyChanged]
    public partial class PeriodoTratamento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PeriodoTratamento()
        {
            CuidadorPaciente = new HashSet<CuidadorPaciente>();
        }
        public string Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime PerInicio { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PerTermino { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuidadorPaciente> CuidadorPaciente { get; set; }
    }
}
