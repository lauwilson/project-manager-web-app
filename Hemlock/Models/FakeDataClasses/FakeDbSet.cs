using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Hemlock.Models
{
    public class FakeDbSet<T> : IDbSet<T>
        where T : class
    {
        private ObservableCollection<T> _data;
        private IQueryable _query;

        public FakeDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();
        }

        public Type ElementType
        {
            get
            {
                return _query.ElementType;
            }
        }

        public Expression Expression
        {
            get
            {
                return _query.Expression;
            }
        }

        public ObservableCollection<T> Local
        {
            get
            {
                return _data;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return _query.Provider;
            }
        }

        public T Add(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public T Attach(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public T Remove(T entity)
        {
            _data.Remove(entity);
            return entity;
        }

        public virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Use the specific DBSet find() override.");
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        TDerivedEntity IDbSet<T>.Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}