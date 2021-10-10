﻿// -----------------------------------------------------------------------
// <copyright file="ChangeTrackerExtensions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using Kitpymes.Core.Entities;
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore;

    /*
       Clase de extensión ChangeTrackerExtensions
       Contiene las extensiones que detectan los cambios en la base de datos
    */

    /// <summary>
    /// Clase de extensión <c>ChangeTrackerExtensions</c>.
    /// Contiene las extensiones que detectan los cambios en la base de datos.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las extensiones para detectar los cambios de estado de los objetos en la base de datos.</para>
    /// </remarks>
    public static class ChangeTrackerExtensions
    {
        /// <summary>
        /// Detecta los cambios antes de guardar los datos al contexto.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        /// <param name="enabled">Si se habilita.</param>
        /// <returns>DbContext.</returns>
        public static DbContext WithChangeTracker(this DbContext context, bool enabled = true)
        {
            if (enabled)
            {
                if (context.ChangeTracker.HasChanges())
                {
                    var timestamp = DateTime.UtcNow;

                    foreach (var entry in context.ChangeTracker.Entries())
                    {
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                {
                                    if (entry.Entity is ITenant)
                                    {
                                        if (AppSession.Tenant?.Enabled == true)
                                        {
                                            var tenantId = AppSession.Tenant?.Id.ToIsNullOrEmptyThrow("AppSession.Tenant?.Id");

                                            entry.Property(ITenant.TenantId).CurrentValue = tenantId;
                                        }
                                    }

                                    if (entry.Entity is IActive)
                                    {
                                        entry.Property(IActive.IsActive).CurrentValue = true;
                                    }

                                    if (entry.Entity is IDelete)
                                    {
                                        entry.Property(IDelete.IsDelete).CurrentValue = false;
                                    }

                                    if (entry.Entity is ICreationAudited)
                                    {
                                        entry.Property(ICreationAudited.CreatedDate).CurrentValue = timestamp;

                                        entry.Property(ICreationAudited.CreatedUserId).CurrentValue = AppSession.User?.Id;
                                    }

                                    break;
                                }

                            case EntityState.Deleted:
                                {
                                    if (entry.Entity is IActive)
                                    {
                                        entry.Property(IActive.IsActive).CurrentValue = false;
                                    }

                                    if (entry.Entity is IDelete)
                                    {
                                        entry.Property(IDelete.IsDelete).CurrentValue = true;
                                    }

                                    if (entry.Entity is IDeletionAudited)
                                    {
                                        entry.Property(IDeletionAudited.DeletedDate).CurrentValue = timestamp;

                                        entry.Property(IDeletionAudited.DeletedUserId).CurrentValue = AppSession.User?.Id;
                                    }

                                    break;
                                }

                            case EntityState.Modified when entry.Entity is IModificationAudited:

                                entry.Property(IModificationAudited.ModifiedDate).CurrentValue = timestamp;

                                entry.Property(IModificationAudited.ModifiedUserId).CurrentValue = AppSession.User?.Id;

                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            return context;
        }
    }
}
