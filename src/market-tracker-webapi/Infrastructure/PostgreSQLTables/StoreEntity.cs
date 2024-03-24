using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables
{
    [Table("store", Schema = "MarketTracker")]
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
        [ForeignKey("city")]
        public required int? CityId { get; set; }
    
        [ForeignKey("company")]
        public int CompanyId { get; set; }
    }
}