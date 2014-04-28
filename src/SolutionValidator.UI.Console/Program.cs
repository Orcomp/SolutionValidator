// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator
{
    using System;
    using System.IO;
    using System.Linq;
    using Catel.Logging;
    using CommandLine;
    using CommandLineParsing;
    using Configuration;
    using Infrastructure;
    using Properties;
    using Validator.Common;

    internal static class Program
    {
        #region Constants
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods
        private static void Main(string[] args)
        {
            Logger.Info(Resources.Program_Main_SolutionValidator_started);

            BootStrapper.RegisterServices();

            var options = new Options();

            var parser = new Parser(with => with.HelpWriter = Console.Error);

            if (parser.ParseArgumentsStrict(args, options, () => Environment.Exit(-1)))
            {
                Run(options);
            }
        }

        private static void Run(Options options)
        {
            string repoRootPath = string.Empty;
            string configFilePath = string.Empty;



            try
            {
                var context = new Context(options.RepoRootPath)
                {
                    // TODO: Build context
                };

                context.ValidateContext();
            }
            catch (SolutionValidatorException ex)
            {
                Exit("An error occurred", -1, ex);
            }
            catch (Exception ex)
            {
                Exit("An unexpected error occurred", -2, ex);
            }





            try
            {
                if (string.Equals(options.ConfigFilePath, SolutionValidatorEnvironment.ConfigFilePathDefaultValue))
                {
                    configFilePath = null;
                }
                else
                {
                    configFilePath = Path.GetFullPath(options.ConfigFilePath);
                    if (!File.Exists(configFilePath))
                    {
                        string message = string.Format(Resources.Program_Run_Configuration_file_does_not_exist, configFilePath);
                        Exit(message, -3);
                    }
                }

                var configuration = ConfigurationHelper.Load(configFilePath);
                var ruleProcessor = new RuleProcessor(repoRootPath, configuration);

                ruleProcessor.Process(validationResult =>
                {
                    foreach (var validationMessage in validationResult.Messages.Where(vm => !vm.Processed))
                    {
                        validationMessage.Processed = true;
                        switch (validationMessage.ResultLevel)
                        {
                            case ResultLevel.Error:
                                Logger.Error(Resources.Program_Run_Error, validationMessage.Message);
                                break;

                            case ResultLevel.Warning:
                                Logger.Warning(Resources.Program_Run_Error, validationMessage.Message);
                                break;

                            case ResultLevel.Passed:
                                if (options.Verbose)
                                {
                                    Logger.Info(Resources.Program_Run_Passed, validationMessage.Message);
                                }
                                break;

                            case ResultLevel.Info:
                                Logger.Info(validationMessage.Message);
                                break;
                        }
                    }
                });

                string totalMessage = string.Format(Resources.Program_Run_Total_checks_Total_errors_found, ruleProcessor.TotalCheckCount, ruleProcessor.TotalErrorCount);
                Logger.Info(totalMessage);
                Logger.Info(Resources.Program_Run_Press_any_key_to_continue);
                Console.ReadKey(true);
                Environment.Exit(ruleProcessor.TotalErrorCount);
            }
            catch (Exception ex)
            {
                string message = string.Format(Resources.Program_Run_Unexpected_error, ex.Message);
                Logger.Error(ex, message);
                Exit(message, -4, ex);
            }
        }

        private static void Exit(string message, int exitCode, Exception e = null)
        {
            Logger.Info(message);

            if (e == null)
            {
                Logger.Error(message);
            }
            else
            {
                Logger.Error(e, message);
            }

            Logger.Info(Resources.Program_Run_SolutionValidator_exited_with_code, exitCode);
            Environment.Exit(exitCode);
        }
        #endregion
    }
}
