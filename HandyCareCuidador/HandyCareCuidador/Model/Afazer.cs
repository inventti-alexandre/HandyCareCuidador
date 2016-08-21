namespace HandyCareCuidador.Model
{
    using PropertyChanged;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Afazer")]
    [ImplementPropertyChanged]
    public partial class Afazer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Afazer()
        //{
        //    ConclusaoAfazer = new HashSet<ConclusaoAfazer>();
        //    MaterialUtilizado = new HashSet<MaterialUtilizado>();
        //    MedicamentoAdministrado = new HashSet<MedicamentoAdministrado>();
        //}

        public string Id { get; set; }

        public DateTime AfaHorarioPrevisto { get; set; }

        public string AfaObservacao { get; set; }
        public string AfaPaciente { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ConclusaoAfazer> ConclusaoAfazer { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MaterialUtilizado> MaterialUtilizado { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MedicamentoAdministrado> MedicamentoAdministrado { get; set; }
    }
}
