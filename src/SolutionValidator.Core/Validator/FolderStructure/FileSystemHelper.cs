#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemHelper.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.FolderStructure
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using CodeInspection;
	using Configuration;

	#endregion

	public class FileSystemHelper : IFileSystemHelper
	{
		public bool Exists(string folder, string searchPattern = null)
		{
			if (String.IsNullOrEmpty(searchPattern))
			{
				return Directory.Exists(folder);
			}

			return Directory.GetFiles(folder, searchPattern).Length != 0;
		}

		public IEnumerable<string> GetFolders(string root, string pattern)
		{
			var result = new List<string>();
			root = root.Trim().ToLower();
			pattern = pattern.Replace('/', '\\').Trim('\\') + '\\';
			var folders = Directory.GetDirectories(root, "*", SearchOption.AllDirectories);

			var regexPattern = EscapeRegexSpecial(pattern);
			regexPattern = regexPattern.Replace(FileSystemRule.RecursionToken, @".+");
			// Not used currently: .Replace(FileSystemRule.OneLevelWildCardToken, @"[^\\]+")

			regexPattern = String.Format(@"^{0}$", regexPattern);

			foreach (var folder in folders)
			{
				var relativePart = folder.ToLower().Replace(root, "").Trim('\\') + '\\';
				if (Regex.IsMatch(relativePart, regexPattern, RegexOptions.IgnoreCase))
				{
					result.Add(folder);
				}
			}
			return result;
		}

		public IEnumerable<string> GetFiles(string root, string pattern, IncludeExcludeCollection sourceFileFilters)
		{
			root = root.Trim().ToLower().Replace('/', '\\').Trim('\\') + '\\';
			var excludeIncludeElements = sourceFileFilters.Cast<IncludeExcludeElement>();
			return Directory.GetFiles(root, pattern, SearchOption.AllDirectories).Where(fn => Filter(root, fn, excludeIncludeElements));
		}

		public string ReadAllText(string fileName)
		{
			return File.ReadAllText(fileName);
		}

		public void WriteAllText(string fileName, string content, Encoding encoding)
		{
			File.WriteAllText(fileName, content, encoding);
		}

		private static bool Filter(string root, string text, IEnumerable<IncludeExcludeElement> filters)
		{
			text = text.Replace(root, "", StringComparison.InvariantCultureIgnoreCase);
			var excludeIncludeElements = filters as IList<IncludeExcludeElement> ?? filters.ToList();

			if (excludeIncludeElements.Any(f => f.Include.Length > 0))
			{
				if (excludeIncludeElements.Where(f => f.Include.Length > 0)
					.All(f => !Regex.IsMatch(text, f.Include, RegexOptions.IgnoreCase)))
				{
					return false;
				}
			}

			if (excludeIncludeElements.Any(f => f.Exclude.Length > 0))
			{
				if (excludeIncludeElements.Where(f => f.Exclude.Length > 0)
					.Any(f => Regex.IsMatch(text, f.Exclude, RegexOptions.IgnoreCase)))
				{
					return false;
				}
			}
			return true;
		}

		private string EscapeRegexSpecial(string pattern)
		{
			var result = pattern;
			//const string regexSpecials = @".$^{}[]()|*+?\";
			const string regexSpecials = @".$^{}[]()|+?\";
			foreach (var regexSpecial in regexSpecials)
			{
				result = result.Replace(regexSpecial.ToString(CultureInfo.InvariantCulture), @"\" + regexSpecial);
			}
			return result;
		}
	}
}