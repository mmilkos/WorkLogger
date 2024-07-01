﻿using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Domain.Interfaces;

public interface IWorkLoggerRepository
{
    Task AddCompanyAsync(Company company);
    Task<Company?> FindCompanyByIdAsync(int companyId);
    Task UpdateCompanyAsync(Company company);
    Task<bool> IsUserInDbAsync(string userName);
    Task AddUserAsync(User user);
    Task<List<Company>> GetAllCompaniesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}