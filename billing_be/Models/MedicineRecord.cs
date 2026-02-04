using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace billing_be.Models
{

    public class MedicineRecord
    {
        [Key]
        [Column("record_id")]
        public int RecordId { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("name_clean")]
        public string? NameClean { get; set; }
    }
}
