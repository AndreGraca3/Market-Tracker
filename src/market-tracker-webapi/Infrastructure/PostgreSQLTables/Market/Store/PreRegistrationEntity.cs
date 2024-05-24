using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;

[Table("pre_registration", Schema = "MarketTracker")]
public class PreRegistrationEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("code")]
    [Key]
    public Guid Code { get; set; }

    [Required] [Column("operator_name")] public required string OperatorName { get; set; }

    [Required] [Column("email")] public required string Email { get; set; }

    [Required] [Column("phone_number")] public required int PhoneNumber { get; set; }

    [Required] [Column("store_name")] public required string StoreName { get; set; }

    [Required] [Column("store_address")] public required string StoreAddress { get; set; }

    [Required] [Column("company_name")] public required string CompanyName { get; set; }

    [Required]
    [Column("company_logo_url")]
    public required string CompanyLogoUrl { get; set; }

    [Column("city_name")] public string? CityName { get; set; }

    [DataType(DataType.Date)]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Required] [Column("document")] public required string Document { get; set; }

    [Column("is_approved")] public bool IsApproved { get; set; }

    public PreRegistration ToPreRegistration()
    {
        return new PreRegistration(
            Code,
            OperatorName,
            Email,
            PhoneNumber,
            StoreName,
            CompanyName,
            CompanyLogoUrl,
            StoreAddress,
            CityName,
            Document,
            CreatedAt,
            IsApproved
        );
    }
}