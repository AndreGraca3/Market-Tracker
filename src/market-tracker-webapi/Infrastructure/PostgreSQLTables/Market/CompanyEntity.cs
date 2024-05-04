using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;

[Table("company", Schema = "MarketTracker")]
public class CompanyEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    [Column("name")]
    public required string Name { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Column("created_at")]
    public DateTime CreatedAt { get; } = DateTime.Now;

    public Company ToCompany()
    {
        return new Company
        {
            Id = Id,
            Name = Name,
            CreatedAt = CreatedAt
        };
    }
}
