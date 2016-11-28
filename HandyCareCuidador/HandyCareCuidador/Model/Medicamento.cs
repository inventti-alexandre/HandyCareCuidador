using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("Medicamento")]
    [ImplementPropertyChanged]
    public class Medicamento
    {

        public string Id { get; set; }
        public float MedQuantidade { get; set; }

        public string MedApresentacao { get; set; }

        public string MedViaAdministracao { get; set; }

        public string MedDescricao { get; set; }
        public string MedPacId { get; set; }
        public string MedUnidade { get; set; }

    }
}