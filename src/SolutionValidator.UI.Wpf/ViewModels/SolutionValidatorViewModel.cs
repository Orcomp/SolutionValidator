namespace SolutionValidator.ViewModels
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.MVVM.Services;
    using Common;
    using Configuration;
    using Models;

    /// <summary>
    /// Backing ViewModel for the SolutionValidatorView
    /// </summary>
    class SolutionValidatorViewModel : ViewModelBase
    {
        #region fields
        private AppSettings _appSettings;
        private readonly ISelectDirectoryService _selectDirectoryService;
        private readonly IOpenFileService _openFileService;
        private readonly ISaveFileService _saveFileService;
        private bool _isRuleEngineWorking;
        private const string DefaulProjectLocation = "No project selected";
        private const string DefaultConfigurationLocation = "Default configuration selected";
        #endregion

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionValidatorViewModel"/> class.
        /// </summary>
        public SolutionValidatorViewModel(ISelectDirectoryService selectDirectoryService, IOpenFileService openFileService, ISaveFileService saveFileService)
        {
            Argument.IsNotNull(() => selectDirectoryService);
            Argument.IsNotNull(() => openFileService);

            _selectDirectoryService = selectDirectoryService;
            _openFileService = openFileService;
            _saveFileService = saveFileService;

            CreateCommands();

            LoadAppSettings();
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

        public ICommand ExportToFileCommand { get; private set; }
        #endregion

        #region methods      
        private void CreateCommands()
        {
            SelectSettingsFileCommand = new Command(SelectSettingsFile);
            SelectProjectFolderCommand = new Command(SelectProjectFolder);
            RunCommand = new Command(Run, CanRun);
            ExportToFileCommand = new Command(ExportToFile, CanExportToFile);
        }        

        private void Run()
        {
            ValidationResults.Clear();

            var configuration = ConfigurationHelper.Load(ConfigFilePath);
            var ruleProcessor = new RuleProcessor(ProjectFileLocation, configuration, IsReformatEnabled);

            _isRuleEngineWorking = true;

            Task.Factory.StartNew(() => ruleProcessor.Process(validationResult =>
            {
                if (System.Windows.Application.Current.Dispatcher.CheckAccess())
                {
                    ValidationResults.AddRange(validationResult.Messages.Where(vm => !vm.Processed));
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => ValidationResults.AddRange(validationResult.Messages.Where(vm => !vm.Processed)));
                }
            })).ContinueWith(t => _isRuleEngineWorking = false);  
        }

        private bool CanRun()
        {
            return ProjectFileLocation != DefaulProjectLocation && !_isRuleEngineWorking;
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

        private bool CanExportToFile()
        {
            return ValidationResults.Count > 0 && !_isRuleEngineWorking;
        }

        private void ExportToFile()
        {
            if (_saveFileService.DetermineFile())
            {
                SaveResultToFile(_saveFileService.FileName);
            }
        }

        private void SaveResultToFile(string fileName)
        {
            using (var sw = new StreamWriter(fileName, false))
            {
                foreach (var result in ValidationResults)
                {
                    var stringWriter = new StringWriter();
                    stringWriter.Write(result.ResultLevel);
                    stringWriter.Write(", ");
                    stringWriter.Write(result.Message); 
                    sw.WriteLine(stringWriter.ToString());
                }

                sw.Close();
            }
        }

        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == "IsReformatEnabled")
            {
                _appSettings.Reformat = (bool)e.NewValue;
            }

            if (e.PropertyName == "ProjectFileLocation")
            {
                _appSettings.ProjectFolder = e.NewValue.ToString();
            }

            if (e.PropertyName == "ConfigFilePath")
            {
                _appSettings.ConfigFilePath = e.NewValue.ToString();
            }
        }

        private void LoadAppSettings()
        {
            _appSettings = DependencyResolver.Resolve<AppSettings>();

            ProjectFileLocation = string.IsNullOrWhiteSpace(_appSettings.ProjectFolder) ? DefaulProjectLocation : _appSettings.ProjectFolder;
            
            ConfigFilePath = string.IsNullOrWhiteSpace(_appSettings.ConfigFilePath) ? DefaultConfigurationLocation : _appSettings.ConfigFilePath;

            IsReformatEnabled = _appSettings.Reformat;
        }
        #endregion
    }
}
