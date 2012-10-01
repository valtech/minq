using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	public static class SItemLocalization
	{
		public static SField Field(this SItem source, string name, IEnumerable<string> languageFallback)
		{
			if (languageFallback != null && languageFallback.Any())
			{
				SField field = source.Field(name);

				if (!SField.IsNullOrEmpty(field) && field.FieldType == typeof(bool))
				{
					SItem localizedItem = source.Db.Item(source.Guid, languageFallback);

					if (localizedItem != null)
					{
						return localizedItem.Field(name);
					}
				}
				else
				{
					return LanguageFallbackFields(source, name, languageFallback).FirstOrDefault();
				}
			}

			return source.Field(name);
		}

		private static IEnumerable<SField> LanguageFallbackFields(this SItem source, string name, IEnumerable<string> languageFallback)
		{
			bool anyLanguages = false;

			bool foundNonEmptyField = false;

			SField fieldInItemLanguage = null;

			foreach (string languageName in languageFallback)
			{
				if (!anyLanguages)
				{
					anyLanguages = true;
				}

				SField field = null;

				if (source.LanguageName == languageName)
				{
					field = fieldInItemLanguage = source.Field(name);
				}
				else
				{
					SItem item = source.Db.Item(source.Guid, languageName);

					if (item != null)
					{
						field = item.Field(name);
					}
				}

				if (field != null)
				{
					object value = field.Value(typeof(object), null);

					//requires bool? check to allow empty tristate to climb to next language in chain
					if (value != null && ((field.FieldType != typeof(string) && field.FieldType != typeof(bool?)) || !String.IsNullOrEmpty((string)field.Value(typeof(string), ""))))
					{
						foundNonEmptyField = true;

						yield return field;
					}
				}
			}

			if (!foundNonEmptyField)
			{
				if (fieldInItemLanguage != null)
				{
					//if no field was found and the language for the item was included in the chain then return the empty field
					yield return fieldInItemLanguage;
				}
			}
			

			if (!anyLanguages)
			{
				yield return source.Field(name);
			}
		}
	}
}
