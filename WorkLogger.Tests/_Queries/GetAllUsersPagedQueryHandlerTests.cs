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
        var company1 = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "Test Company1" );
        var company2 = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "Test Company2" );
        var users = new List<User>();
        
        for (int i = 0; i < 10; i++)
        {
            var user1 = await UserObjectMother.CreateAsync(dbContext:_dbContext, companyId: company1.Id, name:$"John{i}");
            var user2 = await UserObjectMother.CreateAsync(dbContext:_dbContext, companyId: company2.Id, name:$"Steve{i}");
            users.Add(user1);
        }
        
        var dto = new PagedRequestDto()
        {
            Page = page,
            PageSize = pageSize,
            CompanyId = company1.Id
        };
        var query = new GetAllUsersPagedQuery(dto);
        var handler = new GetAllUsersPagedQueryHandler(repository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ErrorsList.Should().BeEmpty();
        result.Success.Should().BeTrue();
        result.Data.DataList.Count.Should().Be(dto.PageSize);
        
        //We expect users only from company1
        result.Data.TotalRecords.Should().Be(users.Count);

        if (page == 2) result.Data.DataList[0].Name.Should().Be(users[5].Name);
    }
}