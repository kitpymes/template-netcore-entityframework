// -----------------------------------------------------------------------
// <copyright file="IEntityFrameworkDbContext.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    /// <summary>
    /// En esta interfaz se pueden agregar todas las acciones comunes al contexto.
    /// </summary>
    public interface IEntityFrameworkDbContext
    {
        /// <summary>
        /// Proporciona acceso a información y operaciones de seguimiento de cambios para una entidad determinada.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de entidad.</typeparam>
        /// <param name="entity">Entidad.</param>
        /// <returns>Entidad de seguimiento.</returns>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// Guarda una operación en la base de datos.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Guarda una operación en la base de datos.
        /// </summary>
        /// <param name="cancellationToken">Para observar mientras espera a que se complete la tarea.</param>
        /// <returns>Task.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Guarda una operación en la base de datos con transacciones.
        /// </summary>
        /// <param name="isolationLevel">Especifica el comportamiento de bloqueo de transacciones para la conexión.</param>
        void SaveChangesWithTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Guarda una operación en la base de datos con transacciones.
        /// </summary>
        /// <param name="isolationLevel">Especifica el comportamiento de bloqueo de transacciones para la conexión.</param>
        /// <param name="cancellationToken">Para observar mientras espera a que se complete la tarea.</param>
        /// <returns>Task.</returns>
        Task SaveChangesWithTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
    }
}
