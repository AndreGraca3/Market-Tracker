﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables
{
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
    }
}
