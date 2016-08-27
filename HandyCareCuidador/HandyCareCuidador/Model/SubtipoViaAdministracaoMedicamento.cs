using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HandyCareCuidador.Model
{
    [Table("SubtipoViaAdministracaoMedicamento")]
    public class SubtipoViaAdministracaoMedicamento
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubtipoViaAdministracaoMedicamento()
        {
            ViaAdministracaoMedicamento = new HashSet<ViaAdministracaoMedicamento>();
        }

        public string SubtipoViaAdministracal { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ViaAdministracaoMedicamento> ViaAdministracaoMedicamento { get; set; }
    }
}