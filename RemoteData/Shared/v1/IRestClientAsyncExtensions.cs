namespace TekConf.RemoteData.v1
{
	using System.Threading.Tasks;

	using ServiceStack.Service;
	using ServiceStack.ServiceHost;

	public static class IRestClientAsyncExtensions
	{
		//public static Task<T> GetAsync<T>(this IRestClientAsync client, string relativeOrAbsoluteUrl)
		//{
		//	var task = new TaskCompletionSource<T>();
		//	client.GetAsync<T>(relativeOrAbsoluteUrl,
		//		onSuccess: task.SetResult,
		//		onError: (result, exception) => task.SetException(exception));
			
		//	return task.Task;
		//}

		public static Task<T> GetAsync<T>(this IRestClientAsync client, IReturn<T> request)
		{
			var task = new TaskCompletionSource<T>();
			client.GetAsync<T>(request,
				onSuccess: task.SetResult,
				onError: (result, exception) => task.SetException(exception));

			return task.Task;
		}

		public static Task<T> PostAsync<T>(this IRestClientAsync client, IReturn<T> request)
		{
			var task = new TaskCompletionSource<T>();
			client.PostAsync<T>(request,
				onSuccess: task.SetResult,
				onError: (result, exception) => task.SetException(exception));

			return task.Task;
		}

		public static Task<T> PutAsync<T>(this IRestClientAsync client, IReturn<T> request)
		{
			var task = new TaskCompletionSource<T>();
			client.PutAsync<T>(request,
				onSuccess: task.SetResult,
				onError: (result, exception) => task.SetException(exception));

			return task.Task;
		}

		public static Task<T> DeleteAsync<T>(this IRestClientAsync client, IReturn<T> request)
		{
			var task = new TaskCompletionSource<T>();
			client.DeleteAsync<T>(request,
				onSuccess: task.SetResult,
				onError: (result, exception) => task.SetException(exception));

			return task.Task;
		}	

	}
}