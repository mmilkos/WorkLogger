using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WorkLogger.Domain;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Queries.Users;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, OperationResult<UserResponseDto>>
{
    private IWorkLoggerRepository _repository;
    private IConfiguration _configuration;
    
    public LoginUserQueryHandler(IWorkLoggerRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }
    
    public async Task<OperationResult<UserResponseDto>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var dto = request.RequestDto;
        var operationResult = new OperationResult<UserResponseDto>();
        
        var user = await _repository.FindUserByUsernameAsync(dto.UserName);
        
        var correctCredentials = CheckCredentials(user, dto);
        
        if (correctCredentials == false)
        {
            operationResult.AddError(Errors.InvalidCredentials);
            operationResult.ErrorType = ErrorTypesEnum.Unauthorized;
            return operationResult;
        }

        var token = GenerateJwtToken(user);

        var userDto = new UserResponseDto()
        {
            JwtToken = token
        };

        operationResult.Data = userDto;

        return operationResult;
    }

    private bool CheckCredentials(User? user, LoginUserRequestDto loginRequestDto)
    {
        if (user == null) return false;
        
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequestDto.Password));
        
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return false;
            }
        }

        return true;
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