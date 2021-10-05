// -----------------------------------------------------------------------
// <copyright file="QueryableExtensions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore;

    /*
        Clase de extensión QueryableExtensions
        Contiene las extensiones para consultar la base de datos
    */

    /// <summary>
    /// Clase de extensión <c>QueryableExtensions</c>.
    /// Contiene las extensiones para consultar la base de datos.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las extensiones para consultar la base de datos.</para>
    /// </remarks>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Hace que un contexto sea mas rápido para obtener información de la base de datos.
        /// </summary>
        /// <typeparam name="T">Tipo de entidad a consultar.</typeparam>
        /// <param name="context">Contexto de datos.</param>
        /// <returns>IQueryable{T} | ApplicationException: context es nulo.</returns>
        public static IQueryable<T> ToQueryable<T>(this DbContext context)
            where T : class
        => context.WithOptimizedContext(false).Set<T>();

        /// <summary>
        /// Incluye entidades asociadas a la consulta.
        /// </summary>
        /// <typeparam name="T">Tipo de entidad a consultar.</typeparam>
        /// <param name="queryable">Consulta del contexto.</param>
        /// <param name="includes">Las entidades a incluir.</param>
        /// <returns>IQueryable{T}.</returns>
        public static IQueryable<T> ToInclude<T>(this IQueryable<T> queryable, Expression<Func<T, object>>[] includes)
            where T : class
        {
            includes?.ToList().ForEach(include => queryable = queryable.Include(include));

            return queryable;
        }
    }
}
