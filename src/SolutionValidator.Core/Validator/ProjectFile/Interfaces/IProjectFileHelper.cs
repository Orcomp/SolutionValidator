#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IProjectFileHelper.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.ProjectFile
{
	#region using...
	using System;
	using System.Collections.Generic;
	using Common;

	#endregion

	public interface IProjectFileHelper
	{
		string GetProjectInfo();
		string GetProjectInfo(string configuration);

		string GetProjectShortName();

		string GetPropertyValue(string propertyName);

		IEnumerable<string> GetAllProjectFilePath(string root);

		void LoadProject(string path);

		void CheckOutputPath(string repoRoot, string expectedOutputPath, ValidationResult result, Action<ValidationResult> notify = null);

		IEnumerable<string> GetConfigurations();
	}
}