// -----------------------------------------------------------------------
// <copyright file="EntityFrameworkUnitOfWork.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using System.Data;
    using System.Text;
    using System.Threading.Tasks;
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <inheritdoc/>
    public class EntityFrameworkUnitOfWork : IEntityFrameworkUnitOfWork
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="EntityFrameworkUnitOfWork"/>.
        /// </summary>
        /// <param name="context">Contexto de datos.</param>
        public EntityFrameworkUnitOfWork(DbContext context)
        => Context = context.ToIsNullOrEmptyThrow(nameof(context));

        private IDbContextTransaction? Transaction { get; set; }

        private DbContext Context { get; }

        #region Transaction

        /// <inheritdoc/>
        public void OpenTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        => Task.FromResult(OpenTransactionAsync(isolationLevel));

        /// <inheritdoc/>
        public async Task OpenTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (Transaction != null)
            {
                await Transaction.DisposeAsync();
            }

            Transaction = await Context.Database.BeginTransactionAsync(isolationLevel).ConfigureAwait(false);
        }

        #endregion Transaction

        #region Save

        /// <inheritdoc/>
        public void Save() => Task.FromResult(SaveAsync());

        /// <inheritdoc/>
        public void Save(bool useChangeTracker = false) => Task.FromResult(SaveAsync(useChangeTracker));

        /// <inheritdoc/>
        public async Task SaveAsync(bool useChangeTracker = false)
        {
            try
            {
                await Context
                    .WithChangeTracker(useChangeTracker)
                    .SaveChangesAsync()
                    .ConfigureAwait(false);

                if (Transaction != null)
                {
                    await Transaction.CommitAsync().ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                await CloseTransactionAsync().ConfigureAwait(false);

                ThrowSave(exception);
            }
        }

        #endregion Save

        #region Private

        private async Task CloseTransactionAsync()
        {
            if (Transaction != null)
            {
                await Transaction.DisposeAsync();
            }

            if (Context != null)
            {
                await Context.Database.CloseConnectionAsync().ConfigureAwait(false);

                await Context.DisposeAsync();
            }
        }

        private void ThrowSave(Exception exception)
        {
            var sb = new StringBuilder();

            switch (exception)
            {
                case DbUpdateConcurrencyException dbUpdateConcurrencyException when exception is DbUpdateConcurrencyException:

                    sb.AppendLine(dbUpdateConcurrencyException.ToFullMessage());

                    if (dbUpdateConcurrencyException?.Entries != null)
                    {
                        foreach (var eve in dbUpdateConcurrencyException.Entries)
                        {
                            sb.Append("Entity of type ")
                                .Append(eve.Entity.GetType().Name)
                                .Append(" in state ")
                                .Append(eve.State)
                                .AppendLine(" could not be updated");
                        }
                    }

                    throw new DbUpdateConcurrencyException(sb.ToString());

                case DbUpdateException dbUpdateException when exception is DbUpdateException:

                    sb.AppendLine(dbUpdateException.ToFullMessage());

                    if (dbUpdateException?.Entries != null)
                    {
                        foreach (var eve in dbUpdateException.Entries)
                        {
                            sb.Append("Entity of type ")
                                .Append(eve.Entity.GetType().Name)
                                .Append(" in state ")
                                .Append(eve.State)
                                .AppendLine(" could not be updated");
                        }
                    }

                    throw new DbUpdateException(sb.ToString());

                default:

                    sb.AppendLine(exception.ToFullMessage());

                    if (exception?.Data != null)
                    {
                        foreach (var eve in exception.Data)
                        {
                            sb.Append("Entity of type ")
                                .AppendLine(eve?.GetType().Name);
                        }
                    }

                    throw new Exception(sb.ToString());
            }
        }

        #endregion Private
    }
}
