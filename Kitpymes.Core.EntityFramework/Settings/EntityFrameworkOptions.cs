// -----------------------------------------------------------------------
// <copyright file="EntityFrameworkOptions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using Microsoft.EntityFrameworkCore;

    /*
       Clase de configuración EntityFrameworkOptions
       Contiene las propiedades para la configuración de entity framework
    */

    /// <summary>
    /// Clase de configuración <c>EntityFrameworkOptions</c>.
    /// Contiene las propiedades para la configuración de entity framework.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las propiedades para la configuración de entity framework.</para>
    /// </remarks>
    public class EntityFrameworkOptions
    {
        /// <summary>
        /// Obtiene la configuración de la entity framework.
        /// </summary>
        public EntityFrameworkSettings EntityFrameworkSettings { get; private set; } = new EntityFrameworkSettings();

        /// <summary>
        /// Configuración del contexto.
        /// </summary>
        /// <param name="dbContextOptionsBuilder">Opciones del contexto.</param>
        /// <returns>EntityFrameworkOptions.</returns>
        public virtual EntityFrameworkOptions WithDbContextOptions(Action<DbContextOptionsBuilder> dbContextOptionsBuilder)
        {
            EntityFrameworkSettings.DbContextOptionsBuilder = dbContextOptionsBuilder;

            return this;
        }

        /// <summary>
        /// Indica si se habilita la creación de la base de datos si no existe.
        /// No utiliza migraciones para crear la base de datos y, por lo tanto, no se puede actualizar posteriormente mediante migraciones.
        /// </summary>
        /// <param name="enabled">Si se habilita o no.</param>
        /// <returns>EntityFrameworkOptions.</returns>
        public virtual EntityFrameworkOptions WithEnsuredCreated(bool enabled = true)
        {
            EntityFrameworkSettings.IsEnsuredCreatedEnabled = enabled;
            EntityFrameworkSettings.IsMigrateEnabled = !enabled;

            return this;
        }

        /// <summary>
        /// Indica si se habilitan las migraciones.
        /// Creará la base de datos si aún no existe.
        /// Es mutuamente excluyente con IsEnsuredDeletedEnabled./// Indica si se habilita las transacciones.
        /// </summary>
        /// <param name="enabled">Si se habilita o no.</param>
        /// <returns>EntityFrameworkOptions.</returns>
        public virtual EntityFrameworkOptions WithMigrateEnabled(bool enabled = true)
        {
            EntityFrameworkSettings.IsMigrateEnabled = enabled;
            EntityFrameworkSettings.IsEnsuredCreatedEnabled = !enabled;

            return this;
        }

        /// <summary>
        /// Indica si se habilita la eliminación de la base de datos si esta existe.
        /// </summary>
        /// <param name="enabled">Si se habilita o no.</param>
        /// <returns>EntityFrameworkOptions.</returns>
        public virtual EntityFrameworkOptions WithEnsuredDeleted(bool enabled = true)
        {
            EntityFrameworkSettings.IsEnsuredDeletedEnabled = enabled;

            return this;
        }
    }
}
