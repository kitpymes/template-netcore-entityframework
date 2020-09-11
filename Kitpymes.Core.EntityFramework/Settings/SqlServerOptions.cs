// -----------------------------------------------------------------------
// <copyright file="SqlServerOptions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    /*
       Clase de configuración SqlServerOptions
       Contiene las propiedades para la configuración de sql server
    */

    /// <summary>
    /// Clase de configuración <c>SqlServerOptions</c>.
    /// Contiene las propiedades para la configuración de sql server.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las propiedades para la configuración de sql server.</para>
    /// </remarks>
    public class SqlServerOptions : EntityFrameworkOptions
    {
        /// <summary>
        /// Obtiene la configuración de sql server.
        /// </summary>
        public SqlServerSettings SqlServerSettings { get; private set; } = new SqlServerSettings();

        /// <summary>
        /// Configuración del contexto de sql server.
        /// </summary>
        /// <param name="sqlServerDbContextOptions">Opciones de sql server.</param>
        /// <returns>SqlServerOptions.</returns>
        public SqlServerOptions WithDbContextOptions(Action<SqlServerDbContextOptionsBuilder> sqlServerDbContextOptions)
        {
            SqlServerSettings.SqlServerDbContextOptions = sqlServerDbContextOptions;

            return this;
        }

        /// <summary>
        /// Indica si se habilita el log de errores.
        /// </summary>
        /// <param name="enabled">Si se habilita o no.</param>
        /// <returns>SqlServerOptions.</returns>
        public SqlServerOptions WithLogErrors(bool enabled = true)
        {
            SqlServerSettings.IsLogErrorsEnabled = enabled;

            return this;
        }

        /// <summary>
        /// Conexión de la base de datos.
        /// </summary>
        /// <param name="connectionString">String de conexión.</param>
        /// <returns>SqlServerOptions | ApplicationException: si connectionString es nulo o vacio.</returns>
        public SqlServerOptions WithConnectionString(string connectionString)
        {
            SqlServerSettings.ConnectionString = connectionString.ToIsNullOrEmptyThrow(nameof(connectionString));

            return this;
        }
    }
}
