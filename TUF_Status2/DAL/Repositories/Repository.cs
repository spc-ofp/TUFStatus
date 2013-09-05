using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using TUFStatus.DAL.Infrastructure;

// -----------------------------------------------------------------------
// <copyright file="Repository.cs" company="Secretariat of the Pacific Community">
// Copyright (C) 2012 Secretariat of the Pacific Community
// </copyright>
// -----------------------------------------------------------------------
namespace TUFStatus.DAL.Repositories
{
    /// <summary>
    /// Repository implementation that hides the differences between
    /// stateless and stateful sessions.
    /// </summary>
    public class Repository<S, T> : IRepository<T>, IDisposable
        where T : class
        where S : class
    {
        private readonly S _session;
        private readonly bool _isStateless;
        private bool _disposed; // For IDisposable

        // Use delegates so that we've got the same internal interface to the
        // different operations that each session type implements.
        delegate T ReturningOperation(object id);
        delegate void Operation(T entity);
        delegate void MergingOperation(T entity, bool merge);
        delegate IQueryable<T> QueryOperation();

        // Pointers to the actual functions used to perform the
        // NHibernate operations.
        private readonly Operation CreateOperation;
        private readonly ReturningOperation ReadOperation;
        private readonly MergingOperation UpdateOperation;
        private readonly Operation DeleteOperation;
        private readonly QueryOperation ListOperation;

        /// <summary>
        /// Internal so that we can force creation via TubsDataService
        /// (I refuse to say RepositoryFactory)
        /// </summary>
        /// <param name="session"></param>
        internal Repository(S session)
        {
            this._session = session;
            this._isStateless = session is IStatelessSession;
            if (this._isStateless)
            {
                CreateOperation = (x) => GetStatelessSession().Insert(x);
                ReadOperation = (x) => GetStatelessSession().Get<T>(x);
                UpdateOperation = (x, m) => GetStatelessSession().Update(x);
                DeleteOperation = (x) => GetStatelessSession().Delete(x);
                ListOperation = () => GetStatelessSession().Query<T>();
            }
            else
            {
                CreateOperation = (x) => GetSession().SaveOrUpdate(x);
                ReadOperation = (x) => GetSession().Get<T>(x);
                UpdateOperation = (x, m) =>
                {
                    if (m)
                    {
                        GetSession().Merge(x);
                    }
                    else
                    {
                        GetSession().SaveOrUpdate(x);
                    }
                };
                DeleteOperation = (x) => GetSession().Delete(x);
                ListOperation = () => GetSession().Query<T>();
            }
        }

        private IStatelessSession GetStatelessSession()
        {
            return this._session as IStatelessSession;
        }

        private ISession GetSession()
        {
            return this._session as ISession;
        }

        public T FindById(object id)
        {            
            try
            {
                return ReadOperation(id);
            }
            catch (ObjectNotFoundException)
            {
                return null;
            }
        }

        public void Add(T entity)
        {
            CreateOperation(entity);
        }

        public void Add(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                CreateOperation(item);
            }
        }

        public void Update(T entity, bool autoMerge = false)
        {
            UpdateOperation(entity, autoMerge);
        }

        public void Update(IEnumerable<T> items, bool autoMerge = false)
        {
            foreach (T item in items)
            {
                UpdateOperation(item, autoMerge);
            }
        }

        // TODO: Test me!
        public void Reload(T entity)
        {
            if (!_isStateless)
            {
                GetSession().Refresh(entity);
            }
        }

        // At this rate I'd be better off just using ISession...
        public void Evict(T entity)
        {
            if (!_isStateless)
            {
                GetSession().Evict(entity);
            }
        }

        //
        public void Save(T entity)
        {
            // This caused a problem with PageCount -- if the
            // entity doesn't implement IEntity, then .Save(...) does nothing
            // which is not what we want.
            if (null != entity && entity is IEntity)
            {
                var e = entity as IEntity;
                if (e.IsNew())
                {
                    Add(entity);
                }
                else
                {
                    Update(entity, true);
                }
            }
        }

        public ICriteria CreateCriteria()
        {
            return
                this._isStateless ?
                    (GetStatelessSession()).CreateCriteria<T>() :
                    (GetSession()).CreateCriteria<T>();
        }

        public ICriteria CreateCriteria(string alias)
        {
            return
                this._isStateless ?
                    (GetStatelessSession()).CreateCriteria<T>(alias) :
                    (GetSession()).CreateCriteria<T>(alias);
        }

        public IMultiCriteria CreateMultiCriteria()
        {
            if (this._isStateless)
            {
                throw new InvalidOperationException("IStatelessSession doesn't support multiple criteria");
            }
            return GetSession().CreateMultiCriteria();
        }

        public void DeleteById(object id)
        {
            var item = this.FindById(id);
            this.Delete(item);
        }

        public void Delete(T entity)
        {
            if (null == entity)
            {
                return;
            }
            
            DeleteOperation(entity);
        }

        public IQueryable<T> All()
        {
            return ListOperation();
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Okay to suppress for LINQ expressions")]
        public T FindBy(Expression<Func<T, bool>> expression)
        {
            return this.FilterBy(expression).SingleOrDefault();
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Okay to suppress for LINQ expressions")]
        public IQueryable<T> FilterBy(Expression<Func<T, bool>> expression)
        {
            return this.All().Where(expression); //.AsQueryable();
        }

        ~Repository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called
            if (!this._disposed)
            {
                // If disposing is true, dispose all managed and unmanaged resources
                if (disposing)
                {
                    if (null != this._session)
                    {
                        lock (this._session)
                        {
                            if (this._isStateless)
                            {
                                if (GetStatelessSession().IsOpen)
                                {
                                    GetStatelessSession().Close();
                                }
                            }
                            else
                            {
                                if (GetSession().IsOpen)
                                {
                                    GetSession().Close();
                                }
                            }
                        }
                    }
                }

                this._disposed = true;
            }
        }


    }
}