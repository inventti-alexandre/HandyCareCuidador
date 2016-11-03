using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("ConclusaoAfazer")]
    [ImplementPropertyChanged]
    public class ConclusaoAfazer
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public ConclusaoAfazer()
        //{
        //    MotivoNaoConclusaoTarefa = new HashSet<MotivoNaoConclusaoTarefa>();
        //    ValidacaoAfazer = new HashSet<ValidacaoAfazer>();
        //}

        public string Id { get; set; }
        public DateTime ConHorarioConcluido { get; set; }

        public bool ConConcluido { get; set; }
        public string ConAfazer { get; set; }

        //public virtual Afazer Afazer { get; set; }

        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<MotivoNaoConclusaoTarefa> MotivoNaoConclusaoTarefa { get; set; }

        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ValidacaoAfazer> ValidacaoAfazer { get; set; }
    }
}