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
    public class EntityFrameworkUnitOfWork<TDbContext> : IEntityFrameworkUnitOfWork
        where TDbContext : DbContext
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="EntityFrameworkUnitOfWork{TDbContext}"/>.
        /// </summary>
        /// <param name="context">Contexto de datos.</param>
        public EntityFrameworkUnitOfWork(TDbContext context)
        => Context = context.ToIsNullOrEmptyThrow(nameof(context));

        private IDbContextTransaction? Transaction { get; set; }

        private TDbContext Context { get; }

        #region Transaction

        /// <inheritdoc/>
        public virtual void OpenTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        => Task.FromResult(OpenTransactionAsync(isolationLevel));

        /// <inheritdoc/>
        public virtual async ValueTask OpenTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (Transaction is not null)
            {
                await Transaction.DisposeAsync();
            }

            Transaction = await Context.Database.BeginTransactionAsync(isolationLevel);
        }

        #endregion Transaction

        #region Save

        /// <inheritdoc/>
        public virtual void Save() => Task.FromResult(SaveAsync());

        /// <inheritdoc/>
        public virtual void Save(bool useChangeTracker = true) => Task.FromResult(SaveAsync(useChangeTracker));

        /// <inheritdoc/>
        public virtual async ValueTask SaveAsync(bool useChangeTracker = true)
        {
            try
            {
                await Context.WithChangeTracker(useChangeTracker).SaveChangesAsync();

                if (Transaction is not null)
                {
                    await Transaction.CommitAsync();
                }
            }
            catch (Exception exception)
            {
                await CloseTransactionAsync();

                await ThrowSaveAsync(exception);
            }
        }

        #endregion Save

        #region Private

        private async ValueTask CloseTransactionAsync()
        {
            if (Transaction is not null)
            {
                await Transaction.DisposeAsync();
            }

            if (Context is not null)
            {
                await Context.Database.CloseConnectionAsync();

                await Context.DisposeAsync();
            }
        }

        private async ValueTask ThrowSaveAsync(Exception exception)
        {
            var sb = new StringBuilder();

            switch (exception)
            {
                case DbUpdateConcurrencyException dbUpdateConcurrencyException when exception is DbUpdateConcurrencyException:

                    sb.AppendLine(dbUpdateConcurrencyException.ToFullMessage());

                    if (dbUpdateConcurrencyException.Entries is not null)
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

                    if (dbUpdateException?.Entries is not null)
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

                    if (exception?.Data is not null)
                    {
                        foreach (var eve in exception.Data)
                        {
                            sb.Append("Entity of type ")
                                .AppendLine(eve?.GetType().Name);
                        }
                    }

                    throw new Exception(sb.ToString());
            }

#pragma warning disable CS0162 // Se detectó código inaccesible
            await Task.CompletedTask;
#pragma warning restore CS0162 // Se detectó código inaccesible
        }

        #endregion Private
    }
}
