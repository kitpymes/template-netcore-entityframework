// -----------------------------------------------------------------------
// <copyright file="ShadowPropertiesExtensions.cs" company="Kitpymes">
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
        /// Agrega y configura la propiedad 'TenantId' a las entidades que implementen la interface 'ITenant'.
        /// </summary>
        /// <typeparam name="TTenant">Tipo de dato de la clase tenant.</typeparam>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <param name="enabled">Si se habilita la configuración.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder WithTenantShadowProperty<TTenant>(this ModelBuilder modelBuilder, bool enabled = true)
           where TTenant : IEntityBase
        {
            if (enabled)
            {
                if (AppSession.Tenant?.Enabled == true)
                {
                    var tenantId = AppSession.Tenant?.Id.ToIsNullOrEmptyThrow("AppSession.Tenant?.Id");

                    var entities = modelBuilder.GetEntityTypes<ITenant>();

                    if (entities is not null)
                    {
                        foreach (var entity in entities)
                        {
                            if (typeof(ITenant).IsAssignableFrom(entity))
                            {
                                modelBuilder?.Entity(entity).Property<string?>(ITenant.TenantId).HasDefaultValue(tenantId).ValueGeneratedOnAdd();
                                modelBuilder?.Entity(entity).HasOne(typeof(TTenant)).WithMany().HasForeignKey(ITenant.TenantId).OnDelete(DeleteBehavior.ClientSetNull);
                            }
                        }
                    }
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// Agrega y configura la propiedad 'IsActive' a las entidades que implementen la interface 'IActive'.
        /// </summary>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <param name="enabled">Si se habilita la configuración.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder WithActiveShadowProperty(this ModelBuilder modelBuilder, bool enabled = true)
        {
            if (enabled)
            {
                var entities = modelBuilder.GetEntityTypes<IActive>();

                if (entities is not null)
                {
                    foreach (var entity in entities)
                    {
                        if (typeof(IActive).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<bool>(IActive.IsActive).IsRequired();
                        }
                    }
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// Agrega y configura la propiedad 'IsDelete' a las entidades que implementen la interface 'IDelete'.
        /// </summary>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <param name="enabled">Si se habilita la configuración.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder WithDeleteShadowProperty(this ModelBuilder modelBuilder, bool enabled = true)
        {
            if (enabled)
            {
                var entities = modelBuilder.GetEntityTypes<IDelete>();

                if (entities is not null)
                {
                    foreach (var entity in entities)
                    {
                        if (typeof(IDelete).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<bool>(IDelete.IsDelete).IsRequired();
                        }
                    }
                }
            }

            return modelBuilder;
        }

        /// <summary>
        ///  <para>
        ///     Agrega y configura la propiedad 'CreatedDate' y 'CreatedUserId' a las entidades que implementen la interface 'ICreationAudited'.
        ///  </para>
        ///  <para>
        ///     Agrega y configura la propiedad 'ModifiedDate' y 'ModifiedUserId' a las entidades que implementen la interface 'IModificationAudited'.
        ///  </para>
        ///  <para>
        ///     Agrega y configura la propiedad 'DeletedDate' y 'DeletedUserId' a las entidades que implementen la interface 'IDeletionAudited'.
        ///  </para>
        /// </summary>
        /// <typeparam name="TUser">Tipo de dato de la clase user.</typeparam>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <param name="enabled">Si se habilita la configuración.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder WithAuditedShadowProperties<TUser>(this ModelBuilder modelBuilder, bool enabled = true)
            where TUser : IEntityBase
        {
            if (enabled)
            {
                var entities = modelBuilder.GetEntityTypes<IEntityBase>();

                if (entities is not null)
                {
                    var timestamp = DateTime.UtcNow;

                    foreach (var entity in entities)
                    {
                        if (typeof(ICreationAudited).IsAssignableFrom(entity))
                        {
                            var userId = AppSession.User?.Id.ToIsNullOrEmptyThrow("AppSession.User?.Id");

                            modelBuilder?.Entity(entity).Property<DateTime>(ICreationAudited.CreatedDate).HasDefaultValue(timestamp).ValueGeneratedOnAdd().IsRequired();
                            modelBuilder?.Entity(entity).Property<string>(ICreationAudited.CreatedUserId).HasDefaultValue(userId).ValueGeneratedOnAdd().IsRequired();
                            modelBuilder?.Entity(entity).HasOne(typeof(TUser)).WithMany().HasForeignKey(ICreationAudited.CreatedUserId).OnDelete(DeleteBehavior.ClientSetNull);
                        }

                        if (typeof(IModificationAudited).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<DateTime?>(IModificationAudited.ModifiedDate).HasDefaultValue(timestamp).ValueGeneratedOnUpdate();
                            modelBuilder?.Entity(entity).Property<string?>(IModificationAudited.ModifiedUserId).HasDefaultValue(AppSession.User?.Id).ValueGeneratedOnUpdate();
                            modelBuilder?.Entity(entity).HasOne(typeof(TUser)).WithMany().HasForeignKey(IModificationAudited.ModifiedUserId).OnDelete(DeleteBehavior.ClientSetNull);
                        }

                        if (typeof(IDeletionAudited).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<DateTime?>(IDeletionAudited.DeletedDate).HasDefaultValue(timestamp);
                            modelBuilder?.Entity(entity).Property<string?>(IDeletionAudited.DeletedUserId).HasDefaultValue(AppSession.User?.Id);
                            modelBuilder?.Entity(entity).HasOne(typeof(TUser)).WithMany().HasForeignKey(IDeletionAudited.DeletedUserId).OnDelete(DeleteBehavior.ClientSetNull);
                        }
                    }
                }
            }

            return modelBuilder;
        }
    }
}
