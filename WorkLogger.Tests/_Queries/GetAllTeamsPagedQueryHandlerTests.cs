using FluentAssertions;
using WorkLogger.Application._Queries.Teams;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Repositories;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Queries;

public class GetAllTeamsPagedQueryHandlerTests : BaseTests
{
    [Theory]
    [InlineData(2, 5)]
    public async Task ValidRequest_ReturnDataPaged(int page, int pageSize)
    {
        // Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        
        var company1 = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "Test Company1" );
        var company2 = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "Test Company2" );
        
        var company1Teams = new List<Team>();
        var company2Teams = new List<Team>();
        
        for (int i = 0; i < 10; i++)
        {
           var team1 = await TeamObjectMother.CreateAsync(dbContext: _dbContext, companyId: company1.Id, name: $"test team1 {i}");
           var team2 = await TeamObjectMother.CreateAsync(dbContext: _dbContext, companyId: company2.Id, name: $"test team2 {i}");
           company1Teams.Add(team1);
           company2Teams.Add(team2);
        }
        
        var dto = new PagedRequestDto()
        {
            Page = page,
            PageSize = pageSize,
            CompanyId = company1.Id
        };
        var query = new GetAllTeamsPagedQuery(dto);
        var handler = new GetAllTeamsPagedQueryHandler(repository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ErrorsList.Should().BeEmpty();
        result.Success.Should().BeTrue();
        result.Data.TotalRecords.Should().Be(company1Teams.Count);
        result.Data.DataList.Count.Should().Be(dto.PageSize);

        if (page == 2) result.Data.DataList[0].Name.Should().Be(company1Teams[5].Name);
    }
}