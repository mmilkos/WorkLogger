﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Commands.Tasks;
using WorkLogger.Domain.DTOs;
using WorkLogger.Infrastructure.Repositories;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Queries;

public class GetAllCompanyTasksPagedQueryHandlerTests : BaseTests
{
    [Fact]
    public async Task ValidRequest_ShouldReturnCompanyTaskPaged()
    {
        //Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        
        var company1 = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, "testName");
        var company2 = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, "testName2");

        var team1 = await TeamObjectMother.CreateAsync(dbContext: _dbContext, companyId: company1.Id, name: "team1");
        
        var author1 = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company1.Id, name: "author1");
        var assignedUser1 = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company1.Id, name: "assignedUser1");
        
        var author2 = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company1.Id, name: "author2");
        var assignedUser2 = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company1.Id, name: "assignedUser2");
        
        var task1 = await UserTaskObjectMother.CreateAsync(dbContext: _dbContext, assignedUserId: assignedUser1.Id,
            authorId: author1.Id,
            teamId: team1.Id,
            name: "Task1", description: "Task1 desc");
        
        var task2 = await UserTaskObjectMother.CreateAsync(dbContext: _dbContext, assignedUserId: assignedUser2.Id,
            authorId: author2.Id,
            teamId: team1.Id,
            loggedHours: 10,
            name: "Task2", description: "Task2 desc");
        
        //different company
        var author3 = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company2.Id, name: "author3");
        var assignedUser3 = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company2.Id, name: "assignedUser3");
        var team2 = await TeamObjectMother.CreateAsync(dbContext: _dbContext, companyId: company2.Id, name: "team2");
        
        var task3 = await UserTaskObjectMother.CreateAsync(dbContext: _dbContext, assignedUserId: assignedUser3.Id,
            authorId: author3.Id,
            loggedHours: 10,
            teamId: team2.Id,
            name: "Task3", description: "Task3 desc");
        

        var dto = new PagedRequestDto()
        {
            CompanyId = company1.Id,
            Page = 1,
            PageSize = 10
        };

        var query = new GetAllCompanyTasksPagedQuery(dto);
        var handler = new GetAllCompanyTasksPagedQueryHandler(repository);

        var result = await handler.Handle(query, CancellationToken.None);

        result.ErrorsList.Should().BeEmpty();
        result.Success.Should().BeTrue();
        
        //we expect only 2 task from our company
        result.Data.DataList.Count.Should().Be(2);
        
        //task1 
        result.Data.DataList[0].Id.Should().Be(task1.Id);
        result.Data.DataList[0].Name.Should().Be(task1.Name);
        result.Data.DataList[0].Team.Should().Be(team1.Name);
        result.Data.DataList[0].AssignedUser.Id.Should().Be(assignedUser1.Id);
        result.Data.DataList[0].AssignedUser.Name.Should().Be(assignedUser1.Name);
        result.Data.DataList[0].AssignedUser.Surname.Should().Be(assignedUser1.Surname);
        result.Data.DataList[0].AssignedUser.Team.Should().Be(team1.Name);
        result.Data.DataList[0].AssignedUser.Role.Should().Be(assignedUser1.Role.ToString());
        
        //task2
        result.Data.DataList[1].Id.Should().Be(task2.Id);
        result.Data.DataList[1].Name.Should().Be(task2.Name);
        result.Data.DataList[1].Team.Should().Be(team1.Name);
        result.Data.DataList[1].AssignedUser.Id.Should().Be(assignedUser2.Id);
        result.Data.DataList[1].AssignedUser.Name.Should().Be(assignedUser2.Name);
        result.Data.DataList[1].AssignedUser.Surname.Should().Be(assignedUser2.Surname);
        result.Data.DataList[1].AssignedUser.Team.Should().Be(team1.Name);
        result.Data.DataList[1].AssignedUser.Role.Should().Be(assignedUser2.Role.ToString());
    }
}