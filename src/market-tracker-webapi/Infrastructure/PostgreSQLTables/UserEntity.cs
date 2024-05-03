﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto.User;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables;

[Table("user", Schema = "MarketTracker")]
public class UserEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("name")] public required string Name { get; set; }

    [Column("email")] public required string Email { get; set; }

    [Column("role")] public required string Role { get; set; }

    [DataType(DataType.Date)] [Column("created_at")]
    public readonly DateTime CreatedAt;

    public User ToUser()
    {
        return new User(
            Id,
            Name,
            Email,
            Role,
            CreatedAt
        );
    }

    public UserItem ToUserItem()
    {
        return new UserItem(
            Id,
            Name,
            Role
        );
    }
}