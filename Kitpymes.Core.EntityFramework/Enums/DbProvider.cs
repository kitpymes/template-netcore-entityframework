// -----------------------------------------------------------------------
// <copyright file="DbProvider.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    /// <summary>
    /// Enumeración de tipo de proveedor de base de datos.
    /// </summary>
    public enum DbProvider
    {
        /// <summary>
        /// Poveedor memoria.
        /// </summary>
        Memory = 1,

        /// <summary>
        /// Poveedor SqlServer.
        /// </summary>
        SqlServer,

        /// <summary>
        /// Poveedor SQLite.
        /// </summary>
        SQLite,
    }
}
