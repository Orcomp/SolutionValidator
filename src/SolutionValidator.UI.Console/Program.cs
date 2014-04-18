using System;
using System.IO;
using System.Linq;
using CommandLine;
using SolutionValidator.Core.Infrastructure.Configuration;
using SolutionValidator.Core.Infrastructure.DependencyInjection;
using SolutionValidator.Core.Infrastructure.Logging;
using SolutionValidator.Core.Validator.Common;
using SolutionValidator.UI.Console.CommanLineParsing;
using SolutionValidator.UI.Console.Infrastructure;
using SolutionValidator.UI.Console.Properties;

namespace SolutionValidator.UI.Console
{
	internal static class Program
	{
		private static ILogger logger;

		private static void Main(string[] args)
		{
			BootStrapper.CreateKernel();
			logger = Dependency.Resolve<ILogger>();
			logger.Info(Resources.Program_Main_SolutionValidator_started);

			var options = new Options();

			var parser = new Parser(with => with.HelpWriter = System.Console.Error);

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
				repoRootPath = Path.GetFullPath(options.RepoRootPath);
				if (!Directory.Exists(repoRootPath))
				{
					string message = string.Format(Resources.Program_Run_Repository_root_path__does_not_exist, repoRootPath);
					Exit(message, -2);
				}
			}
			catch (Exception e)
			{
				string message = string.Format(Resources.Program_Run_Error_when_processing_Repository_root_path,
					options.RepoRootPath);
				Exit(message, -2, e);
			}

			try
			{
				if (options.ConfigFilePath == Options.ConfigFilePathDefaultValue)
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

				SolutionValidatorConfigurationSection configuration = ConfigurationHelper.Load(configFilePath);
				var ruleProcessor = new RuleProcessor(repoRootPath, configuration, logger);

				ruleProcessor.Process(validationResult =>
				{
					foreach (ValidationMessage validationMessage in validationResult.Messages.Where(vm => !vm.Processed))
					{
						validationMessage.Processed = true;
						switch (validationMessage.ResultLevel)
						{
							case ResultLevel.Error:
							{
								using (new ColorChanger(ConsoleColor.Red))
								{
									System.Console.WriteLine(Resources.Program_Run_Error, validationMessage.Message);
								}
							}
								break;
							
							case ResultLevel.Warning:
							{
								using (new ColorChanger(ConsoleColor.Yellow))
								{
									System.Console.WriteLine(Resources.Program_Run_Error, validationMessage.Message);
								}
							}
								break;
							
							case ResultLevel.Passed:
							{
								if (options.Verbose)
								{
									using (new ColorChanger(ConsoleColor.Green))
									{
										System.Console.WriteLine(Resources.Program_Run_Passed, validationMessage.Message);
									}
								}
							}
								break;
							
							case ResultLevel.Info:
							{
								System.Console.WriteLine(validationMessage.Message);
							}
								break;
						}
					}
				});

				string totalMessage = string.Format(Resources.Program_Run_Total_checks_Total_errors_found,
					ruleProcessor.TotalCheckCount,
					ruleProcessor.TotalErrorCount);

				System.Console.WriteLine(totalMessage);
				logger.Info(totalMessage);
				System.Console.WriteLine(Resources.Program_Run_Press_any_key_to_continue);
				System.Console.ReadKey(true);
				Environment.Exit(ruleProcessor.TotalErrorCount);
			}
			catch (Exception e)
			{
				string message = string.Format(Resources.Program_Run_Unexpected_error, e.Message);
				logger.Error(e, message);
				Exit(message, -4, e);
			}
		}

		private static void Exit(string message, int exitCode, Exception e = null)
		{
			System.Console.Error.WriteLine(message);
			if (e == null)
			{
				logger.Error(message);
			}
			else
			{
				logger.Error(e, message);
			}
			logger.Info(Resources.Program_Run_SolutionValidator_exited_with_code, exitCode);
			Environment.Exit(exitCode);
		}
	}
}