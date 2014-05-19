namespace SolutionValidator.Models
{
    using Helpers;

    class AppSettings
    {
        private const string ReformatRegistryKey = "Reformat";
        private const string ProjectFolderRegistryKey = "ProjectFolder";
        private const string ConfigFilePathRegistryKey = "ConfigFilePath";
        
        private const string Path = @"SOFTWARE\SolutionValidator";

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        public AppSettings()
        {            
            LoadAppSettings();
        }

        public string ConfigFilePath { get; set; }

        public string ProjectFolder { get; set; }

        public bool Reformat { get; set; }

        public void LoadAppSettings()
        {
            ProjectFolder = (string)RegistryHelper.ReadUserValue(Path, ProjectFolderRegistryKey);
            ConfigFilePath = (string)RegistryHelper.ReadUserValue(Path, ConfigFilePathRegistryKey);

            var reformat = (string)RegistryHelper.ReadUserValue(Path, ReformatRegistryKey);    

            if (!string.IsNullOrWhiteSpace(reformat))
            {
                Reformat = bool.Parse(reformat);
            }
        }

        public void SaveAppSettings()
        {
            RegistryHelper.WriteUserValue(Path, ProjectFolderRegistryKey, ProjectFolder);
            RegistryHelper.WriteUserValue(Path, ConfigFilePathRegistryKey, ConfigFilePath);
            RegistryHelper.WriteUserValue(Path, ReformatRegistryKey, Reformat.ToString());
        }
    }
}
