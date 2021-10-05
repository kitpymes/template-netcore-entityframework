// -----------------------------------------------------------------------
// <copyright file="IEntityFrameworkUnitOfWork.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System.Data;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// En esta interfaz se pueden agregar todas las acciones comunes al contexto.
    /// </summary>
    public interface IEntityFrameworkUnitOfWork
    {
        /// <summary>
        /// Obtiene un valor para realizar transacciones.
        /// </summary>
        IDbContextTransaction Transaction { get; }

        /// <summary>
        /// Abrir una conexión a una bade de datos.
        /// </summary>
        /// <param name="isolationLevel">Especifica el comportamiento de bloqueo de transacciones para la conexión.</param>
        void OpenTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Abrir una conexión a una bade de datos.
        /// </summary>
        /// <param name="isolationLevel">Especifica el comportamiento de bloqueo de transacciones para la conexión.</param>
        /// <returns>Task.</returns>
        Task OpenTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Guarda una operación en la base de datos.
        /// </summary>
        /// <param name="useChangeTracker">Si utiliza el seguimiento de la operación.</param>
        void Save(bool useChangeTracker = true);

        /// <summary>
        /// Guarda una operación en la base de datos.
        /// </summary>
        /// <param name="useChangeTracker">Si utiliza el seguimiento de la operación.</param>
        /// <returns>Task.</returns>
        Task SaveAsync(bool useChangeTracker = true);
    }
}
