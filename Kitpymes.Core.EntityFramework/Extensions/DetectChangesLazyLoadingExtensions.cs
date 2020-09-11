// -----------------------------------------------------------------------
// <copyright file="DetectChangesLazyLoadingExtensions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore;

    /*
       Clase de extensión DetectChangesLazyLoadingExtensions
       Contiene las extensiones que detectan los cambios cuando un objeto es modificado en la base de datos
    */

    /// <summary>
    /// Clase de extensión <c>DetectChangesLazyLoadingExtensions</c>.
    /// Contiene las extensiones que detectan los cambios cuando un objeto es modificado en la base de datos.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las extensiones para detectar los cambios de estado de los objetos modificados en la base de datos.</para>
    /// </remarks>
    public static class DetectChangesLazyLoadingExtensions
    {
        /// <summary>
        /// Detecta los cambios en las entidades de la base de datos.
        /// </summary>
        /// <param name="context">Contexto de datos.</param>
        /// <param name="enabled">Si habilitamos la detección de los cambios.</param>
        /// <returns>DbContext | ApplicationException: context es nulo.</returns>
        public static DbContext WithDetectChangesLazyLoading(this DbContext context, bool enabled = true)
        {
            var validContext = context.ToIsNullOrEmptyThrow(nameof(context));

            validContext.ChangeTracker.AutoDetectChangesEnabled = enabled;

            validContext.ChangeTracker.LazyLoadingEnabled = enabled;

            validContext.ChangeTracker.QueryTrackingBehavior = enabled ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking;

            return context;
        }
    }
}
