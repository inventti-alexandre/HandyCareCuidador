using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HandyCareCuidador.Model
{
    [Table("FormaApresentacaoMedicamento")]
    public class FormaApresentacaoMedicamento
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FormaApresentacaoMedicamento()
        {
            Medicamento = new HashSet<Medicamento>();
        }

        public string ForSubtipo { get; set; }

        public string FormaApresentacao { get; set; }

        public virtual SubtipoFormaAdministracaoMedicamento SubtipoFormaAdministracaoMedicamento { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Medicamento> Medicamento { get; set; }
    }
}