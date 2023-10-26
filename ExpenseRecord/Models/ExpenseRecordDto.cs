using System.ComponentModel.DataAnnotations;

namespace ExpenseRecord.Models
{
    public class ExpenseRecordDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(50)]
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
