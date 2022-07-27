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
        /// <list type="bullet">
        ///     <para>
        ///         Descripción de las Shadow Properties que se agregan a la entidad que implementa alguna de estas interfaces.
        ///     </para>
        ///     <item>
        ///         <term>ICreationAudited</term>
        ///         <para>Agrega y configura la propiedad "ICreationAudited.CreatedDate" | Type: DateTime | Required: true.</para>
        ///         <para>Agrega y configura la propiedad "ICreationAudited.CreatedUserId" | Type: TUserId | Required: true.</para>
        ///     </item>
        ///     <item>
        ///         <term>IModificationAudited</term>
        ///         <para>Agrega y configura la propiedad "IModificationAudited.ModifiedDate" | Type: DateTime? | Required: false.</para>
        ///         <para>Agrega y configura la propiedad "IModificationAudited.ModifiedUserId" | Type: TUserId | Required: false.</para>
        ///     </item>
        ///     <item>
        ///         <term>IDeletionAudited</term>
        ///         <para>Agrega y configura la propiedad "IDeletionAudited.DeletedDate" | Type: DateTime? | Required: false.</para>
        ///         <para>Agrega y configura la propiedad "IDeletionAudited.DeletedUserId" | Type: TUserId | Required: false.</para>
        ///     </item>
        ///     <item>
        ///         <term>IActive</term>
        ///         <description>Agrega y configura la propiedad "IActive.IsActive" | Type: bool | Required: true.</description>
        ///     </item>
        ///     <item>
        ///         <term>IDefault</term>
        ///         <description>Agrega y configura la propiedad "IDefault.IsDefault" | Type: bool | Required: true | DefaultValue: false.</description>
        ///     </item>
        ///     <item>
        ///         <term>IDelete</term>
        ///         <description>Agrega y configura la propiedad "IDelete.IsDelete" | Type: bool | Required: true.</description>
        ///     </item>
        ///     <item>
        ///         <term>IStatic</term>
        ///         <description>Agrega y configura la propiedad "IStatic.IsStatic" | Type: bool | Required: true | DefaultValue: false.</description>
        ///     </item>
        ///     <item>
        ///         <term>INotVisible</term>
        ///         <description>Agrega y configura la propiedad "INotVisible.IsNotVisible" | Type: bool | Required: true | DefaultValue: false.</description>
        ///     </item>
        ///     <item>
        ///         <term>IRowVersion</term>
        ///         <description>Agrega y configura la propiedad "IRowVersion.RowVersion" | Type: long | Required: true.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <typeparam name="TUser">Tipo de dato de la clase user.</typeparam>
        /// <typeparam name="TUserId">Tipo de dato del id de user.</typeparam>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <param name="enabled">Si se habilita la configuración.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder WithShadowProperties<TUser, TUserId>(this ModelBuilder modelBuilder, bool enabled = true)
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
                            modelBuilder?.Entity(entity).Property<DateTime>(ICreationAudited.CreatedDate).IsRequired();
                            modelBuilder?.Entity(entity).Property(userIdType, ICreationAudited.CreatedUserId).IsRequired();
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

                        if (typeof(IActive).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<bool>(IActive.IsActive).IsRequired();
                        }

                        if (typeof(IDefault).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<bool>(IDefault.IsDefault).HasDefaultValue(false).IsRequired();
                        }

                        if (typeof(IDelete).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<bool>(IDelete.IsDelete).IsRequired();
                        }

                        if (typeof(IStatic).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<bool>(IStatic.IsStatic).HasDefaultValue(false).IsRequired();
                        }

                        if (typeof(INotVisible).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<bool>(INotVisible.IsNotVisible).HasDefaultValue(false).IsRequired();
                        }

                        if (typeof(IRowVersion).IsAssignableFrom(entity))
                        {
                            modelBuilder?.Entity(entity).Property<long>(IRowVersion.RowVersion).IsRequired();
                        }
                    }
                }
            }

            return modelBuilder;
        }
    }
}
