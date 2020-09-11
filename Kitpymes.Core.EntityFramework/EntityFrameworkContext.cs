// -----------------------------------------------------------------------
// <copyright file="EntityFrameworkContext.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using Microsoft.EntityFrameworkCore;

    /*
       Clase de extensión EntityFrameworkContext
       Contiene el contexto base para entity framework
    */

    /// <summary>
    /// Clase de extensión <c>EntityFrameworkContext</c>.
    /// Contiene el contexto base para entity framework.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las acciones comunes para el contexto de entity framework.</para>
    /// </remarks>
    public abstract class EntityFrameworkContext : DbContext
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="EntityFrameworkContext"/>.
        /// </summary>
        /// <param name="options">Configuración del contexto.</param>
        protected EntityFrameworkContext(DbContextOptions options)
            : base(options)
        { }
    }
}
