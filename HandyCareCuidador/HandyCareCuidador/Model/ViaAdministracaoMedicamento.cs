namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ViaAdministracaoMedicamento")]
    public partial class ViaAdministracaoMedicamento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ViaAdministracaoMedicamento()
        {
            Medicamento = new HashSet<Medicamento>();
        }

        public string Id { get; set; }
        public string ViaAdministracao { get; set; }

        public string ViaSubtipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Medicamento> Medicamento { get; set; }

        public virtual SubtipoViaAdministracaoMedicamento SubtipoViaAdministracaoMedicamento { get; set; }
    }
}
