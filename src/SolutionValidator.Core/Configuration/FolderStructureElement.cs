#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderStructureElement.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	#region using...
	using System;
	using System.Configuration;
	using System.IO;
	using Catel.Logging;
	using Properties;
	using SolutionValidator.Configuration;

	#endregion

	[UsedImplicitly]
	public class FolderStructureElement : ConfigurationElement
	{
		private const string DefinitionFilePathAttributeName = "definitionFilePath";

		/// <summary>
		/// The log.
		/// </summary>
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		[ConfigurationProperty(DefinitionFilePathAttributeName, DefaultValue = ".folderStructure")]
		public string DefinitionFilePath
		{
			get { return (string) base[DefinitionFilePathAttributeName]; }
		}

		[ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
		public bool Check
		{
			get { return (bool) base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
		}

		public string EvaluatedDefinitionFilePath()
		{
			try
			{
				var folder = Path.GetDirectoryName(SolutionValidatorConfigurationSection.ConfigFilePath);
				var combine = Path.Combine(folder, DefinitionFilePath);
				return Path.GetFullPath(combine);
			}
			catch (Exception e)
			{
				Logger.Error(e, Resources.FolderStructureElement_EvaluatedDefinitionFilePath_Error_getting_EvaluatedDefinitionFilePath);
				return
					Path.GetFullPath(DefinitionFilePath);
			}
		}
	}
}