#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpFormattingElement.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Configuration
{
	#region using...
	using System;
	using System.Configuration;
	using System.IO;
	using Catel.Logging;
	using ICSharpCode.NRefactory.CSharp;
	using Properties;

	#endregion

	[UsedImplicitly]
	public class CSharpFormattingElement : ConfigurationElement
	{
		private const string OptionsFilePathDefaultValue = "csharpformatting.xml";
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		private const string OptionsFilePathAttributeName = "optionsFilePath";
		[ConfigurationProperty(OptionsFilePathAttributeName, DefaultValue = OptionsFilePathDefaultValue)]
		public string OptionsFilePath
		{
			get { return (string) base[OptionsFilePathAttributeName]; }
		}

		private const string DefaultFormattingOptionSetAttributeName = "DefaultFormattingOptionSetName";		
		[ConfigurationProperty(DefaultFormattingOptionSetAttributeName, DefaultValue = "Orcomp")]
		public string DefaultFormattingOptionSetName
		{
			get { return (string) base[OptionsFilePathAttributeName]; }
		}

		private const string SourceFileFiltersElementName = "sourceFileFilters";
		[ConfigurationProperty(SourceFileFiltersElementName)]
		public IncludeExcludeCollection SourceFileFilters
		{
			get { return ((IncludeExcludeCollection)(base[SourceFileFiltersElementName])); }
		}

		private const string CodeMemberOrderElementName = "codeMemberOrder";
		[ConfigurationProperty(CodeMemberOrderElementName)]
		public CodeMemberOrderCollection CodeMemberOrder
		{
			get { return ((CodeMemberOrderCollection)(base[CodeMemberOrderElementName])); }
		}



		public FormattingOptionSet DefaultFormattingOptionSet
		{
			get
			{
				FormattingOptionSet result;
				if (Enum.TryParse(DefaultFormattingOptionSetName, true, out result))
				{
					return result;
				}
				return FormattingOptionSet.VisualStudio;
			}
		}

		[ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
		public bool Check
		{
			get { return (bool) base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
		}

		public bool IsDefaultOptionsFilePath
		{
			get { return OptionsFilePathDefaultValue.Equals(OptionsFilePath); }
		}

		public string EvaluatedOptionsFilePath()
		{
			try
			{
				var folder = Path.GetDirectoryName(SolutionValidatorConfigurationSection.ConfigFilePath);
				var combine = Path.Combine(folder, OptionsFilePath);
				return Path.GetFullPath(combine);
			}
			catch (Exception e)
			{
				Logger.Error(e, Resources.FolderStructureElement_EvaluatedDefinitionFilePath_Error_getting_EvaluatedDefinitionFilePath);
				return Path.GetFullPath(OptionsFilePath);
			}
		}
	}

	[ConfigurationCollection(typeof(GeneratedCodeMemberElement))]
	public class CodeMemberOrderCollection : ConfigurationElementCollection
	{
		public GeneratedCodeMemberElement this[int idx]
		{
			get { return (GeneratedCodeMemberElement)BaseGet(idx); }
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new GeneratedCodeMemberElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return element.ToString();
		}

		public void Add(GeneratedCodeMemberElement item)
		{
			base.BaseAdd(item);
		}
	}

	public class GeneratedCodeMemberElement : ConfigurationElement
	{
		private const string MemberAttributeName = "member";
		[ConfigurationProperty(MemberAttributeName)]
		public GeneratedCodeMember Member
		{
			get { return (GeneratedCodeMember)base[MemberAttributeName]; }
		}

	}
}