// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpFormattingProperties.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.Configuration
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Xaml;
	using System.Xml.Linq;
	using ICSharpCode.NRefactory.CSharp;

	#endregion

	public class CSharpFormattingProperties
	{
		#region Fields
		private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();
		#endregion

		#region Properties
		public string this[string key]
		{
			get
			{
				object val;
				dictionary.TryGetValue(key, out val);
				return val as string ?? string.Empty;
			}
		}
		#endregion

		#region Methods
		public T Get<T>(string key, T defaultValue)
		{
			object value;
			if (dictionary.TryGetValue(key, out value))
			{
				try
				{
					return (T) Deserialize(value, typeof (T));
				}
				catch (SerializationException)
				{
					return defaultValue;
				}
			}
			return defaultValue;
		}

		private object Deserialize(object serializedVal, Type targetType)
		{
			if (serializedVal == null)
			{
				return null;
			}
			var element = serializedVal as XElement;
			if (element != null)
			{
				using (var xmlReader = element.Elements().Single().CreateReader())
				{
					return XamlServices.Load(xmlReader);
				}
			}
			var text = serializedVal as string;
			if (text == null)
			{
				throw new InvalidOperationException("Cannot read a properties container as a single value");
			}
			var c = TypeDescriptor.GetConverter(targetType);
			return c.ConvertFromInvariantString(text);
		}

		private static CSharpFormattingProperties Load(string fileName)
		{
			var result = new CSharpFormattingProperties();
			XDocument xDocument = XDocument.Load(new FileStream(fileName, FileMode.Open, FileAccess.Read));
			if (xDocument.Root == null)
			{
				return result;
			}

			XElement csharpFormattingPropertiesElement = xDocument.Root.DescendantsAndSelf().FirstOrDefault(
				e => e.Name == "Properties"
					 && e.Attribute("key") != null
					 && e.Attribute("key").Value == "CSharpFormatting"
				);

			return Load(csharpFormattingPropertiesElement);
		}

		private static CSharpFormattingProperties Load(XElement element)
		{
			var properties = new CSharpFormattingProperties();
			properties.LoadContents(element.Elements());
			return properties;
		}

		private void LoadContents(IEnumerable<XElement> elements)
		{
			foreach (var element in elements)
			{
				var key = (string) element.Attribute("key");
				if (key == null)
				{
					continue;
				}
				dictionary[key] = new XElement(element);
			}
		}

		public static CSharpFormattingOptions GetOptions(string fileName = null, CSharpFormattingOptions defaultOptions = null)
		{
			var result = defaultOptions ?? FormattingOptionsFactory.CreateAllman();
			if (fileName == null)
			{
				return result;
			}

			var properties = Load(fileName);
			foreach (var propertyInfo in typeof (CSharpFormattingOptions).GetProperties())
			{
				var value = properties.Get(propertyInfo.Name, GetDefaultValue(propertyInfo.PropertyType));
				//if (propertyInfo.Name == "AllowOneLinedArrayInitialziers")

				if ((value != null) && (value.GetType() == propertyInfo.PropertyType))
				{
					propertyInfo.SetValue(result, value, new object[0]);
				}
			}

			return result;
		}

		private static object GetDefaultValue(Type type)
		{
			return type.IsValueType ? Activator.CreateInstance(type) : null;
		}
		#endregion
	}
}