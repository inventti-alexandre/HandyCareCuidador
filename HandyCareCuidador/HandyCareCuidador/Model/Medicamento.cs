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
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Medicamento()
        {
            MedicamentoAdministrado = new HashSet<MedicamentoAdministrado>();
        }

        public string Id { get; set; }
        public float MedQuantidade { get; set; }

        public string MedApresentacao { get; set; }

        public string MedViaAdministracao { get; set; }

        public string MedDescricao { get; set; }
        public string MedPacId { get; set; }
        public string MedUnidade { get; set; }

        public virtual FormaApresentacaoMedicamento FormaApresentacaoMedicamento { get; set; }

        public virtual ViaAdministracaoMedicamento ViaAdministracaoMedicamento { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicamentoAdministrado> MedicamentoAdministrado { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}