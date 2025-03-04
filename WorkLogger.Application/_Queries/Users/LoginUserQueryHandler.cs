using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WorkLogger.Domain;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Queries.Users;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, OperationResult<UserLoginResponseDto>>
{
    private IWorkLoggerRepository _repository;
    private IConfiguration _configuration;
    private PasswordHasher<User> _hasher;
    
    public LoginUserQueryHandler(IWorkLoggerRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
        _hasher = new PasswordHasher<User>();
    }
    
    public async Task<OperationResult<UserLoginResponseDto>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var dto = request.RequestDto;
        var operationResult = new OperationResult<UserLoginResponseDto>();
        
        var user = await _repository.FindEntityByConditionAsync<User>(user => user.UserName == dto.UserName);
        
        var correctCredentials = CheckCredentials(user, dto);
        
        if (correctCredentials == false)
        {
            operationResult.AddError(Errors.InvalidCredentials);
            operationResult.ErrorType = ErrorTypesEnum.Unauthorized;
            return operationResult;
        }

        var token = GenerateJwtToken(user);

        var userDto = new UserLoginResponseDto()
        {
            JwtToken = token
        };

        operationResult.Data = userDto;

        return operationResult;
    }

    private bool CheckCredentials(User? user, LoginUserRequestDto loginRequestDto)
    {
        if (user == null) return false;

        var result = _hasher.VerifyHashedPassword(user: null, hashedPassword: user.PasswordHash,
            providedPassword: loginRequestDto.Password);
        var isValid = result == PasswordVerificationResult.Success;
        return isValid;
    }

    private string GenerateJwtToken(User user)
    {
        var authentication = _configuration.GetSection("Auth");
        var jwtIssuer = authentication.GetValue<string>("JwtIssuer");
        var jwtKey = authentication.GetValue<string>("JwtKey");
        var jwtExpireHours = authentication.GetValue<int>("JwtExpireHours");

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("CompanyId", user.CompanyId.ToString()),
            new Claim(ClaimTypes.GivenName, user.UserName),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddHours(jwtExpireHours);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtIssuer,
            claims: claims,
            expires: expires,
            signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}