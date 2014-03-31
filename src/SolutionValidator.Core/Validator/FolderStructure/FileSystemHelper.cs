using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;


namespace SolutionValidator.Core.Validator.FolderStructure
{
	
	public class FileSystemHelper : IFileSystemHelper
	{
		#region IFileSystemHelper Members

		public bool Exists(string folder, string searchPattern = null)
		{
			if (string.IsNullOrEmpty(searchPattern))
			{
				return Directory.Exists(folder);
			}

			return Directory.GetFiles(folder, searchPattern).Length != 0;
		}

		public IEnumerable<string> GetFolders(string root, string pattern)
		{
			var result = new List<string>();
			root = root.Trim().ToLower();
			pattern = pattern.Replace('/','\\').Trim('\\');
			string[] folders = Directory.GetDirectories(root, "*", SearchOption.AllDirectories);
			
			string regexPattern = EscapeRegexSpecial(pattern);
			regexPattern = regexPattern
				.Replace(@"\\", @"\")
				.Replace(FileSystemRule.RecursionToken, @".+")
				// Not used currently: .Replace(FileSystemRule.OneLevelWildCardToken, @"[^\\]+")
				;
			
			regexPattern = string.Format("^{0}$", regexPattern);

			foreach (string folder in folders)
			{
				string relativePart = folder.ToLower().Replace(root, "").Trim('\\') + '\\';
				if (Regex.IsMatch(relativePart, regexPattern, RegexOptions.IgnoreCase))
				{
					result.Add(folder);
				}
				else
				{
					var i = 0;
				}
			}
			return result;
		}

		#endregion

		private string EscapeRegexSpecial(string pattern)
		{
			string result = pattern;
			//const string regexSpecials = @".$^{}[]()|*+?\";
			const string regexSpecials = @".$^{}[]()|+?\";
			foreach (char regexSpecial in regexSpecials)
			{
				result = result.Replace(regexSpecial.ToString(), @"\" + regexSpecial);
			}
			return result;
		}
	}
}