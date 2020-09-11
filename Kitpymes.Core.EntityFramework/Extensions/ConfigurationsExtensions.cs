// -----------------------------------------------------------------------
// <copyright file="ConfigurationsExtensions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore;

    /*
        Clase de extensión ConfigurationsExtensions
        Contiene las extensiones para aplicar la configuración de las entidades
    */

    /// <summary>
    /// Clase de extensión <c>ConfigurationsExtensions</c>.
    /// Contiene las extensiones para aplicar la configuración de las entidades.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las extensiones para configurar el modelo de entidades.</para>
    /// </remarks>
    public static class ConfigurationsExtensions
    {
        /// <summary>
        /// Aplica la configuración del modelo de entidades.
        /// </summary>
        /// <param name="modelBuilder">Modelo de entidades.</param>
        /// <param name="enabled">Si se habilita la configuración.</param>
        /// <returns>ModelBuilder | ApplicationException: modelBuilder es nulo.</returns>
        public static ModelBuilder WithEntitiesConfigurations(this ModelBuilder modelBuilder, bool enabled = true)
        {
            if (enabled)
            {
                modelBuilder.ToIsNullOrEmptyThrow(nameof(modelBuilder));

                static bool Expression(Type type)
                    => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>);

                var types = Assembly.GetCallingAssembly().GetTypes()
                    .Where(type => type.GetInterfaces()
                    .Any(Expression))
                    .ToList();

                foreach (var type in types)
                {
                    dynamic? configuration = Activator.CreateInstance(type);

                    if (configuration != null)
                    {
                        modelBuilder.ApplyConfiguration(configuration);
                    }
                }
            }

            return modelBuilder;
        }
    }
}
