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
    using Newtonsoft.Json;

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
        /// Valor por defecto que indica si esta habilitado el servicio.
        /// </summary>
        public const bool DefaultEnabled = false;

        /// <summary>
        /// Si se habilita la eliminación de la base de datos si esta existe.
        /// </summary>
        public const bool DefaultCreate = false;

        /// <summary>
        /// Si se habilitan las migraciones.
        /// </summary>
        public const bool DefaultDelete = false;

        /// <summary>
        /// Si se habilita el log de errores.
        /// </summary>
        public const bool DefaultMigrate = false;

        /// <summary>
        /// Si se habilita el log de errores.
        /// </summary>
        public const bool DefaultLogErrors = false;

        private bool _enabled = DefaultEnabled;
        private bool _create = DefaultCreate;
        private bool _delete = DefaultDelete;
        private bool _migrate = DefaultMigrate;
        private bool _logErrors = DefaultLogErrors;

        /// <summary>
        /// Obtiene o establece un valor que indica el servicio esta habilitado.
        /// <para><strong>Default:</strong> <see cref="DefaultEnabled"/> = false.</para>
        /// </summary>
        public bool? Enabled
        {
            get => _enabled;
            set
            {
                if (value.HasValue)
                {
                    _enabled = value.Value;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que indica si se habilita la creación de la base de datos si no existe.
        /// No utiliza migraciones para crear la base de datos y, por lo tanto, no se puede actualizar posteriormente mediante migraciones.
        /// </summary>
        public bool? Create
        {
            get => _create;
            set
            {
                if (value.HasValue)
                {
                    _create = value.Value;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que indica si se habilita la eliminación de la base de datos si esta existe.
        /// </summary>
        public bool? Delete
        {
            get => _delete;
            set
            {
                if (value.HasValue)
                {
                    _delete = value.Value;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que indica si se habilitan las migraciones.
        /// Creará la base de datos si aún no existe.
        /// Es mutuamente excluyente con IsEnsuredDeletedEnabled.
        /// </summary>
        public bool? Migrate
        {
            get => _migrate;
            set
            {
                if (value.HasValue)
                {
                    _migrate = value.Value;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor que indica si obtiene o establece un valor para habilitar el registro de errores del contexto.
        /// </summary>
        [JsonIgnore]
        public bool? LogErrors
        {
            get => _logErrors;
            set
            {
                if (value.HasValue)
                {
                    _logErrors = value.Value;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece un valor de la configuración del contexto.
        /// </summary>
        [JsonIgnore]
        public Action<DbContextOptionsBuilder>? DbContextOptions { get; set; }
    }
}
