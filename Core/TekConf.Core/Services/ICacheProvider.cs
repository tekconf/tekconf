using System;
using System.Collections.Generic;
using System.Linq;

namespace TekConf.Core.Services
{
	public interface ICacheService
	{
		void Add<TKey, TValue>(TKey key, TValue value, TimeSpan relativeTime) where TValue : class;
		void Add<TKey, TValue>(TKey key, TValue value, DateTime absoluteTime) where TValue : class;
		TValue Get<TKey, TValue>(TKey key) where TValue : class;
		void Remove<TKey>(TKey key);
		void Clear();
		void PreemptiveInitialise();
		bool IsCacheable<T>(T value, ref IEnumerable<Type> failingTypes) where T : class;
		IEnumerable<TKey> Keys<TKey>();
	}

	public sealed class CacheService : ICacheService, IDisposable
	{
		private readonly IDictionary<object, object> dictionary = new Dictionary<object, object>();

		private readonly object sync = new object();

		public void Dispose()
		{
			dictionary.Clear();
		}

		#region ICacheProvider Members

		public void Add<TKey, TValue>(TKey key, TValue value, TimeSpan relativeTime) where TValue : class
		{
			AddImpl(key, value);
		}

		public void Add<TKey, TValue>(TKey key, TValue value, DateTime absoluteTime) where TValue : class
		{
			if (absoluteTime < DateTime.Now)
			{
				return;
			}

			AddImpl(key, value);
		}

		public TValue Get<TKey, TValue>(TKey key) where TValue : class
		{
			object value;
			if (dictionary.TryGetValue(key, out value))
			{
				return (TValue)value;
			}

			return null;
		}

		public void Remove<TKey>(TKey key)
		{
			if (Equals(key, null))
			{
				return;
			}

			Purge(key);
		}

		public void Clear()
		{
			lock (sync)
			{
				dictionary.Clear();
			}
		}

		public void PreemptiveInitialise()
		{
		}

		public bool IsCacheable<T>(T value, ref IEnumerable<Type> failingTypes) where T : class
		{
			return true;
		}

		public IEnumerable<TKey> Keys<TKey>()
		{
			lock (sync)
			{
				return dictionary.Keys.Where(k => k.GetType() == typeof(TKey)).Cast<TKey>().ToList();
			}
		}

		#endregion

		private void AddImpl<TKey, TValue>(TKey key, TValue value)
		{
			lock (sync)
			{
				//Observable.Timer(relative)
				//		.Finally(() =>
				//		{
				//			GC.Collect();
				//			GC.WaitForPendingFinalizers();
				//		})
				//		.Subscribe(x => this.Purge(key),
				//		exn => this.log.Write("InMemoryCacheProvider: Purge Failed - '{0}'", exn.Message),
				//		() => this.log.Write("InMemoryCacheProvider: Purge Completed..."));

				dictionary[key] = value;
			}

		}

		private void Purge(object key)
		{
			lock (sync)
			{
				dictionary.Remove(key);
			}

		}
	}
}