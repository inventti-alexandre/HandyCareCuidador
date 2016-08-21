namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SubtipoViaAdministracaoMedicamento")]
    public partial class SubtipoViaAdministracaoMedicamento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubtipoViaAdministracaoMedicamento()
        {
            ViaAdministracaoMedicamento = new HashSet<ViaAdministracaoMedicamento>();
        }

        public string SubtipoViaAdministracal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ViaAdministracaoMedicamento> ViaAdministracaoMedicamento { get; set; }
    }
}
