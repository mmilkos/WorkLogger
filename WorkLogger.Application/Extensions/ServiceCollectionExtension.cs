﻿using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WorkLogger.Application._Commands;

namespace WorkLogger.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        const string bearer = "Bearer";
        var authentication = configuration.GetSection("Auth");
        var jwtIssuer = authentication.GetValue<string>("JwtIssuer");
        var jwtKey = authentication.GetValue<string>("JwtKey");

        services.AddMediatR(typeof(RegisterUserRequestCommand));

        services.AddAuthentication(oprion =>
        {
            oprion.DefaultAuthenticateScheme = bearer;
            oprion.DefaultScheme = bearer;
            oprion.DefaultChallengeScheme = bearer;

        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };

        });
    }
}