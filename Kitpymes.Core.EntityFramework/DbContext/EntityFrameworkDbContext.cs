// -----------------------------------------------------------------------
// <copyright file="EntityFrameworkDbContext.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using System.Data;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    /*
       Clase de extensión EntityFrameworkDbContext
       Contiene el contexto base para entity framework
    */

    /// <summary>
    /// Clase de extensión <c>EntityFrameworkDbContext</c>.
    /// Contiene el contexto base para entity framework.
    /// </summary>
    /// <remarks>
    /// <para>En esta clase se pueden agregar todas las acciones comunes para el contexto de entity framework.</para>
    /// </remarks>
    public abstract class EntityFrameworkDbContext : DbContext, IEntityFrameworkDbContext
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="EntityFrameworkDbContext"/>.
        /// </summary>
        /// <param name="options">Configuración del contexto.</param>
        public EntityFrameworkDbContext(DbContextOptions options)
            : base(options)
        {
        }

        private IDbContextTransaction Transaction { get; set; } = null!;

        #region Save

        /// <inheritdoc/>
        public new virtual void SaveChanges()
        {
            try
            {
                base.SaveChanges();
            }
            catch (Exception exception)
            {
                ThrowSave(exception);
            }
        }

        /// <inheritdoc/>
        public new virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                await ThrowSaveAsync(exception);
            }
        }

        /// <inheritdoc/>
        public virtual void SaveChangesWithTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            try
            {
                if (Transaction is not null)
                {
                    Transaction.Dispose();
                }

                Transaction = Database.BeginTransaction(isolationLevel);

                base.SaveChanges();

                if (Transaction is not null)
                {
                    Transaction.Commit();
                }
            }
            catch (Exception exception)
            {
                CloseTransaction();

                ThrowSave(exception);
            }
        }

        /// <inheritdoc/>
        public virtual async Task SaveChangesWithTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            try
            {
                if (Transaction is not null)
                {
                    await Transaction.DisposeAsync();
                }

                Transaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);

                await base.SaveChangesAsync(cancellationToken);

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

        private void CloseTransaction()
        {
            if (Transaction is not null)
            {
                Transaction.Dispose();
            }

            if (this is not null)
            {
                Database.CloseConnection();

                Dispose();
            }
        }

        private async Task CloseTransactionAsync()
        {
            if (Transaction is not null)
            {
                await Transaction.DisposeAsync();
            }

            if (this is not null)
            {
                await Database.CloseConnectionAsync();

                await DisposeAsync();
            }
        }

        private void ThrowSave(Exception exception)
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
        }

        private async Task ThrowSaveAsync(Exception exception)
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
