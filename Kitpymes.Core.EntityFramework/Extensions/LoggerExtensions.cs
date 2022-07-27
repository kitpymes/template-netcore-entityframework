// -----------------------------------------------------------------------
// <copyright file="LoggerExtensions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
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
        /// <summary>
        /// Logea en la consola los cambios que se producen en la DB.
        /// </summary>
        /// <param name="context">Contexto.</param>
        /// <param name="enabled">Si esta habilitado.</param>
        /// <returns>DbContext.</returns>
        public static DbContext WithConsoleLogSaveChanges(this DbContext context, bool enabled = true)
        {
            if (enabled)
            {
                if (context.ChangeTracker.HasChanges())
                {
                    Console.WriteLine(context.ChangeTracker.DebugView.LongView);
                }
            }

            return context;
        }

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
