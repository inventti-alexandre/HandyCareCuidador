namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ValidacaoCuidador")]
    public partial class ValidacaoCuidador
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ValidacaoCuidador()
        {
            Cuidador = new HashSet<Cuidador>();
        }

        public string Id { get; set; }

        [Column(TypeName = "image")]
        public byte[] ValDocumento { get; set; }

        public bool ValValidado { get; set; }
        public string CorenEnfermeiro { get; set; }

        public virtual CorenEnfermeiro CorenEnfermeiro1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cuidador> Cuidador { get; set; }
    }
}
