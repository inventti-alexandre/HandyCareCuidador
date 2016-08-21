namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FormaApresentacaoMedicamento")]
    public partial class FormaApresentacaoMedicamento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FormaApresentacaoMedicamento()
        {
            Medicamento = new HashSet<Medicamento>();
        }

        public string ForSubtipo { get; set; }

        public string FormaApresentacao { get; set; }

        public virtual SubtipoFormaAdministracaoMedicamento SubtipoFormaAdministracaoMedicamento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Medicamento> Medicamento { get; set; }
    }
}
