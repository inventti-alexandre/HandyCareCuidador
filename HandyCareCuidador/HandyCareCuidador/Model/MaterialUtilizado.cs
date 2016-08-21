namespace HandyCareCuidador.Model
{
    using PropertyChanged;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MaterialUtilizado")]
    [ImplementPropertyChanged]
    public partial class MaterialUtilizado
    {
        [Column(Order = 0)]
        public string MatAfazer { get; set; }

        [Column(Order = 1)]
        public string MatUtilizado { get; set; }
        [Column(Order = 2)]
        public string Id { get; set; }

        public int MatQuantidadeUtilizada { get; set; }

        //public virtual Afazer Afazer { get; set; }

        //public virtual Material Material { get; set; }
    }
}
