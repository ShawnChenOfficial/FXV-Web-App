using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace FXV.CustomAuth
{
    public static class CustomJwtBearerExtensions
    {
        // create a AddCustomJwtBearer with customized purpose to replace the addJwtBearer
        public static AuthenticationBuilder AddCustomJwtBearer(this AuthenticationBuilder builder, Action<JwtBearerOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());
            return builder.AddScheme<JwtBearerOptions, CustomJwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, configureOptions);
        }
    }
}
