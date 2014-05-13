namespace SolutionValidator.ViewModels
{
    using System.Collections.ObjectModel;
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

            ValidationResults = new ObservableCollection<ValidationMessage>();
            TotalCheckCount = 0;
            ConfigFilePath = null;
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

            ruleProcessor.Process(ProcessValidationResult);
        }
        
        private bool CanRun()
        {
            return !string.IsNullOrWhiteSpace(ProjectFileLocation);
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

        private void ProcessValidationResult(ValidationResult validationResult)
        {
            Argument.IsNotNull(() => validationResult);

            ValidationResults.AddRange(validationResult.Messages);
            TotalErrorCount = validationResult.ErrorCount;
            TotalCheckCount = validationResult.CheckCount;
        }
        #endregion
    }
}
