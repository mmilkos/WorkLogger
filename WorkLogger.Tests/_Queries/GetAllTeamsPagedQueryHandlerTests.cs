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
    [InlineData(1, 5)]
    [InlineData(2, 5)]
    [InlineData(1, 10)]
    public async Task ValidRequest_ReturnDataPaged(int page, int pageSize)
    {
        // Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        var company = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "Test Company" );
        var teams = new List<Team>();
        
        for (int i = 0; i < 10; i++)
        {
           var team = await TeamObjectMother.Create(dbContext: _dbContext, companyId: company.Id, name: $"test team {i}");
           teams.Add(team);
        }
        
        var dto = new PagedRequestDto()
        {
            Page = page,
            PageSize = pageSize,
            CompanyId = company.Id
        };
        var query = new GetAllTeamsPagedQuery(dto);
        var handler = new GetAllTeamsPagedQueryHandler(repository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.ErrorsList.Should().BeEmpty();
        result.Success.Should().BeTrue();
        result.Data.TotalRecords.Should().Be(teams.Count);
        result.Data.Data.Count.Should().Be(dto.PageSize);

        if (page == 2) result.Data.Data[0].Name.Should().Be(teams[5].Name);
    }
}