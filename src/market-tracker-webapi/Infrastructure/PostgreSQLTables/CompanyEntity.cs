using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables
{
  [Table("company", Schema = "MarketTracker")]
  public class CompanyEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public required string Name { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; } = DateTime.Now;
  }
}

