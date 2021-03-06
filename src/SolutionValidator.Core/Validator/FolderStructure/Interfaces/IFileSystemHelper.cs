﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileSystemHelper.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.FolderStructure
{
	#region using...
	using System.Collections.Generic;
	using System.Text;
	using Configuration;

	#endregion

	public interface IFileSystemHelper
	{
		bool Exists(string folder, string searchPattern = null);
		IEnumerable<string> GetFolders(string root, string pattern);
		IEnumerable<string> GetFiles(string root, string pattern, IncludeExcludeCollection sourceFileFilters);
		string ReadAllText(string fileName);
		void WriteAllText(string fileName, string content, Encoding encoding);
	}
}