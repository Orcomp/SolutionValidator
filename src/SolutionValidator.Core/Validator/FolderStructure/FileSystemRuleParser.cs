using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SolutionValidator.Core.Validator.FolderStructure
{
	public class FileSystemRuleParser
	{
		private const string RecursionTokenReplacement = "T_o@k@e_n";

		public IEnumerable<FileSystemRule> Parse(StreamReader reader)
		{
			var result = new List<FileSystemRule>();
			string line;
			int lineNumber = 1;
			try
			{
				while (null != (line = reader.ReadLine()))
				{
					FileSystemRule rule = ParseLine(line);
					if (rule != null)
					{
						result.Add(rule);
					}
					lineNumber++;
				}
			}
			catch (ParseException e)
			{
				throw new ParseException(e.Message, lineNumber, 0);
			}

			return result;
		}

		public FileSystemRule ParseLine(string line)
		{
			line = line.Trim();

			// Empty line:
			if (string.IsNullOrWhiteSpace(line))
			{
				return null;
			}
			// Comment 
			if (line.StartsWith("#"))
			{
				return null;
			}

			line = line.Replace("/", "\\");

			var checkType = CheckType.MustExist;

			if (line.StartsWith("!"))
			{
				checkType = CheckType.MustNotExist;
				line = line.TrimStart('!');
			}

			// Check if path is valid (valid does not mean exist...)
			if (!IsPathValid(line.Replace(FileSystemRule.RecursionToken, RecursionTokenReplacement)))
			{
				throw new ParseException("Invalid path", 0, 0);
			}

			if (Path.IsPathRooted(line.Replace(FileSystemRule.RecursionToken, RecursionTokenReplacement)))
			{
				throw new ParseException("RelativePath must be relative (and not rooted)", 0, 0);
			}

			string[] parts = (line + "dummy ending").Split('\\');
			if (parts.Any(p => p.Contains(FileSystemRule.RecursionToken) && p.Length != 2))
			{
				throw new ParseException(string.Format("Invalid use of '{0}' token", FileSystemRule.RecursionToken), 0, 0);
			}

			if (parts.Any(p => p.Trim().Length == 0))
			{
				throw new ParseException(string.Format("RelativePath can not contain empty parts like 'folder1\\ \\folder2."), 0, 0);
			}

			if (line.EndsWith("\\"))
			{
				return new FolderRule(line, checkType);
			}
			return new FileRule(line, checkType);
		}

		private bool IsPathValid(string path)
		{
			try
			{
				Path.GetFullPath(path);
				return true;
			}
			catch 
			{
				return false;
			}
		}
	}
}