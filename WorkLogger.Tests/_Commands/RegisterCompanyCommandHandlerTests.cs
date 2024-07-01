using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Commands.Companies;
using WorkLogger.Domain.DTOs;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Commands;

public class RegisterCompanyCommandHandlerTests : BaseTests
{
    [Fact]
    public async Task ValidRequest_ShouldAddCompanyToDb()
    {
        //Arrange
        var request = new RegisterCompanyDto()
        {
            Name = "testName",
        };

        //Act
        var command = new RegisterCompanyCommand(request);

        var result = await _mediator.Send(command);
        var company = await _dbContext.Companies.FirstOrDefaultAsync();
        
        //Assert
        result.Should().Be(Unit.Value);
        company.Should().NotBeNull();
        company.Name.Should().Be(request.Name);
    }
}