// -----------------------------------------------------------------------
// <copyright file="SqlServerOptions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
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
        /// Conexión de la base de datos.
        /// </summary>
        /// <param name="connectionString">String de conexión.</param>
        /// <returns>SqlServerOptions.</returns>
        public SqlServerOptions WithConnectionString(string connectionString)
        {
            SqlServerSettings.Connection = connectionString;

            return this;
        }

        /// <summary>
        /// Configuración del contexto de sql server.
        /// </summary>
        /// <param name="sqlServerDbContextOptions">Opciones de sql server.</param>
        /// <returns>SqlServerOptions.</returns>
        public SqlServerOptions WithSqlServerDbContextOptions(Action<SqlServerDbContextOptionsBuilder> sqlServerDbContextOptions)
        {
            SqlServerSettings.SqlServerOptions = sqlServerDbContextOptions;

            return this;
        }

        /// <summary>
        /// Indica si se habilita la creación de la base de datos si no existe.
        /// No utiliza migraciones para crear la base de datos y, por lo tanto, no se puede actualizar posteriormente mediante migraciones.
        /// </summary>
        /// <param name="enabled">Si se habilita o no.</param>
        /// <returns>SqlServerOptions.</returns>
        public new SqlServerOptions WithCreate(bool enabled = true)
        {
            base.WithCreate(enabled);

            return this;
        }

        /// <summary>
        /// Indica si se habilitan las migraciones.
        /// Creará la base de datos si aún no existe.
        /// Es mutuamente excluyente con IsEnsuredDeletedEnabled./// Indica si se habilita las transacciones.
        /// </summary>
        /// <param name="enabled">Si se habilita o no.</param>
        /// <returns>SqlServerOptions.</returns>
        public new SqlServerOptions WithMigrate(bool enabled = true)
        {
            base.WithMigrate(enabled);

            return this;
        }

        /// <summary>
        /// Indica si se habilita la eliminación de la base de datos si esta existe.
        /// </summary>
        /// <param name="enabled">Si se habilita o no.</param>
        /// <returns>SqlServerOptions.</returns>
        public new SqlServerOptions WithDelete(bool enabled = true)
        {
            base.WithDelete(enabled);

            return this;
        }

        /// <summary>
        /// Indica si se habilita el log de errores.
        /// </summary>
        /// <param name="enabled">Si se habilita o no.</param>
        /// <returns>SqlServerOptions.</returns>
        public new SqlServerOptions WithLogErrors(bool enabled = SqlServerSettings.DefaultLogErrors)
        {
            base.WithLogErrors(enabled);

            return this;
        }
    }
}
