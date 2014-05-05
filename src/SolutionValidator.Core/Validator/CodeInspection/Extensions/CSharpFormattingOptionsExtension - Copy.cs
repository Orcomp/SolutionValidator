#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpFormattingOptionsExtension.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection
{
	#region using...
	using System;
	using System.Text;
	using Catel.Text;
	using ICSharpCode.NRefactory.CSharp;

	#endregion

	public static class CSharpFormattingOptionsExtension
	{
		public static string Dump(this CSharpFormattingOptions @this)
		{
			var result = new StringBuilder();
			var index = 1;
			foreach (var propertyInfo in typeof (CSharpFormattingOptions).GetProperties())
			{
				if (propertyInfo.Name.Equals("Name") || propertyInfo.Name.Equals("IsBuiltIn"))
				{
					continue;
				}

				var values = string.Empty;
				if (propertyInfo.PropertyType.IsEnum)
				{
					values = string.Join(", ", Enum.GetNames(propertyInfo.PropertyType));
				}
				if (!string.IsNullOrEmpty(values))
				{
					values = string.Format("({0})", values);
				}

				result.AppendLine("{0:###}. {1}: {2} {3}", index++, propertyInfo.Name, propertyInfo.GetValue(@this, new object[0]), values);
			}
			return result.ToString();
		}
	}
}