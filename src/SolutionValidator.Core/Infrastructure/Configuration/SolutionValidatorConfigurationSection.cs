using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Properties;

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	public class SolutionValidatorConfigurationSection : ConfigurationSection
	{
		public static string ConfigFilePath { get; set; }
		public const string SectionName = "solutionValidatorConfigSection";
		private const string CheckAttributeName = "check";



		private const string FolderStructureElementName = "folderStructure";
		[ConfigurationProperty(FolderStructureElementName)]
		public FolderStructureElement FolderStructure
		{
			get { return (FolderStructureElement)base[FolderStructureElementName]; }
		}

		private const string ProjectFileElementName = "projectFile";
		[ConfigurationProperty(ProjectFileElementName)]
		public ProjectFileElement ProjectFile
		{
			get { return (ProjectFileElement)base[ProjectFileElementName]; }
		}



		[UsedImplicitly]
		public class ProjectFileElement : ConfigurationElement
		{
			private const string OutputPathElementName = "outputPath";
			[ConfigurationProperty(OutputPathElementName)]
			public OutputPathElement outputPath
			{
				get { return (OutputPathElement)base[OutputPathElementName]; }
			}

		}

		[UsedImplicitly]
		public class OutputPathElement : ConfigurationElement
		{
			private const string ValueAttributeName = "value";

			[ConfigurationProperty(ValueAttributeName, DefaultValue = "output")]
			public string Value
			{
				get { return (string) base[ValueAttributeName]; }
			}

			[ConfigurationProperty(CheckAttributeName, DefaultValue = "true")]
			public bool Check
			{
				get { return (bool) base[CheckAttributeName]; }
			}
		}




		[UsedImplicitly]
		public class FolderStructureElement : ConfigurationElement
		{
			private const string DefinitionFilePathAttributeName = "definitionFilePath";

			[ConfigurationProperty(DefinitionFilePathAttributeName, DefaultValue = ".folderStructure")]
			public string DefinitionFilePath
			{
				get { return (string)base[DefinitionFilePathAttributeName]; }
			}

			[ConfigurationProperty(CheckAttributeName, DefaultValue = "true")]			
			public bool Check
			{
				get { return (bool)base[CheckAttributeName]; }
			}

			public string EvaluatedDefinitionFilePath()
			{
				try
				{
					var folder = Path.GetDirectoryName(ConfigFilePath);
					var combine = Path.Combine(folder, DefinitionFilePath);
					return Path.GetFullPath(combine);
				}
				catch (Exception e)
				{
					Dependency.Resolve<ILogger>().Error(e, "Error getting EvaluatedDefinitionFilePath.");
					return Path.GetFullPath("Error getting EvaluatedDefinitionFilePath");
				}
			}


		}

	//	private const string ImplementationFiltersElementName = "ImplementationFilters";
	//	[ConfigurationProperty(ImplementationFiltersElementName)]
	//	public ExcludeIncludeCollection ImplementationFilters
	//	{
	//		get { return ((ExcludeIncludeCollection)(base[ImplementationFiltersElementName])); }
	//	}

	//	private const string TestCaseFiltersElementName = "TestCaseFilters";
	//	[ConfigurationProperty(TestCaseFiltersElementName)]
	//	public ExcludeIncludeCollection TestCaseFilters
	//	{
	//		get { return ((ExcludeIncludeCollection)(base[TestCaseFiltersElementName])); }
	//	}
	//}

	//[ConfigurationCollection(typeof(SearchFolder))]
	//public class SearchFolderCollection : ConfigurationElementCollection
	//{

	//	protected override ConfigurationElement CreateNewElement()
	//	{
	//		return new SearchFolder();
	//	}

	//	protected override object GetElementKey(ConfigurationElement element)
	//	{
	//		return ((SearchFolder)(element)).Folder;
	//	}

	//	public void Add(SearchFolder searchFolder)
	//	{
	//		BaseAdd(searchFolder);
	//	}


	//	public SearchFolder this[int idx]
	//	{
	//		get
	//		{
	//			return (SearchFolder)BaseGet(idx);
	//		}
	//	}
	//}

	//[ConfigurationCollection(typeof(ExcludeIncludeElement))]
	//public class ExcludeIncludeCollection : ConfigurationElementCollection
	//{

	//	protected override ConfigurationElement CreateNewElement()
	//	{
	//		return new ExcludeIncludeElement();
	//	}

	//	protected override object GetElementKey(ConfigurationElement element)
	//	{
	//		return ((ExcludeIncludeElement)(element)).Exclude + ((ExcludeIncludeElement)(element)).Include;
	//	}

	//	public void Add(ExcludeIncludeElement item)
	//	{
	//		base.BaseAdd(item);
	//	}

	//	public ExcludeIncludeElement this[int idx]
	//	{
	//		get
	//		{
	//			return (ExcludeIncludeElement)BaseGet(idx);
	//		}
	//	}
	//}	
	
	//public class SearchFolder : ConfigurationElement
	//{
	//	private const string ExcludeAttributeName = "Exclude";
	//	private const string IncludeAttributeName = "Include";
	//	private const string FolderAttributeName = "Folder";
		
	//	[ConfigurationProperty(FolderAttributeName, IsKey = true, IsRequired = true)]
	//	// MinLength has a known issue [StringValidator(InvalidCharacters = "<>|?*/", MinLength=1, MaxLength = 255)]
	//	[StringValidator(InvalidCharacters = "<>|?*/", MaxLength = 255)]
	//	public string Folder
	//	{
	//		get
	//		{
	//			return ((string)(this[FolderAttributeName]));
	//		}
	//		set
	//		{
	//			this[FolderAttributeName] = value;
	//		}
	//	}

	//	[ConfigurationProperty(ExcludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
	//	public string Exclude
	//	{
	//		get
	//		{
	//			return ((string)(this[ExcludeAttributeName]));
	//		}
	//		set
	//		{
	//			this[ExcludeAttributeName] = value;
	//		}
	//	}

	//	[ConfigurationProperty(IncludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
	//	public string Include
	//	{
	//		get
	//		{
	//			return ((string)(this[IncludeAttributeName]));
	//		}
	//		set
	//		{
	//			this[IncludeAttributeName] = value;
	//		} 
	//	}

	//}

	//public class ExcludeIncludeElement : ConfigurationElement
	//{
	//	private const string ExcludeAttributeName = "Exclude";
	//	private const string IncludeAttributeName = "Include";

	//	[ConfigurationProperty(ExcludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
	//	public string Exclude
	//	{
	//		get
	//		{
	//			return ((string)(this[ExcludeAttributeName]));
	//		}
	//		set
	//		{
	//			this[ExcludeAttributeName] = value;
	//		}
	//	}

	//	[ConfigurationProperty(IncludeAttributeName, DefaultValue = "", IsKey = false, IsRequired = false)]
	//	public string Include
	//	{
	//		get
	//		{
	//			return ((string)(this[IncludeAttributeName]));
	//		}
	//		set
	//		{
	//			this[IncludeAttributeName] = value;
	//		}
	//	}
	}

	
}
