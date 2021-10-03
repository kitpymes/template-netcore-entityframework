// -----------------------------------------------------------------------
// <copyright file="EntityFrameworkSettings.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using Microsoft.EntityFrameworkCore;

    /*
       Clase de configuración EntityFrameworkSettings
       Contiene las propiedades para la configuración de entity framework
    */

    /// <summary>
    /// Clase de configuración <c>EntityFrameworkSettings</c>.
    /// Contiene las propiedades para la configuración de entity framework.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las propiedades para la configuración de entity framework.</para>
    /// </remarks>
    public class EntityFrameworkSettings
    {
        /// <summary>
        /// Si se habilita la eliminación de la base de datos si esta existe.
        /// </summary>
        public const bool DefaultIsEnsuredCreatedEnabled = false;

        /// <summary>
        /// Si se habilitan las migraciones.
        /// </summary>
        public const bool DefaultIsEnsuredDeletedEnabled = false;

        /// <summary>
        /// Si se habilita el log de errores.
        /// </summary>
        public const bool DefaultIsMigrateEnabled = false;

        private bool isEnsuredCreatedEnabled = DefaultIsEnsuredCreatedEnabled;
        private bool isEnsuredDeletedEnabled = DefaultIsEnsuredDeletedEnabled;
        private bool isMigrateEnabled = DefaultIsMigrateEnabled;

        /// <summary>
        /// Obtiene o establece un valor de la configuración del contexto.
        /// </summary>
        public Action<DbContextOptionsBuilder>? DbContextOptionsBuilder { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si se habilita la creación de la base de datos si no existe.
        /// No utiliza migraciones para crear la base de datos y, por lo tanto, no se puede actualizar posteriormente mediante migraciones.
        /// </summary>
        public bool? IsEnsuredCreatedEnabled
        {
            get => isEnsuredCreatedEnabled;
            set
            {
                if (value.HasValue)
                {
                    isEnsuredCreatedEnabled = value.Value;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que indica si se habilita la eliminación de la base de datos si esta existe.
        /// </summary>
        public bool? IsEnsuredDeletedEnabled
        {
            get => isEnsuredDeletedEnabled;
            set
            {
                if (value.HasValue)
                {
                    isEnsuredDeletedEnabled = value.Value;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que indica si se habilitan las migraciones.
        /// Creará la base de datos si aún no existe.
        /// Es mutuamente excluyente con IsEnsuredDeletedEnabled.
        /// </summary>
        public bool? IsMigrateEnabled
        {
            get => isMigrateEnabled;
            set
            {
                if (value.HasValue)
                {
                    isMigrateEnabled = value.Value;
                }
            }
        }
    }
}
