﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;

[Table("operator", Schema = "MarketTracker")]
public class OperatorEntity
{
    [Key] [Column("user_id")] public required Guid UserId { get; set; }
    [Column("phone_number")] public required int PhoneNumber { get; set; }

    public Operator ToOperator(User user)
    {
        return new Operator(
            user,
            PhoneNumber
        );
    }
}