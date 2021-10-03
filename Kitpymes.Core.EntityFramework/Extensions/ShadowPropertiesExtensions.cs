// -----------------------------------------------------------------------
// <copyright file="ShadowPropertiesExtensions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using System.Linq;
    using Kitpymes.Core.Entities;
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore;

    /*
       Clase de extensión ShadowPropertiesExtensions
       Contiene las extensiones para agregar propiedades al modelo
    */

    /// <summary>
    /// Clase de extensión <c>ShadowPropertiesExtensions</c>.
    /// Contiene las extensiones para agregar propiedades al modelo.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las extensiones para agregar propiedades al modelo de entidades.</para>
    /// </remarks>
    public static class ShadowPropertiesExtensions
    {
        /// <summary>
        /// Agrega propiedades al model de entidades.
        /// </summary>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <returns>ModelBuilder | ApplicationException: modelBuilder es nulo.</returns>
        public static ModelBuilder WithShadowProperties(this ModelBuilder modelBuilder)
        {
            var entities = modelBuilder.ToIsNullOrEmptyThrow(nameof(modelBuilder)).Model.GetEntityTypes()
                .Where(
                    x => !x.ClrType.IsAbstract &&
                    x.ClrType.GetInterface(typeof(IEntityBase).Name) != null &&
                    x.ClrType.GetInterface(typeof(INotMapped).Name) == null)
                .Select(e => e.ClrType);

            if (entities != null)
            {
                var userType = entities.FirstOrDefault(x => x.Name == nameof(AppSession.User));

                var tenantType = entities.FirstOrDefault(x => x.Name == nameof(AppSession.Tenant));

                foreach (var entity in entities)
                {
                    if (typeof(ITenant).IsAssignableFrom(entity) && AppSession.Tenant != null && AppSession.Tenant.Enabled.HasValue && AppSession.Tenant.Enabled.Value)
                    {
                        modelBuilder?.Entity(entity).Property<string?>(ITenant.TenantId).HasDefaultValue(AppSession.Tenant.Id).ValueGeneratedOnAdd();

                        modelBuilder?.Entity(entity).HasOne(tenantType).WithMany().HasForeignKey(ITenant.TenantId).OnDelete(DeleteBehavior.Restrict);
                    }

                    if (typeof(IActive).IsAssignableFrom(entity))
                    {
                        modelBuilder?.Entity(entity).Property<bool>(IActive.IsActive)
                            .HasDefaultValue(true)
                            .IsRequired();
                    }

                    if (typeof(IDelete).IsAssignableFrom(entity))
                    {
                        modelBuilder?.Entity(entity).Property<bool>(IDelete.IsDelete)
                            .HasDefaultValue(false)
                            .IsRequired();
                    }

                    if (typeof(ICreationAudited).IsAssignableFrom(entity))
                    {
                        modelBuilder?.Entity(entity).Property<DateTime>(ICreationAudited.CreatedDate).HasDefaultValue(DateTime.Now).ValueGeneratedOnAdd();

                        modelBuilder?.Entity(entity).Property<string?>(ICreationAudited.CreatedUserId).HasDefaultValue(AppSession.User?.Id).ValueGeneratedOnAdd();

                        modelBuilder?.Entity(entity).HasOne(userType).WithMany().HasForeignKey(ICreationAudited.CreatedUserId).OnDelete(DeleteBehavior.Restrict);
                    }

                    if (typeof(IModificationAudited).IsAssignableFrom(entity))
                    {
                        modelBuilder?.Entity(entity).Property<DateTime?>(IModificationAudited.ModifiedDate).HasDefaultValue(DateTime.Now).ValueGeneratedOnUpdate();

                        modelBuilder?.Entity(entity).Property<string?>(IModificationAudited.ModifiedUserId).HasDefaultValue(AppSession.User?.Id).ValueGeneratedOnUpdate();

                        modelBuilder?.Entity(entity).HasOne(userType).WithMany().HasForeignKey(IModificationAudited.ModifiedUserId).OnDelete(DeleteBehavior.Restrict);
                    }

                    if (typeof(IDeletionAudited).IsAssignableFrom(entity))
                    {
                        modelBuilder?.Entity(entity).Property<DateTime?>(IDeletionAudited.DeletedDate).HasDefaultValue(DateTime.Now).ValueGeneratedNever();

                        modelBuilder?.Entity(entity).Property<string?>(IDeletionAudited.DeletedUserId).HasDefaultValue(AppSession.User?.Id);

                        modelBuilder?.Entity(entity).HasOne(userType).WithMany().HasForeignKey(IDeletionAudited.DeletedUserId).OnDelete(DeleteBehavior.Restrict);
                    }
                }
            }

            return modelBuilder;
        }
    }
}
