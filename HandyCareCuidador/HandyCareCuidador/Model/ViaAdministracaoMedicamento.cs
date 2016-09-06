using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("ViaAdministracaoMedicamento")]
    [ImplementPropertyChanged]
    public class ViaAdministracaoMedicamento
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ViaAdministracaoMedicamento()
        {
            Medicamento = new HashSet<Medicamento>();
        }

        public string Id { get; set; }
        public string ViaAdministracao { get; set; }

        public string ViaSubtipo { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Medicamento> Medicamento { get; set; }

        public virtual SubtipoViaAdministracaoMedicamento SubtipoViaAdministracaoMedicamento { get; set; }
    }
}