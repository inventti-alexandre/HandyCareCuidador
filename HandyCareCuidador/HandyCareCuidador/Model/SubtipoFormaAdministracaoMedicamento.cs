using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HandyCareCuidador.Model
{
    [Table("SubtipoFormaAdministracaoMedicamento")]
    public class SubtipoFormaAdministracaoMedicamento
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubtipoFormaAdministracaoMedicamento()
        {
            FormaApresentacaoMedicamento = new HashSet<FormaApresentacaoMedicamento>();
        }

        [Column("SubtipoFormaAdministracaoMedicamento")]
        public string SubtipoFormaAdministracaoMedicamento1 { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormaApresentacaoMedicamento> FormaApresentacaoMedicamento { get; set; }
    }
}