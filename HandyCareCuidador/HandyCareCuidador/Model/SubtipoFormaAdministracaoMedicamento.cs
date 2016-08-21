namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SubtipoFormaAdministracaoMedicamento")]
    public partial class SubtipoFormaAdministracaoMedicamento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubtipoFormaAdministracaoMedicamento()
        {
            FormaApresentacaoMedicamento = new HashSet<FormaApresentacaoMedicamento>();
        }

        [Column("SubtipoFormaAdministracaoMedicamento")]
        public string SubtipoFormaAdministracaoMedicamento1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormaApresentacaoMedicamento> FormaApresentacaoMedicamento { get; set; }
    }
}
