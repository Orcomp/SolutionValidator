// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator
{
    using System;
    using System.Linq;
    using Catel.Logging;
    using CommandLine;
    using CommandLineParsing;
    using Infrastructure;
    using Properties;
    using Validator.Common;

    internal static class Program
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods
        private static void Main(string[] args)
        {
#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            Log.Info(Resources.Program_Main_SolutionValidator_started);

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
            try
            {
                var context = new Context(options.RepoRootPath, options.ConfigFilePath);

                try
                {
                    var ruleProcessor = new RuleProcessor(context);

                    ruleProcessor.Process(validationResult =>
                    {
                        foreach (var validationMessage in validationResult.Messages.Where(vm => !vm.Processed))
                        {
                            validationMessage.Processed = true;
                            switch (validationMessage.ResultLevel)
                            {
                                case ResultLevel.Error:
                                    Log.Error(Resources.Program_Run_Error, validationMessage.Message);
                                    break;

                                case ResultLevel.Warning:
                                    Log.Warning(Resources.Program_Run_Error, validationMessage.Message);
                                    break;

                                case ResultLevel.Passed:
                                    if (options.Verbose)
                                    {
                                        Log.Info(Resources.Program_Run_Passed, validationMessage.Message);
                                    }
                                    break;

                                case ResultLevel.Info:
                                    Log.Info(validationMessage.Message);
                                    break;
                            }
                        }
                    });

                    string totalMessage = string.Format(Resources.Program_Run_Total_checks_Total_errors_found, ruleProcessor.TotalCheckCount, ruleProcessor.TotalErrorCount);

                    Log.Info(totalMessage);
                    Log.Info(Resources.Program_Run_Press_any_key_to_continue);

                    Environment.Exit(ruleProcessor.TotalErrorCount);
                }
                catch (Exception ex)
                {
                    string message = string.Format(Resources.Program_Run_Unexpected_error, ex.Message);
                    Log.Error(ex, message);
                    Exit(message, -4, ex);
                }
            }
            catch (SolutionValidatorException ex)
            {
                Exit("An error occurred", -1, ex);
            }
            catch (Exception ex)
            {
                Exit("An unexpected error occurred", -2, ex);
            }
        }

        private static void Exit(string message, int exitCode, Exception ex = null)
        {
            Log.Info(message);

            if (ex == null)
            {
                Log.Error(message);
            }
            else
            {
                Log.Error(ex, message);
            }

#if DEBUG
            WaitForKeyPress();
#endif

            Log.Info(Resources.Program_Run_SolutionValidator_exited_with_code, exitCode);
            Environment.Exit(exitCode);
        }

        private static void WaitForKeyPress()
        {
            Log.Info(string.Empty);
            Log.Info("Press any key to continue");

            Console.ReadKey();
        }
        #endregion
    }
}
