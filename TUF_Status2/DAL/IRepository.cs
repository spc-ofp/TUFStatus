// -----------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Secretariat of the Pacific Community">
// Copyright (C) 2012 Secretariat of the Pacific Community
// </copyright>
// -----------------------------------------------------------------------

namespace TUFStatus.DAL
{
    /*
     * This file is part of TUBS.
     *
     * TUBS is free software: you can redistribute it and/or modify
     * it under the terms of the GNU Affero General Public License as published by
     * the Free Software Foundation, either version 3 of the License, or
     * (at your option) any later version.
     *  
     * TUBS is distributed in the hope that it will be useful,
     * but WITHOUT ANY WARRANTY; without even the implied warranty of
     * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     * GNU Affero General Public License for more details.
     *  
     * You should have received a copy of the GNU Affero General Public License
     * along with TUBS.  If not, see <http://www.gnu.org/licenses/>.
     */
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using NHibernate;

    /// <summary>
    /// Interface shared by stateless and stateful repositories.
    /// </summary>
    public interface IRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Find entity by primary key
        /// </summary>
        /// <param name="id">Primary key, as object</param>
        /// <returns>Entity with specified identifier or null if no such entity exists.</returns>
        T FindById(object id);

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">Entity to be added</param>
        /// <returns></returns>
        void Add(T entity);

        void Add(IEnumerable<T> items);

        void Update(T entity, bool autoMerge = false);

        void Update(IEnumerable<T> items, bool autoMerge = false);

        void Reload(T entity);

        void Evict(T entity);

        void Save(T entity);

        ICriteria CreateCriteria();

        ICriteria CreateCriteria(string alias);

        IMultiCriteria CreateMultiCriteria();

        void DeleteById(object id);

        void Delete(T entity);

        IQueryable<T> All();

        T FindBy(Expression<Func<T, bool>> expression);

        IQueryable<T> FilterBy(Expression<Func<T, bool>> expression);
    }
}