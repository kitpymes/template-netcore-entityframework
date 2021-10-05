// -----------------------------------------------------------------------
// <copyright file="GetEntityTypesExtensions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Kitpymes.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    /*
       Clase de extensión GetEntityTypesExtensions
       Contiene las extensiones para obtener los tipos de entidades definidos en el modelo
    */

    /// <summary>
    /// Clase de extensión <c>GetEntityTypesExtensions</c>.
    /// Contiene las extensiones para obtener los tipos de entidades definidos en el modelo.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las extensiones para obtener los tipos de entidades definidos en el modelo.</para>
    /// </remarks>
    public static class GetEntityTypesExtensions
    {
        /// <summary>
        /// Obtiene todos los tipos de entidades definidos en el modelo.
        ///  <para>
        ///     Por defecto filtra por las entidades que no implementen 'INotMapped'.
        ///  </para>
        /// </summary>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <param name="predicate">Filtrado de entidades.</param>
        /// <returns>IEnumerable{Type}.</returns>
        public static IEnumerable<Type> GetEntityTypes(
            this ModelBuilder modelBuilder,
            Func<IMutableEntityType, bool> predicate)
         => modelBuilder.Model.GetEntityTypes().Where(predicate).Select(e => e.ClrType);

        /// <summary>
        /// Obtiene todos los tipos de entidades definidos en el modelo.
        /// </summary>
        /// <typeparam name="TType">Tipo de interface que debe implementar la entidad.</typeparam>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <returns>IEnumerable{Type}.</returns>
        public static IEnumerable<Type> GetEntityTypes<TType>(this ModelBuilder modelBuilder)
        => modelBuilder.GetEntityTypes(x => !x.ClrType.IsAbstract &&
            x.ClrType.GetInterface(typeof(TType).Name) != null &&
            x.ClrType.GetInterface(typeof(INotMapped).Name) == null);
    }
}
