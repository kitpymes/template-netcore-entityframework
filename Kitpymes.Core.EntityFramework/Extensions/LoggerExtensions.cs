// -----------------------------------------------------------------------
// <copyright file="LoggerExtensions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /*
       Clase de extensión LoggerExtensions
       Contiene las extensiones para aplicar el logeo de errores
    */

    /// <summary>
    /// Clase de extensión <c>LoggerExtensions</c>.
    /// Contiene las extensiones para aplicar el logeo de errores.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las extensiones para aplicar el logeo de errores.</para>
    /// </remarks>
    public static class LoggerExtensions
    {
        internal static DbContextOptionsBuilder WithLogger(
            this DbContextOptionsBuilder options,
            IServiceCollection services,
            bool enabled = true)
        {
            if (enabled)
            {
                var loggerFactory = services.AddLogging(options => options
                    .AddConsole()
                    .AddFilter((category, level)
                        => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Debug))
                    .ToService<ILoggerFactory>();

                options
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .ConfigureWarnings(warnings
                        => warnings.Log((RelationalEventId.CommandExecuting, LogLevel.Information)));
            }

            return options;
        }
    }
}
