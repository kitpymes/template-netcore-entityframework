// -----------------------------------------------------------------------
// <copyright file="GlobalFiltersExtensions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Kitpymes.Core.Entities;
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore;
    using Remotion.Linq.Parsing.ExpressionVisitors;

    /*
       Clase de extensión GlobalFiltersExtensions
       Contiene las extensiones para aplicar filtros globales
    */

    /// <summary>
    /// Clase de extensión <c>GlobalFiltersExtensions</c>.
    /// Contiene las extensiones para aplicar filtros globales.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las extensiones para aplicar filtros en las entidades.</para>
    /// </remarks>
    public static class GlobalFiltersExtensions
    {
        /// <summary>
        /// Para filtrar por el id del inquilino.
        /// </summary>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <returns>ModelBuilder | ApplicationException: modelBuilder es nulo.</returns>
        public static ModelBuilder WithTenantFilter(this ModelBuilder modelBuilder)
        {
            modelBuilder.ToIsNullOrEmptyThrow(nameof(modelBuilder));

            if (AppSession.Tenant != null && AppSession.Tenant.Enabled.HasValue && AppSession.Tenant.Enabled.Value)
            {
                modelBuilder.WithFilter<ITenant>(property
                    => EF.Property<string>(property, ITenant.TenantId) == AppSession.Tenant.Id);
            }

            return modelBuilder;
        }

        /// <summary>
        /// Para filtrar por los objetos activos.
        /// </summary>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <returns>ModelBuilder | ApplicationException: modelBuilder es nulo.</returns>
        public static ModelBuilder WithActiveFilter(this ModelBuilder modelBuilder)
        {
            modelBuilder.ToIsNullOrEmptyThrow(nameof(modelBuilder));

            modelBuilder.WithFilter<IActive>(property
                     => EF.Property<bool>(property, IActive.IsActive) == true);

            return modelBuilder;
        }

        /// <summary>
        /// Para filtrar por los objetos no eliminados.
        /// </summary>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <returns>ModelBuilder | ApplicationException: modelBuilder es nulo.</returns>
        public static ModelBuilder WithNotDeletedFilter(this ModelBuilder modelBuilder)
        {
            modelBuilder.ToIsNullOrEmptyThrow(nameof(modelBuilder));

            modelBuilder.WithFilter<IDelete>(property
                     => EF.Property<bool>(property, IDelete.IsDelete) == false);

            return modelBuilder;
        }

        /// <summary>
        /// Agrega un filtro global.
        /// </summary>
        /// <typeparam name="TInterface">Tipo de interface.</typeparam>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <param name="expression">Expresión a validar.</param>
        public static void WithFilter<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
        {
            var entities = modelBuilder.ToIsNullOrEmptyThrow(nameof(modelBuilder)).Model
                .GetEntityTypes()
                .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
                .Select(e => e.ClrType);

            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    var newParam = Expression.Parameter(entity);

                    var newbody = ReplacingExpressionVisitor.Replace(expression?.Parameters.Single(), newParam, expression?.Body);

                    modelBuilder?.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
                }
            }
        }
    }
}
