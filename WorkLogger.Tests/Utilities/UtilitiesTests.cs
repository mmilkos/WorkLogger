using FluentAssertions;
using WorkLogger.Domain.Enums;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests.Utilities;

public class UtilitiesTests : BaseTests
{
    [Fact]
    public void StatusCodesEnum_CodeShouldMatchDescription()
    {
        //Arrange
        var ok = (int)StatusCodesEnum.Ok;
        var badRequest = (int)StatusCodesEnum.BadRequest;
        var internalServerError = (int)StatusCodesEnum.InternalServerError;
        
        //Assert
        ok.Should().Be(200);
        badRequest.Should().Be(400);
        internalServerError.Should().Be(500);
    }
    
    [Fact]
    public void RolesEnum_RoleShouldMatchDescription()
    {
        //Arrange
        var admin = (int)Roles.Admin;
        var Manager = (int)Roles.Manager;
        var  Employee = (int)Roles.Employee;
        var CEO = (int)Roles.CEO;
        
        //Assert
        admin.Should().Be(0);
        Manager.Should().Be(1);
        Employee.Should().Be(2);
        CEO.Should().Be(3);
    }
}