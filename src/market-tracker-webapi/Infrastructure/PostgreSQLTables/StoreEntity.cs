using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables
{
    [Table("store", Schema = "MarketTracker")]
    [Index(nameof(Address), IsUnique = true)]
    public class StoreEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    
        [Required]
        [StringLength(200)]
        public required string Address { get; set; }
    
        [Required]
        [StringLength(30)]
        public required string City { get; set; }
    
        [DataType(DataType.Date)]
        public DateTime? OpenTime { get; set; }
    
        [DataType(DataType.Date)]
        public DateTime? CloseTime { get; set; }
    
        [ForeignKey("company")]
        public int CompanyId { get; set; }
    }
}