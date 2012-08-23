using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.UI;

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

			TModel model = viewData.Model;

			if (model != null)
			{
				object container = ContainerFromExpression(expression, model);

				if (container == null)
				{
					throw new Exception(String.Format("Container {0} on model {1} is null for expression path {2}.", modelMetadata.ContainerType, typeof(TModel), ExpressionHelper.GetExpressionText(expression)));
				}

				SitecoreItemKey key = SitecoreItemKeyAttribute.FindKey(container);

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

			SitecoreFieldAttribute itemFieldAttribute = SitecoreFieldAttribute.GetItemFieldAttribute(modelMetadata.ContainerType, modelMetadata.PropertyName);

			if (itemFieldAttribute != null)
			{
				fieldName = itemFieldAttribute.Name;
			}

			sitecoreMetadata.FieldName = fieldName;

			return sitecoreMetadata;
		}

		public static object ContainerFromExpression<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression, TModel model)
		{
			string path = ExpressionHelper.GetExpressionText(expression);

			string[] parts = path.Split('.');

			if (parts.Length > 1)
			{
				return DataBinder.Eval(model, String.Concat(parts.Take(parts.Length - 1)));
			}

			return model;
		}
	}
}
