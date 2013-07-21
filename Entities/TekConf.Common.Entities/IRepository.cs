using System;
using System.Linq;

namespace TekConf.Common.Entities
{
	public interface IRepository<T> where T : class
	{
		void Save(T entity);
		IQueryable<T> AsQueryable();
		void Remove(Guid id);
	}
}