// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OutPutPathProjectFileRule.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.ProjectFile.Rules
{
    using System;
    using System.Collections.Generic;
    using Catel.Logging;
    using Common;

    public class OutPutPathProjectFileRule : ProjectFileRule
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly string _expectedOutputPath;

        public OutPutPathProjectFileRule(string expectedOutputPath, IProjectFileHelper projectFileHelper)
            : base(projectFileHelper)
        {
            _expectedOutputPath = expectedOutputPath;
        }

        public override ValidationResult Validate(RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
        {
            var result = new ValidationResult(this);
            IEnumerable<string> projectFilePaths = _projectFileHelper.GetAllProjectFilePath(repositoryInfo.RootPath);

            foreach (string projectFilePath in projectFilePaths)
            {
                try
                {
                    _projectFileHelper.LoadProject(projectFilePath);
                    _projectFileHelper.CheckOutputPath(repositoryInfo.RootPath, _expectedOutputPath, result, notify);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            return result;
        }

        protected override void DoValidation(ValidationResult result, RepositoryInfo repositoryInfo, Action<ValidationResult> notify = null)
        {
            _projectFileHelper.CheckOutputPath(repositoryInfo.RootPath, _expectedOutputPath, result, notify);
        }
    }
}