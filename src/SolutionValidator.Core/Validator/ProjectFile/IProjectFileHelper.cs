// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProjectFileHelper.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.ProjectFile
{
    using System;
    using System.Collections.Generic;
    using Common;

    public interface IProjectFileHelper
	{				
        string GetProjectInfo(string configuration = "N/A");
		
        string GetProjectShortName();
		
        string GetPropertyValue(string propertyName);

        IEnumerable<string> GetAllProjectFilePath(string root);

        void LoadProject(string path);

        void CheckOutputPath(string repoRoot, string expectedOutputPath, ValidationResult result, Action<ValidationResult> notify = null);

        IEnumerable<string> GetConfigurations();
	}
}