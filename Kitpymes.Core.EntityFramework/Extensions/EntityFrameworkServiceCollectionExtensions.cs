// -----------------------------------------------------------------------
// <copyright file="EntityFrameworkServiceCollectionExtensions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    /*
        Clase de extensión EntityFrameworkServiceCollectionExtensions
        Contiene las extensiones de los servicios de entity framework
    */

    /// <summary>
    /// Clase de extensión <c>EntityFrameworkServiceCollectionExtensions</c>.
    /// Contiene las extensiones de los servicios de entity framework.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las extensiones de los servicios para entity framework.</para>
    /// </remarks>
    public static class EntityFrameworkServiceCollectionExtensions
    {
        /// <summary>
        /// Carga un contexto de datos.
        /// </summary>
        /// <typeparam name="TDbContext">Tipo de contexto.</typeparam>
        /// <param name="services">Collección de servicios.</param>
        /// <param name="sqlServerOptions">Configuración de sql server y entity framework.</param>
        /// <returns>TDbContext | ApplicationException.</returns>
        public static TDbContext LoadSqlServer<TDbContext>(
            this IServiceCollection services,
            Action<SqlServerOptions> sqlServerOptions)
                where TDbContext : EntityFrameworkContext
        {
            var settings = sqlServerOptions.ToConfigureOrDefault().SqlServerSettings;

            return services.LoadSqlServer<TDbContext>(settings);
        }

        /// <summary>
        /// Carga un contexto de datos.
        /// </summary>
        /// <typeparam name="TDbContext">Tipo de contexto.</typeparam>
        /// <param name="services">Collección de servicios.</param>
        /// <param name="sqlServerSettings">Configuración de sql server y entity framework.</param>
        /// <returns>TDbContext | ApplicationException.</returns>
        public static TDbContext LoadSqlServer<TDbContext>(
            this IServiceCollection services,
            SqlServerSettings sqlServerSettings)
                where TDbContext : EntityFrameworkContext
        {
            var settings = sqlServerSettings.ToIsNullOrEmptyThrow(nameof(sqlServerSettings));

            var connectionString = settings.ConnectionString
                .ToIsNullOrEmptyThrow(nameof(settings.ConnectionString));

            settings.DbContextOptionsBuilder = dbContextOptions => dbContextOptions
                     .UseSqlServer(connectionString, settings.SqlServerDbContextOptions)
                     .WithLogger(services, settings.IsLogErrorsEnabled == true);

            return services.LoadContext<TDbContext>(settings);
        }

        /// <summary>
        /// Carga un contexto de datos.
        /// </summary>
        /// <typeparam name="TDbContext">Tipo de contexto.</typeparam>
        /// <param name="services">Collección de servicios.</param>
        /// <param name="inMemoryDbContextOptions">Configuración del contexto.</param>
        /// <param name="databaseName">Nombre de la base de datos.</param>
        /// <returns>TDbContext | ApplicationException.</returns>
        public static TDbContext LoadInMemoryDatabase<TDbContext>(
            this IServiceCollection services,
            Action<InMemoryDbContextOptionsBuilder>? inMemoryDbContextOptions = null,
            string? databaseName = null)
                where TDbContext : EntityFrameworkContext
        {
            var settings = new EntityFrameworkSettings
            {
                DbContextOptionsBuilder = dbContextOptions
                    => dbContextOptions.UseInMemoryDatabase(databaseName ?? typeof(TDbContext).Name, inMemoryDbContextOptions),
            };

            return services.LoadContext<TDbContext>(settings);
        }

        /// <summary>
        /// Carga un contexto de datos.
        /// </summary>
        /// <typeparam name="TDbContext">Tipo de contexto.</typeparam>
        /// <param name="services">Collección de servicios.</param>
        /// <param name="entityFrameworkOptions">Configuración de entity framework.</param>
        /// <returns>TDbContext | ApplicationException.</returns>
        public static TDbContext LoadContext<TDbContext>(
            this IServiceCollection services,
            Action<EntityFrameworkOptions> entityFrameworkOptions)
                where TDbContext : DbContext
        {
            var settings = entityFrameworkOptions.ToConfigureOrDefault().EntityFrameworkSettings;

            return services.LoadContext<TDbContext>(settings);
        }

        /// <summary>
        /// Carga un contexto de datos.
        /// </summary>
        /// <typeparam name="TDbContext">Tipo de contexto.</typeparam>
        /// <param name="services">Collección de servicios.</param>
        /// <param name="entityFrameworkSettings">Configuración de entity framework.</param>
        /// <returns>TDbContext | ApplicationException.</returns>
        public static TDbContext LoadContext<TDbContext>(
           this IServiceCollection services,
           EntityFrameworkSettings entityFrameworkSettings)
               where TDbContext : DbContext
        {
            var settings = entityFrameworkSettings.ToIsNullOrEmptyThrow(nameof(entityFrameworkSettings));

            services.ToServiceMatchingInterface(new[] { typeof(IEntityFrameworkUnitOfWork).Assembly });

            var context = services
                .AddDbContextPool<TDbContext>(settings.DbContextOptionsBuilder)
                .ToService<TDbContext>();

            if (settings.IsEnsuredDeletedEnabled == true)
            {
                context.Database.EnsureDeleted();
            }

            if (settings.IsEnsuredCreatedEnabled == true)
            {
                context.Database.EnsureCreated();
            }
            else if (settings.IsMigrateEnabled == true)
            {
                context.Database.Migrate();
            }

            return context;
        }
    }
}
