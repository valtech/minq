using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace Minq.Mvc
{
	/// <summary>
	/// Provides a container for common metadata for Sitecore model properties relating to fields.
	/// </summary>
	public class SitecoreFieldMetadata
	{
		private SitecoreFieldMetadata()
		{
		}

		/// <summary>
		/// Gets the Sitecore item key associated with the Sitecore field associated with the metadata.
		/// </summary>
		public SitecoreItemKey Key
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the Sitecore field name associated with the metadata.
		/// </summary>
		public string FieldName
		{
			get;
			set;
		}

		/// <summary>
		/// Returns the metadata from the expression parameter for the model.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <typeparam name="TProperty">The type of the property in the expression.</typeparam>
		/// <param name="expression">An expression that identifies the model.</param>
		/// <param name="viewData">The view data dictionary.</param>
		/// <returns>The metadata.</returns>
		public static SitecoreFieldMetadata FromLambdaExpression<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression, ViewDataDictionary<TModel> viewData)
		{
			SitecoreFieldMetadata sitecoreMetadata = new SitecoreFieldMetadata();

			ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, viewData);
			
			if (viewData.Model != null)
			{
				SitecoreItemKey key = SitecoreItemKeyAttribute.FindKey<TModel>(viewData.Model);

				if (key != null)
				{
					sitecoreMetadata.Key = key;
				}
				else
				{
					throw new Exception(String.Format("Item of type {0} has a null key.", typeof(TModel)));
				}
			}
			else
			{
				throw new Exception("Cannot render a field for a null model.");
			}

			string fieldName = modelMetadata.PropertyName;

			SitecoreFieldAttribute itemFieldAttribute = SitecoreFieldAttribute.GetItemFieldAttribute(viewData.Model, modelMetadata.PropertyName);

			if (itemFieldAttribute != null)
			{
				fieldName = itemFieldAttribute.Name;
			}

			sitecoreMetadata.FieldName = fieldName;

			return sitecoreMetadata;
		}
	}
}
