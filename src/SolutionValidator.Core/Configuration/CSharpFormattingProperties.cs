#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpFormattingProperties.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

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
	using Properties;

	#endregion

	public class CSharpFormattingProperties
	{
		private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

		public string this[string key]
		{
			get
			{
				object val;
				dictionary.TryGetValue(key, out val);
				return val as string ?? string.Empty;
			}
		}

		public void Set<T>(string key, T value)
		{
			var serializedValue = Serialize(value, typeof (T), key);
			SetSerializedValue(key, serializedValue);
		}

		private void SetSerializedValue(string key, object serializedValue)
		{
			if (serializedValue == null)
			{
				return;
			}
			object oldValue;
			if (dictionary.TryGetValue(key, out oldValue))
			{
				if (Equals(serializedValue, oldValue))
				{
					return;
				}
			}
			dictionary[key] = serializedValue;
		}

		public void Save(string fileName)
		{
			new XDocument(Save()).Save(fileName);
		}

		private XElement Save()
		{
			return new XElement("Properties", SaveContents());
		}

		private IList<XElement> SaveContents()
		{
			var result = new List<XElement>();
			foreach (var pair in dictionary)
			{
				var key = new XAttribute("key", pair.Key);
				if (pair.Value is XElement)
				{
					result.Add(new XElement((XElement) pair.Value));
				}
				else
				{
					result.Add(new XElement("Property", key, (string) pair.Value));
				}
			}
			return result;
		}

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

		public object Get<T>(string key, T defaultValue, out bool found)
		{
			object value;
			if (dictionary.TryGetValue(key, out value))
			{
				try
				{
					found = true;
					return (T) Deserialize(value, typeof (T));
				}
				catch (SerializationException)
				{
					found = false;
					return defaultValue;
				}
			}
			found = false;
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
				throw new InvalidOperationException(Resources.CSharpFormattingProperties_Deserialize_Cannot_read_a_properties_container_as_a_single_value);
			}
			var c = TypeDescriptor.GetConverter(targetType);
			return c.ConvertFromInvariantString(text);
		}

		private object Serialize(object value, Type sourceType, string key)
		{
			if (value == null)
			{
				return null;
			}
			var c = TypeDescriptor.GetConverter(sourceType);
			if (c != null && c.CanConvertTo(typeof (string)) && c.CanConvertFrom(typeof (string)))
			{
				return c.ConvertToInvariantString(value);
			}

			// This element name is only because to be compatible with sharpdevelop settings
			var element = new XElement("SerializedObject");
			if (key != null)
			{
				element.Add(new XAttribute("key", key));
			}
			using (var xmlWriter = element.CreateWriter())
			{
				XamlServices.Save(xmlWriter, value);
			}
			return element;
		}

		private static CSharpFormattingProperties Load(string fileName)
		{
			var result = new CSharpFormattingProperties();

			using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				var xDocument = XDocument.Load(fileStream);
				if (xDocument.Root == null)
				{
					return result;
				}

				var csharpFormattingPropertiesElement = xDocument.Root.DescendantsAndSelf().FirstOrDefault(
					e => e.Name == "Properties"
					     && e.Attribute("key") != null
					     && e.Attribute("key").Value == "CSharpFormatting"
					);

				if (csharpFormattingPropertiesElement == null)
				{
					// Give it a shot _without the key :-)
					csharpFormattingPropertiesElement = xDocument.Root.DescendantsAndSelf()
						.FirstOrDefault(
							e => e.Name == "Properties");
				}

				return Load(csharpFormattingPropertiesElement);
			}
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

		public static CSharpFormattingOptions GetOptions(string fileName = null, CSharpFormattingOptions defaultOptions = null, FormattingOptionSet defaultOptionSet = FormattingOptionSet.VisualStudio)
		{
			var result = defaultOptions ?? CreateOptionSet(defaultOptionSet);
			if (fileName == null)
			{
				return result;
			}

			var properties = Load(fileName);
			foreach (var propertyInfo in typeof (CSharpFormattingOptions).GetProperties())
			{
				bool found;
				var value = properties.Get(propertyInfo.Name, GetDefaultValue(propertyInfo.PropertyType), out found);
				if (found && (value != null) && (value.GetType() == propertyInfo.PropertyType))
				{
					propertyInfo.SetValue(result, value, new object[0]);
				}
			}

			return result;
		}

		private static CSharpFormattingOptions CreateOptionSet(FormattingOptionSet defaultOptionSet)
		{
			switch (defaultOptionSet)
			{
				case FormattingOptionSet.Orcomp:
					return CreateOrcompOptions();
				case FormattingOptionSet.KRStyle:
					return FormattingOptionsFactory.CreateKRStyle();
				case FormattingOptionSet.Mono:
					return FormattingOptionsFactory.CreateMono();
				case FormattingOptionSet.SharpDevelop:
					return FormattingOptionsFactory.CreateSharpDevelop();
				case FormattingOptionSet.VisualStudio:
					return FormattingOptionsFactory.CreateAllman();
				case FormattingOptionSet.GNU:
					return FormattingOptionsFactory.CreateGNU();
				case FormattingOptionSet.Whitesmiths:
					return FormattingOptionsFactory.CreateWhitesmiths();
				default:
					return FormattingOptionsFactory.CreateAllman();
			}
		}

		public static CSharpFormattingOptions CreateOrcompOptions()
		{
			var options = FormattingOptionsFactory.CreateAllman();
			// TODO: Implement intended differences here like:
			// For the values are in effect in the incoming parameter, please refer to the AllmanFormattingOptions.txt in the doc
			// options.WhileNewLinePlacement = NewLinePlacement.NewLine;
			return options;
		}

		private static object GetDefaultValue(Type type)
		{
			return type.IsValueType ? Activator.CreateInstance(type) : null;
		}
	}
}