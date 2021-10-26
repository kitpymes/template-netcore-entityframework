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
        /// Configura la propiedad 'TenantId' a las entidades que implementen la interface 'ITenant'.
        /// </summary>
        /// <typeparam name="TTenant">Tipo de dato de la clase tenant.</typeparam>
        /// <typeparam name="TTenantId">Tipo de dato del id de tenant.</typeparam>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <param name="tenantId">Id del tenant.</param>
        /// <param name="enabled">Si se habilita la configuración.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder WithTenantShadowProperty<TTenant, TTenantId>(
            this ModelBuilder modelBuilder,
            TTenantId tenantId,
            bool enabled = true)
           where TTenant : IEntityBase
        {
            if (enabled)
            {
                var entities = modelBuilder.GetEntityTypes<ITenant>();

                if (entities is not null)
                {
                    foreach (var entity in entities)
                    {
                        if (typeof(ITenant).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property(typeof(TTenantId), ITenant.TenantId).HasDefaultValue(tenantId).ValueGeneratedOnAdd();
                            modelBuilder?.Entity(entity).HasOne(typeof(TTenant)).WithMany().HasForeignKey(ITenant.TenantId).OnDelete(DeleteBehavior.ClientSetNull);
                        }
                    }
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// Configura la propiedad 'IsActive' a las entidades que implementen la interface 'IActive'.
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
        /// Configura la propiedad 'IsDelete' a las entidades que implementen la interface 'IDelete'.
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
        /// Configura la propiedad 'RowVersion' a las entidades que implementen la interface 'IRowVersion'.
        /// </summary>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <param name="enabled">Si se habilita la configuración.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder WithRowVersionShadowProperty(this ModelBuilder modelBuilder, bool enabled = true)
        {
            if (enabled)
            {
                var entities = modelBuilder.GetEntityTypes<IRowVersion>();

                if (entities is not null)
                {
                    foreach (var entity in entities)
                    {
                        if (typeof(IRowVersion).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<long>(IRowVersion.RowVersion).IsRequired();
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
        /// <typeparam name="TUserId">Tipo de dato del id de user.</typeparam>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <param name="enabled">Si se habilita la configuración.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder WithAuditedShadowProperties<TUser, TUserId>(this ModelBuilder modelBuilder, bool enabled = true)
            where TUser : IEntityBase
        {
            if (enabled)
            {
                var entities = modelBuilder.GetEntityTypes<IEntityBase>();

                if (entities is not null)
                {
                    var userIdType = typeof(TUserId);

                    foreach (var entity in entities)
                    {
                        if (typeof(ICreationAudited).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<DateTime>(ICreationAudited.CreatedDate);
                            modelBuilder?.Entity(entity).Property(userIdType, ICreationAudited.CreatedUserId);
                        }

                        if (typeof(IModificationAudited).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<DateTime?>(IModificationAudited.ModifiedDate);
                            modelBuilder?.Entity(entity).Property(userIdType, IModificationAudited.ModifiedUserId);
                        }

                        if (typeof(IDeletionAudited).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<DateTime?>(IDeletionAudited.DeletedDate);
                            modelBuilder?.Entity(entity).Property(userIdType, IDeletionAudited.DeletedUserId);
                        }
                    }
                }
            }

            return modelBuilder;
        }
    }
}
