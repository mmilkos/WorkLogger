using FluentAssertions;
using WorkLogger.Application._Queries.Users;
using WorkLogger.Domain;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Enums;
using WorkLogger.Infrastructure.Repositories;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Queries;

public class LoginUserQueryHandlerTests : BaseTests
{
    
    IConfiguration configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string> 
        { 
            { "Auth:JwtIssuer", "testIssuer" },
            { "Auth:JwtKey", "Lorem ipsum dolor sit amet, " +
                             "consectetur adipiscing elit, sed do eiusmod " +
                             "tempor incididunt ut labore et dolore magna aliqua. " +
                             "Ut enim ad minim veniam, quis nostrud exercitation ullamco " +
                             "laboris nisi ut aliquip ex ea commodo consequa" },
            { "Auth:JwtExpireHours", "1" }
        })
        .Build();
    
    [Fact]
    public async Task ValidRequest_ShouldReturnJwtToken()
    {
        var password = "test123";
        var repository = new WorkLoggerRepository(_dbContext);
        var company = await CompanyObjectMother.CreateAsync(_dbContext, "testCompany");
        var user = await UserObjectMother.CreateAsync(_dbContext, 
            company.Id,
            null,
            "Robert",
            "Lewandowski",
            "Lewy", 
            Roles.Manager,
            password: password);

        var loginDto = new LoginUserRequestDto()
        {
            UserName = user.UserName,
            Password = password
        };

        var Query = new LoginUserQuery(loginDto);
        
 
        var handler = new LoginUserQueryHandler(repository, configuration);

        var result = await handler.Handle(Query, CancellationToken.None);

        result.ErrorsList.Should().BeEmpty();
        result.Success.Should().BeTrue();
        result.Data.JwtToken.Should().NotBeNullOrEmpty();

    }
    
    [Fact]
    public async Task InvalidRequest_ShouldReturnError()
    {
        var password = "test123";
        var repository = new WorkLoggerRepository(_dbContext);
        var company = await CompanyObjectMother.CreateAsync(_dbContext, "testCompany");
        var user = await UserObjectMother.CreateAsync(_dbContext, 
            company.Id,
            null,
            "Robert",
            "Lewandowski",
            "Lewy", 
            Roles.Manager,
            password: password);

        var loginDto = new LoginUserRequestDto()
        {
            UserName = user.UserName,
            Password = ""
        };

        var Query = new LoginUserQuery(loginDto);
        
 
        var handler = new LoginUserQueryHandler(repository, configuration);

        var result = await handler.Handle(Query, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.Data.Should().BeNull();
        result.ErrorsList.Should().Contain(Errors.InvalidCredentials);
        result.ErrorType.Should().Be(ErrorTypesEnum.Unauthorized);
    }
}