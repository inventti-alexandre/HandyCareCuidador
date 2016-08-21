namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CorenEnfermeiro")]
    public partial class CorenEnfermeiro
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CorenEnfermeiro()
        {
            ValidacaoCuidador = new HashSet<ValidacaoCuidador>();
        }

        public string CorenIdentificacao { get; set; }

        public bool CorenValidado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ValidacaoCuidador> ValidacaoCuidador { get; set; }
    }
}
