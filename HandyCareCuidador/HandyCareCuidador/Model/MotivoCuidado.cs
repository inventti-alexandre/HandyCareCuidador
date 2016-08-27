using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("MotivoCuidado")]
    [ImplementPropertyChanged]
    public class MotivoCuidado
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MotivoCuidado()
        {
            Paciente = new HashSet<Paciente>();
            TipoTratamento = new HashSet<TipoTratamento>();
        }

        public string Id { get; set; }
        public string MotDescricao { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Paciente> Paciente { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TipoTratamento> TipoTratamento { get; set; }
    }
}