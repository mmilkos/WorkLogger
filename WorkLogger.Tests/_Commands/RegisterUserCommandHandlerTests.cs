using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Commands;
using WorkLogger.Application._Commands.Users;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Exceptions;
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
        
        var company = await CompanyObjectMother.CreateAsync(repository, "testCompany", new List<User>());
        
        var request = new RegisterUserDto()
        {
            CompanyId = company.Id,
            Name = "John",
            Surname = "Doe",
            UserName = "johndoe",
            Role = Role.Employee,
            password = "password123"
        };
        
        //Act
        var command = new RegisterUserCommand(request);

        var handler = new RegisterUserCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        var user = await _dbContext.Users.FirstOrDefaultAsync();

        //Assert
        result.Should().Be(Unit.Value);
        user.Should().NotBeNull();
        user.CompanyId.Should().Be(request.CompanyId);
        user.Name.Should().Be(request.Name);
        user.Surname.Should().Be(request.Surname);
        user.UserName.Should().Be(request.UserName);
        user.Role.Should().Be(request.Role);
    }
    
    [Fact]
    public async Task ValidRequest_ShouldAddUserToEmployees()
    {
        //Arrange
        var repository  = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(repository, "testCompany", new List<User>());
        
        var request = new RegisterUserDto()
        {
            CompanyId = company.Id,
            Name = "John",
            Surname = "Doe",
            UserName = "johndoe",
            Role = Role.Employee,
            password = "password123"
        };
        
        //Act
        var command = new RegisterUserCommand(request);

        var handler = new RegisterUserCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        var user = await _dbContext.Users.FirstOrDefaultAsync();
        
        var updatedCompany = await _dbContext.Companies.FirstOrDefaultAsync();

        //Assert
        result.Should().Be(Unit.Value);
        user.Should().NotBeNull();
        user.CompanyId.Should().Be(request.CompanyId);
    }
    
    
    [Fact]
    public async Task ValidRequestAddCeo_ShouldAddUserToEmployeesAndCeo()
    {
        //Arrange
        var repository  = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(repository, "testCompany", new List<User>());
        
        var request = new RegisterUserDto()
        {
            CompanyId = company.Id,
            Name = "John",
            Surname = "Doe",
            UserName = "johndoe",
            Role = Role.CEO,
            password = "password123"
        };
        
        //Act
        var command = new RegisterUserCommand(request);

        var handler = new RegisterUserCommandHandler(repository);

        var result = await handler.Handle(command, CancellationToken.None);

        var user = await _dbContext.Users.FirstOrDefaultAsync();
        
        var updatedCompany = await _dbContext.Companies.FirstOrDefaultAsync();

        //Assert
        result.Should().Be(Unit.Value);
        user.Should().NotBeNull();
        user.CompanyId.Should().Be(request.CompanyId);
    }
    
    [Fact]
    public async Task UserAlreadyExists_ShouldReturnUserAlreadyExistException()
    {
        //Arrange
        var repository  = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(repository, "testCompany", new List<User>());

        var user = await UserObjectMother.CreateAsync(repository: repository, companyId: company.Id);
        
        var request = new RegisterUserDto()
        {
            CompanyId = user.CompanyId,
            Name = user.Name,
            Surname = user.Surname,
            UserName =  user.UserName,
            Role = user.Role,
            password = "password123"
        };
        
        //Act
        var command = new RegisterUserCommand(request);

        var handler = new RegisterUserCommandHandler(repository);
        
        var exception = await Assert.ThrowsAsync<UserAlreadyExistException>(() => handler.Handle(command, CancellationToken.None));
    }
    
    
    [Fact]
    public async Task CompanyDoesNotExist_ShouldReturnUserCompanyDoesNotExistException()
    {
        //Arrange
        var repository  = new WorkLoggerRepository(_dbContext);
        
        var request = new RegisterUserDto()
        {
            CompanyId = -1,
            Name = "John",
            Surname = "Doe",
            UserName =  "johndoe",
            Role = Role.Employee,
            password = "password123"
        };
        
        //Act
        var command = new RegisterUserCommand(request);

        var handler = new RegisterUserCommandHandler(repository);
        
        await Assert.ThrowsAsync<CompanyDoesNotExistException>(() => handler.Handle(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task InvalidRole_ShouldReturnRoleDoesNotExistException()
    {
        //Arrange
        var repository  = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(repository, "testCompany", new List<User>());
        
        var request = new RegisterUserDto()
        {
            CompanyId = company.Id,
            Name = "John",
            Surname = "Doe",
            UserName = "johndoe",
            Role = (Role)Int32.MaxValue,
            password = "password123"
        };
        
        //Act
        var command = new RegisterUserCommand(request);

        var handler = new RegisterUserCommandHandler(repository);
        
        await Assert.ThrowsAsync<RoleDoesNotExistException>(() => handler.Handle(command, CancellationToken.None));
    }
}