using Minq.Linq;
using System;

namespace Minq.Caching
{
	/// <summary>
	/// Defines an object that represents a repository of cached Sitecore templates.
	/// </summary>
	public class CachedTemplateRepository : ISitecoreTemplateGateway, ICachedRepository<SitecoreTemplateKey, STemplate>
	{
		private ISitecoreTemplateGateway _gateway;
		private ICachedRepository<SitecoreTemplateKey, STemplate> _repository;

		/// <summary>
		/// Creates a new cached template repository.
		/// </summary>
		/// <param name="gateway">The template gateway.</param>
		/// <param name="repository">The underlying repository to use for caching.</param>
		public CachedTemplateRepository(ISitecoreTemplateGateway gateway, ICachedRepository<SitecoreTemplateKey, STemplate> repository)
		{
			_gateway = gateway;
			_repository = repository;
		}

		/// <summary>
		/// Returns the Sitecore template for the given key or path.
		/// </summary>
		/// <param name="keyOrPath">The key or path identifying the template to return.</param>
		/// <param name="databaseName">The database of the template to return.</param>
		/// <returns></returns>
		public ISitecoreTemplate GetTemplate(string keyOrPath, string databaseName)
		{
			return _gateway.GetTemplate(keyOrPath, databaseName);
        }

		/// <summary>
		/// Clear the repository cache.
		/// </summary>
		public void ClearCache()
		{
			_repository.ClearCache();
		}

		/// <summary>
		/// Get or add a template from the cache.
		/// </summary>
		/// <param name="key">The key of the template.</param>
		/// <param name="value">The value to add if the template is not in the cache.</param>
		/// <returns>The cached template.</returns>
		public STemplate GetOrAdd(SitecoreTemplateKey key, STemplate value)
		{
			return _repository.GetOrAdd(key, value);
		}

		/// <summary>
		/// Get or add a template from the cache.
		/// </summary>
		/// <param name="key">The key of the template.</param>
		/// <param name="factory">The factory to use to create the template if it is not already in the cache.</param>
		/// <returns>The cached template.</returns>
		/// <remakrs>
		/// If the cost of creating the template is comparable to the cost of creating the factory, do not use this version of the method.
		/// </remakrs>
		public STemplate GetOrAdd(SitecoreTemplateKey key, Func<SitecoreTemplateKey, STemplate> factory)
		{
			return _repository.GetOrAdd(key, factory);
		}
	}
}
