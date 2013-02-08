using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoNamesOrgImporter
{
	public interface IRepository<T> where T : class
	{
		void Save(T entity);
		IQueryable<T> AsQueryable();
		void Remove(Guid id);
	}
}
