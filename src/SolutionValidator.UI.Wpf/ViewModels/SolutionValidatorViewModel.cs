namespace SolutionValidator.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.MVVM.Services;
    using Common;
    using Configuration;

    /// <summary>
    /// Backing ViewModel for the SolutionValidatorView
    /// </summary>
    class SolutionValidatorViewModel : ViewModelBase
    {
        #region fields
        private readonly ISelectDirectoryService _selectDirectoryService;

        private readonly IOpenFileService _openFileService;
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionValidatorViewModel"/> class.
        /// </summary>
        public SolutionValidatorViewModel(ISelectDirectoryService selectDirectoryService, IOpenFileService openFileService)
        {
            Argument.IsNotNull(() => selectDirectoryService);
            Argument.IsNotNull(() => openFileService);

            _selectDirectoryService = selectDirectoryService;
            _openFileService = openFileService;

            CreateCommands();

            ProjectFileLocation = "No project selected";
            ConfigFilePath = "Default configuration selected";
            ValidationResults = new ObservableCollection<ValidationMessage>();
            TotalCheckCount = 0;
        }
        #endregion

        #region properties
        public string ConfigFilePath { get; set; }

        public bool IsReformatEnabled { get; set; }

        public int TotalCheckCount { get; set; }

        public int TotalErrorCount { get; set; }

        public string ProjectFileLocation { get; set; }

        public ObservableCollection<ValidationMessage> ValidationResults { get; set; }
        #endregion

        #region commands
        public Command SelectSettingsFileCommand { get; private set; }

        public Command SelectProjectFolderCommand { get; private set; }

        public Command RunCommand { get; private set; }
        #endregion

        #region methods
        private void CreateCommands()
        {
            SelectSettingsFileCommand = new Command(SelectSettingsFile);
            SelectProjectFolderCommand = new Command(SelectProjectFolder);
            RunCommand = new Command(Run, CanRun);
        }

        private void Run()
        {
            var configuration = ConfigurationHelper.Load(ConfigFilePath);
            var ruleProcessor = new RuleProcessor(ProjectFileLocation, configuration, IsReformatEnabled);

            ruleProcessor.Process(validationResult =>
            {
                foreach (var validationMessage in validationResult.Messages.Where(vm => !vm.Processed))
                {
                    ValidationResults.AddRange(validationResult.Messages);                    
                }
            });

            TotalErrorCount = ruleProcessor.TotalErrorCount;
            TotalCheckCount = ruleProcessor.TotalCheckCount;
        }

        private bool CanRun()
        {
            return ProjectFileLocation != "No project selected";
        }

        private void SelectProjectFolder()
        {
            if (_selectDirectoryService.DetermineDirectory())
            {
                ProjectFileLocation = _selectDirectoryService.DirectoryName;
            }
        }

        private void SelectSettingsFile()
        {
            if (_openFileService.DetermineFile())
            {
                ConfigFilePath = _openFileService.FileName;
            }
        }        
        #endregion
    }
}
