// -----------------------------------------------------------------------
// <copyright file="EntityFrameworkRepository.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Kitpymes.Core.Repositories;
    using Kitpymes.Core.Shared;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    /// <inheritdoc/>
    public class EntityFrameworkRepository<T> : IRelationalRepository<T>
        where T : class
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="EntityFrameworkRepository{T}"/>.
        /// </summary>
        /// <param name="context">Contexto de datos.</param>
        public EntityFrameworkRepository(DbContext context) => Context = context;

        /// <inheritdoc/>
        public IQueryable<T> Query => Context.ToQueryable<T>();

        private DbContext Context { get; }

        #region GetOne

        /// <inheritdoc/>
        public T GetOne(Expression<Func<T, bool>> where)
        => Query.First(where);

        /// <inheritdoc/>
        public async Task<T> GetOneAsync(Expression<Func<T, bool>> where)
        => await Query.FirstOrDefaultAsync(where).ConfigureAwait(false);

        /// <inheritdoc/>
        public T GetOne(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        => Query.ToInclude(includes).First(where);

        /// <inheritdoc/>
        public async Task<T> GetOneAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        => await Query.ToInclude(includes).FirstOrDefaultAsync(where).ConfigureAwait(false);

        /// <inheritdoc/>
        public TResult GetOne<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>>? where = null,
            params Expression<Func<T, object>>[] includes)
        => Query.ToInclude(includes).ToWhere(where).Select(select).First();

        /// <inheritdoc/>
        public async Task<TResult> GetOneAsync<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>>? where = null,
            params Expression<Func<T, object>>[] includes)
        => await Query.ToInclude(includes).ToWhere(where).Select(select).FirstOrDefaultAsync().ConfigureAwait(false);

        #endregion GetOne

        #region GetAll

        /// <inheritdoc/>
        public IEnumerable<T> GetAll()
        => Query.ToList();

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetAllAsync()
        => await Query.ToListAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> where)
        => Query.ToWhere(where).ToList();

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> where)
        => await Query.ToWhere(where).ToListAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public IEnumerable<T> GetAll(
            Expression<Func<T, bool>> where,
            params Expression<Func<T, object>>[] includes)
        => Query.ToWhere(where).ToInclude(includes);

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> where,
            params Expression<Func<T, object>>[] includes)
        => await Query.ToWhere(where).ToInclude(includes).ToListAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public IEnumerable<TResult> GetAll<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>>? where = null,
            params Expression<Func<T, object>>[] includes)
        => Query.ToWhere(where).ToInclude(includes).Select(select);

        /// <inheritdoc/>
        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>>? where = null,
            params Expression<Func<T, object>>[] includes)
        => await Query.ToWhere(where).ToInclude(includes).Select(select).ToListAsync().ConfigureAwait(false);

        #endregion GetAll

        #region GetPaged

        /// <inheritdoc/>
        public IEnumerable<T> GetPaged(string property, Action<PagedOptions> options)
        {
            var settings = options.ToConfigureOrDefault();

            return Query.ToPaged(property, settings.Ascending, settings.Index, settings.Size);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<T>> GetPagedAsync(string property, Action<PagedOptions> options)
        => Task.FromResult(GetPaged(property, options));

        /// <inheritdoc/>
        public IEnumerable<T> GetPaged(
            string property,
            Action<PagedOptions>? options = null,
            Expression<Func<T, bool>>? where = null,
            params Expression<Func<T, object>>[] includes)
        {
            var settings = options.ToConfigureOrDefault();

            return Query.ToWhere(where).ToInclude(includes).ToPaged(property, settings.Ascending, settings.Index, settings.Size);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetPagedAsync(
            string property,
            Action<PagedOptions>? options = null,
            Expression<Func<T, bool>>? where = null,
            params Expression<Func<T, object>>[] includes)
        {
            var settings = options.ToConfigureOrDefault();

            return await Query.ToWhere(where).ToInclude(includes).ToPaged(property, settings.Ascending, settings.Index, settings.Size).ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public IEnumerable<TResult> GetPaged<TResult>(
            string property,
            Expression<Func<T, TResult>> select,
            Action<PagedOptions>? options = null,
            Expression<Func<T, bool>>? where = null,
            params Expression<Func<T, object>>[] includes)
        {
            var settings = options.ToConfigureOrDefault();

            return Query.ToWhere(where).ToInclude(includes).Select(select).ToPaged(property, settings.Ascending, settings.Index, settings.Size);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TResult>> GetPagedAsync<TResult>(
            string property,
            Expression<Func<T, TResult>> select,
            Action<PagedOptions>? options = null,
            Expression<Func<T, bool>>? where = null,
            params Expression<Func<T, object>>[] includes)
        {
            var settings = options.ToConfigureOrDefault();

            return await Query.ToWhere(where).ToInclude(includes).Select(select).ToPaged(property, settings.Ascending, settings.Index, settings.Size).ToListAsync().ConfigureAwait(false);
        }

        #endregion GetPaged

        #region Find

        /// <inheritdoc/>
        public T Find(object key)
        => Context.WithOptimizedContext(false).Set<T>().Find(key);

        /// <inheritdoc/>
        public async Task<T> FindAsync(object key)
        => await Context.WithOptimizedContext(false).Set<T>().FindAsync(key);

        #endregion Find

        #region Add

        /// <inheritdoc/>
        public void Add(T item)
        => Context.WithOptimizedContext().Set<T>().Add(item);

        /// <inheritdoc/>
        public async Task AddAsync(T item)
        => await Context.WithOptimizedContext().Set<T>().AddAsync(item);

        /// <inheritdoc/>
        public void AddRange(IEnumerable<T> items)
        => Context.WithOptimizedContext().Set<T>().AddRange(items);

        /// <inheritdoc/>
        public Task AddRangeAsync(IEnumerable<T> items)
        => Context.WithOptimizedContext().Set<T>().AddRangeAsync(items);

        #endregion Add

        #region Update

        /// <inheritdoc/>
        public void Update(object key, T item) => UpdateItem(key, item);

        /// <inheritdoc/>
        public Task UpdateAsync(object key, T item)
        {
            Update(key, item);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void UpdatePartial(object key, object item) => UpdateItem(key, item);

        /// <inheritdoc/>
        public Task UpdatePartialAsync(object key, object item)
        {
            UpdatePartial(key, item);

            return Task.CompletedTask;
        }

        #endregion Update

        #region Delete

        /// <inheritdoc/>
        public void Delete(object key)
        {
            var item = Context.WithOptimizedContext().Set<T>().Find(key);

            if (item != default)
            {
                Context.Set<T>().Remove(item);
            }
        }

        /// <inheritdoc/>
        public void Delete(Expression<Func<T, bool>> where)
        {
            var items = Context.WithOptimizedContext().Set<T>().Where(where);

            if (items.Any())
            {
                Context.Set<T>().RemoveRange(items);
            }
        }

        /// <inheritdoc/>
        public Task DeleteAsync(object key)
        {
            Delete(key);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task DeleteAsync(Expression<Func<T, bool>> where)
        {
            Delete(where);

            return Task.CompletedTask;
        }

        #endregion Delete

        #region Any

        /// <inheritdoc/>
        public bool Any()
        => Query.Any();

        /// <inheritdoc/>
        public bool Any(Expression<Func<T, bool>> where)
        => Query.Any(where);

        /// <inheritdoc/>
        public Task<bool> AnyAsync()
        => Query.AnyAsync();

        /// <inheritdoc/>
        public Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        => Query.AnyAsync(where);

        #endregion Any

        #region Count

        /// <inheritdoc/>
        public long Count()
        => Query.LongCount();

        /// <inheritdoc/>
        public long Count(Expression<Func<T, bool>> where)
        => Query.LongCount(where);

        /// <inheritdoc/>
        public Task<long> CountAsync()
        => Query.LongCountAsync();

        /// <inheritdoc/>
        public Task<long> CountAsync(Expression<Func<T, bool>> where)
        => Query.LongCountAsync(where);

        #endregion Count

        #region Private

        private static void UpdateReferences(EntityEntry<T> entry, object item)
        {
            foreach (var reference in entry.References)
            {
                var property = item.GetType().GetProperty(reference.Metadata.Name);

                if (property != default)
                {
                    reference.CurrentValue = property.GetValue(item, default);
                }
            }
        }

        private void UpdateItem(object key, object item)
        {
            var entity = Context.WithOptimizedContext(false).Set<T>().Find(key);

            var entry = Context.Entry(entity);

            entry.CurrentValues.SetValues(item);

            UpdateReferences(entry, item);
        }

        #endregion
    }
}