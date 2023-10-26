using System.ComponentModel.DataAnnotations;

namespace ExpenseRecord.Models
{
    public class ExpenseRecordCreateRequest
    {
        [Required]
        [StringLength(50)]
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
    }
}
