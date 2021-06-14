// -----------------------------------------------------------------------
// <copyright file="SqlServerSettings.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    /*
       Clase de configuración SqlServerSettings
       Contiene la configuración de la base de datos de sql server
    */

    /// <summary>
    /// Clase de configuración <c>SqlServerSettings</c>.
    /// Contiene la configuración de la base de datos de sql server.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se puede agregar la configuración necesaria para la conexión a la base de datos sql server.</para>
    /// </remarks>
    public class SqlServerSettings : EntityFrameworkSettings
    {
        /// <summary>
        /// Si se habilita el log de errores.
        /// </summary>
        public const bool DefaultIsLogErrorsEnabled = false;

        private bool isLogErrorsEnabled = DefaultIsLogErrorsEnabled;

        /// <summary>
        /// Obtiene o establece un valor de la configuración del contexto.
        /// </summary>
        public Action<SqlServerDbContextOptionsBuilder>? SqlServerDbContextOptions { get; set; }

        /// <summary>
        /// Obtiene o establece la conexión de la base de datos.
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si se habilita el log de errores.
        /// </summary>
        public bool? IsLogErrorsEnabled
        {
            get => isLogErrorsEnabled;
            set
            {
                if (value.HasValue)
                {
                    isLogErrorsEnabled = value.Value;
                }
            }
        }
    }
}
