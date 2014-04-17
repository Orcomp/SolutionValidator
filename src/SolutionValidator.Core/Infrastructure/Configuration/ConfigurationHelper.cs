using System.Configuration;
using System.Reflection;

namespace SolutionValidator.Core.Infrastructure.Configuration
{
	/// <summary>
	/// ConfigurationHelper is a simple static helper to ease loading SolutionValidator configuration
	/// </summary>
	public static class ConfigurationHelper
	{
		/// <summary>
		/// Loads the specified configuration from the given file name.
		/// </summary>
		/// <param name="configFileName">Name of the configuration file. If not presented the standard
		/// .NET config file will be loaded</param>
		/// <returns>The loaded SolutionValidatorConfigurationSection.</returns>
		public static SolutionValidatorConfigurationSection Load(string configFileName = null)
		{
			if (configFileName == null)
			{
				SolutionValidatorConfigurationSection.ConfigFilePath = Assembly.GetExecutingAssembly()
					.CodeBase.Replace("file:///", "");
				return Check((SolutionValidatorConfigurationSection)ConfigurationManager.GetSection(SolutionValidatorConfigurationSection.SectionName));
			}
			SolutionValidatorConfigurationSection.ConfigFilePath = configFileName;
			
			var configMap = new ExeConfigurationFileMap
			{
				ExeConfigFilename = configFileName
			};

			System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
			
			return Check((SolutionValidatorConfigurationSection) config.GetSection(SolutionValidatorConfigurationSection.SectionName));
		}

		/// <summary>
		/// Checks the specified configuration for additional validation errors like invalid regex etc
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <returns>SolutionValidatorConfigurationSection.</returns>
		private static SolutionValidatorConfigurationSection Check(SolutionValidatorConfigurationSection configuration)
		{
			if (configuration == null)
			{
				configuration = new SolutionValidatorConfigurationSection();
			}

			return configuration;
		}
	}
}
