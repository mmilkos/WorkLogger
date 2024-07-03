using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Commands.Companies;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Enums;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Commands;

public class RegisterCompanyWithCeoCommandHandlerTests : BaseTests
{
    [Fact]
    public async Task ValidRequest_ShouldAddCompanyToDb()
    {
        //Arrange
        var request = new RegisterCompanyDto()
        {
            CompanyName = "testCompany",
            Name = "Robert",
            Surname = "Lewandowski",
            UserName = "RLewandowski",
            Password = "qwertyuiop"
        };

        //Act
        var command = new RegisterCompanyWithCeoCommand(request);

        var result = await _mediator.Send(command);
        var company = await _dbContext.Companies.FirstOrDefaultAsync();
        var ceo = await _dbContext.Users.FirstOrDefaultAsync();
        
        //Assert
        result.Success.Should().BeTrue();
        result.ErrorsList.Count.Should().Be(0);
        company.Should().NotBeNull();
        company.Name.Should().Be(request.CompanyName);
        ceo.Should().NotBeNull();
        ceo.CompanyId.Should().Be(company.Id);
        ceo.Role.Should().Be(Roles.CEO);
    }
}