// -----------------------------------------------------------------------
// <copyright file="DatabaseSettings.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    /*
       Clase de configuración DatabaseSettings
       Contiene la configuración de la base de datos
    */

    /// <summary>
    /// Clase de configuración <c>DatabaseSettings</c>.
    /// Contiene la configuración de la base de datos.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se puede agregar la configuración necesaria para la conexión a la base de datos.</para>
    /// </remarks>
    public class DatabaseSettings
    {
        /// <summary>
        /// Obtiene o establece un valor del tipo de base de datos a utilizar.
        /// </summary>
        public string? DbProvider { get; set; }

        /// <summary>
        /// Obtiene o establece un valor para la configuración de SqlServer.
        /// </summary>
        public SqlServerSettings? SqlServerSettings { get; set; }
    }
}
