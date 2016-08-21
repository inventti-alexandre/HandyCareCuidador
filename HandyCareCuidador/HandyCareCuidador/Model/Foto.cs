namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Foto")]
    public partial class Foto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Foto()
        {
            Familiar = new HashSet<Familiar>();
        }
        public string Id { get; set; }

        [Column(TypeName = "image")]
        public byte[] FotoDados { get; set; }

        public string FotoDescricao { get; set; }

        public string FotCuidador { get; set; }

        public virtual Cuidador Cuidador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Familiar> Familiar { get; set; }
    }
}
