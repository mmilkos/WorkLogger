using FluentAssertions;
using WorkLogger.Application._Queries.Users;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Repositories;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Queries;

public class GetAllUsersPagedQueryHandlerTests : BaseTests
{
    [Theory]
    [InlineData(2, 5)]
    public async Task ValidRequest_ReturnDataPaged(int page, int pageSize)
    {
        // Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        var company = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "Test Company" );
        var users = new List<User>();
        
        for (int i = 0; i < 10; i++)
        {
            var team = await UserObjectMother.CreateAsync(dbContext:_dbContext, companyId: company.Id, name:$"John{i}");
            users.Add(team);
        }
        
        var dto = new PagedRequestDto()
        {
            Page = page,
            PageSize = pageSize,
            CompanyId = company.Id
        };
        var query = new GetAllUsersPagedQuery(dto);
        var handler = new GetAllUsersPagedQueryHandler(repository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ErrorsList.Should().BeEmpty();
        result.Success.Should().BeTrue();
        result.Data.TotalRecords.Should().Be(users.Count);
        result.Data.Data.Count.Should().Be(dto.PageSize);

        if (page == 2) result.Data.Data[0].Name.Should().Be(users[5].Name);
    }
}