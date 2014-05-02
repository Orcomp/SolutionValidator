#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="OutpuPathProjectFileRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.ProjectFile
{
	#region using...
	using System;
	using Catel.Logging;
	using Common;

	#endregion

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
			var projectFilePaths = _projectFileHelper.GetAllProjectFilePath(repositoryInfo.RepositoryRootPath);

			foreach (var projectFilePath in projectFilePaths)
			{
				try
				{
					_projectFileHelper.LoadProject(projectFilePath);
					_projectFileHelper.CheckOutputPath(repositoryInfo.RepositoryRootPath, _expectedOutputPath, result, notify);
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
			_projectFileHelper.CheckOutputPath(repositoryInfo.RepositoryRootPath, _expectedOutputPath, result, notify);
		}
	}
}