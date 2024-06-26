﻿using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Market.Store.PreRegister;

public class PreRegistrationRepository(
    MarketTrackerDataContext dataContext
) : IPreRegistrationRepository
{
    public async Task<PaginatedResult<OperatorItem>> GetPreRegistersAsync(bool? isValid, int skip, int take)
    {
        var query = from preRegister in dataContext.PreRegister
            where isValid == null || preRegister.IsApproved == isValid
            select new OperatorItem(preRegister.Code, preRegister.OperatorName, preRegister.CompanyLogoUrl);

        var operators = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PaginatedResult<OperatorItem>(operators, query.Count(), skip, take);
    }

    public async Task<PreRegistration?> GetPreRegisterByIdAsync(Guid id)
    {
        var database = (await dataContext.PreRegister.FindAsync(id));
        var ola = database!.ToPreRegistration();
        return ola;
    }

    public async Task<PreRegistration?> GetPreRegisterByEmail(string email)
    {
        return (await dataContext.PreRegister.FirstOrDefaultAsync(user => user.Email == email))?.ToPreRegistration();
    }

    public async Task<PreRegistrationCode> CreatePreRegisterAsync(string operatorName, string email, int phoneNumber,
        string storeName,
        string storeAddress,
        string companyName,
        string companyLogoUrl,
        string? cityName,
        string document)
    {
        var newPreRegistration = new PreRegistrationEntity
        {
            OperatorName = operatorName,
            Email = email,
            PhoneNumber = phoneNumber,
            StoreName = storeName,
            StoreAddress = storeAddress,
            CompanyName = companyName,
            CompanyLogoUrl = companyLogoUrl,
            CityName = cityName,
            Document = document,
            CreatedAt = DateTime.UtcNow
        };
        await dataContext.PreRegister.AddAsync(newPreRegistration);
        await dataContext.SaveChangesAsync();
        return new PreRegistrationCode(newPreRegistration.Code);
    }

    public async Task<PreRegistration?> UpdatePreRegistrationById(Guid id, bool isApproved)
    {
        var preRegisterEntity = await dataContext.PreRegister.FindAsync(id);
        if (preRegisterEntity is null)
        {
            return null;
        }

        preRegisterEntity.IsApproved = isApproved;

        await dataContext.SaveChangesAsync();
        return preRegisterEntity.ToPreRegistration();
    }

    public async Task<PreRegistrationCode?> DeletePreRegisterAsync(Guid id)
    {
        var preRegisterEntity = await dataContext.PreRegister.FindAsync(id);
        if (preRegisterEntity is null)
        {
            return null;
        }

        dataContext.Remove(preRegisterEntity);
        await dataContext.SaveChangesAsync();
        return new PreRegistrationCode(preRegisterEntity.Code);
    }
}