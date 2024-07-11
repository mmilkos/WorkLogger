using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Commands;
using WorkLogger.Application._Commands.Users;
using WorkLogger.Domain;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Infrastructure.Repositories;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Commands;

public class RegisterUserCommandHandlerTests : BaseTests
{
    [Fact]
    public async Task ValidRequest_ShouldAddUserToDb()
    {
        //Arrange
        var repository  = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(_dbContext, "testCompany");
        
        var request = new RegisterUserRequestDto()
        {
            CompanyId = company.Id,
            Name = "John",
            Surname = "Doe",
            UserName = "johndoe",
            Role = (int)Roles.Employee,
            Password = "password123"
        };
        
        //Act
        var command = new RegisterUserRequestCommand(request);

        var handler = new RegisterUserCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        var user = await _dbContext.Users.FirstOrDefaultAsync();

        //Assert
        result.Success.Should().BeTrue();
        user.Should().NotBeNull();
        user.CompanyId.Should().Be(request.CompanyId);
        user.Name.Should().Be(request.Name);
        user.Surname.Should().Be(request.Surname);
        user.UserName.Should().Be(request.UserName);
        user.Role.Should().Be(Roles.Employee);
    }
    
    [Fact]
    public async Task ValidRequest_ShouldAddUserToEmployees()
    {
        //Arrange
        var repository  = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(_dbContext, "testCompany");
        
        var request = new RegisterUserRequestDto()
        {
            CompanyId = company.Id,
            Name = "John",
            Surname = "Doe",
            UserName = "johndoe",
            Role = (int)Roles.Employee,
            Password = "password123"
        };
        
        //Act
        var command = new RegisterUserRequestCommand(request);

        var handler = new RegisterUserCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        var user = await _dbContext.Users.FirstOrDefaultAsync();

        //Assert
        result.Success.Should().BeTrue();
        user.Should().NotBeNull();
        user.CompanyId.Should().Be(request.CompanyId);
    }
    
    
    [Fact]
    public async Task ValidRequestAddCeo_ShouldAddUserToEmployeesAndCeo()
    {
        //Arrange
        var repository  = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(_dbContext, "testCompany");
        
        var request = new RegisterUserRequestDto()
        {
            CompanyId = company.Id,
            Name = "John",
            Surname = "Doe",
            UserName = "johndoe",
            Role = (int)Roles.CEO,
            Password = "password123"
        };
        
        //Act
        var command = new RegisterUserRequestCommand(request);

        var handler = new RegisterUserCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        var user = await _dbContext.Users.FirstOrDefaultAsync();
        
        var updatedCompany = await _dbContext.Companies.FirstOrDefaultAsync();

        //Assert
        result.Success.Should().BeTrue();
        result.ErrorsList.Should().Equal(new List<string>() { });
        user.Should().NotBeNull();
        user.CompanyId.Should().Be(request.CompanyId);
    }
    
    [Fact]
    public async Task UserAlreadyExists_ShouldContainUserAlreadyExists()
    {
        //Arrange
        var repository  = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(_dbContext, "testCompany");

        var user = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id);
        
        var request = new RegisterUserRequestDto()
        {
            CompanyId = user.CompanyId,
            Name = user.Name,
            Surname = user.Surname,
            UserName =  user.UserName,
            Role = (int) user.Role,
            Password = "password123"
        };
        
        //Act
        var command = new RegisterUserRequestCommand(request);

        var handler = new RegisterUserCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorsList.Should().Contain(Errors.UserAlreadyExist);
        result.ErrorsList.Count.Should().Be(1);
        result.ErrorType.Should().Be(ErrorTypesEnum.BadRequest);
    }
    
    
    [Fact]
    public async Task CompanyDoesNotExist_ShouldReturnUserCompanyDoesNotExistException()
    {
        //Arrange
        var repository  = new WorkLoggerRepository(_dbContext);
        
        var request = new RegisterUserRequestDto()
        {
            CompanyId = -1,
            Name = "John",
            Surname = "Doe",
            UserName =  "johndoe",
            Role = (int)Roles.Employee,
            Password = "password123"
        };
        
        //Act
        var command = new RegisterUserRequestCommand(request);

        var handler = new RegisterUserCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorsList.Should().Contain(Errors.CompanyDoesNotExist);
        result.ErrorsList.Count.Should().Be(1);
        result.ErrorType.Should().Be(ErrorTypesEnum.BadRequest);
    }
    
    [Fact]
    public async Task InvalidRole_ShouldReturnRoleDoesNotExistException()
    {
        //Arrange
        var repository  = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(_dbContext, "testCompany");
        
        var request = new RegisterUserRequestDto()
        {
            CompanyId = company.Id,
            Name = "John",
            Surname = "Doe",
            UserName = "johndoe",
            Role = Int32.MaxValue,
            Password = "password123"
        };
        
        //Act
        var command = new RegisterUserRequestCommand(request);

        var handler = new RegisterUserCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeFalse();
        result.ErrorsList.Should().Contain(Errors.RoleDoesNotExist);
        result.ErrorsList.Count.Should().Be(1);
        result.ErrorType.Should().Be(ErrorTypesEnum.BadRequest);
    }
}