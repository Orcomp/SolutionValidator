// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileRule.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.ProjectFile.Rules
{
    using System;
    using System.Collections.Generic;
    using Catel.Logging;
    using Common;

    public abstract class ProjectFileRule : Rule
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        protected const string EmptyPropertyName = "<empty>";
        protected readonly IProjectFileHelper _projectFileHelper;

        protected ProjectFileRule(IProjectFileHelper projectFileHelper)
            : base()
        {
            _projectFileHelper = projectFileHelper;
        }

        // Custom Whitebox sorry...
        //public dynamic UnitTestPeek
        //{
        //	get
        //	{
        //		dynamic result = new ExpandoObject();
        //		return result;
        //	}
        //}
        public override ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
        {
            var result = new ValidationResult(this);
            IEnumerable<string> projectFilePaths = _projectFileHelper.GetAllProjectFilePath(repositoryInfo.RootPath);

            foreach (string projectFilePath in projectFilePaths)
            {
                try
                {
                    _projectFileHelper.LoadProject(projectFilePath);
                    DoValidation(result, repositoryInfo, notify);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            return result;
        }

        protected abstract void DoValidation(ValidationResult result, RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null);
    }
}