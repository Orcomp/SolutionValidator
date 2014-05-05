#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator
{
	#region using...
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using Catel.Logging;
	using CommandLine;
	using CommandLineParsing;
	using Common;
	using Configuration;
	using Infrastructure;
	using Properties;
	using UI.Console.Infrastructure;

	#endregion

	internal static class Program
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		private static void Main(string[] args)
		{
			WriteLineBold(Resources.Program_Main_SolutionValidator_started);

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
			var repoRootPath = string.Empty;

			try
			{
				repoRootPath = Path.GetFullPath(options.RepoRootPath);
				if (!Directory.Exists(repoRootPath))
				{
					var message = string.Format(Resources.Program_Run_Repository_root_path__does_not_exist, repoRootPath);
					Exit(message, -2);
				}
			}
			catch (Exception ex)
			{
				var message = string.Format(Resources.Program_Run_Error_when_processing_Repository_root_path, options.RepoRootPath);
				Exit(message, -2, ex);
			}

			try
			{
				string configFilePath;
				if (options.ConfigFilePath == Options.ConfigFilePathDefaultValue)
				{
					configFilePath = null;
				}
				else
				{
					configFilePath = Path.GetFullPath(options.ConfigFilePath);
					if (!File.Exists(configFilePath))
					{
						var message = string.Format(Resources.Program_Run_Configuration_file_does_not_exist, configFilePath);
						Exit(message, -3);
					}
				}

				var configuration = ConfigurationHelper.Load(configFilePath);
				var ruleProcessor = new RuleProcessor(repoRootPath, configuration, options.Reformat);

				ruleProcessor.Process(validationResult =>
				{
					foreach (var validationMessage in validationResult.Messages.Where(vm => !vm.Processed))
					{
						validationMessage.Processed = true;
						switch (validationMessage.ResultLevel)
						{
							case ResultLevel.NotPassed:
							{
								using (new ColorChanger(ConsoleColor.Red))
								{
									Console.Write(Resources.Program_Run_NotPassed);
								}
								Console.WriteLine(validationMessage.Message);
							}
								break;

							case ResultLevel.Error:
								{
									using (new ColorChanger(ConsoleColor.Red))
									{
										Console.Write(Resources.Program_Run_Error);
									}
									Console.WriteLine(validationMessage.Message);
								}
								break;

							case ResultLevel.Warning:
							{
								using (new ColorChanger(ConsoleColor.Yellow))
								{
									Console.Write(Resources.Program_Run_Warning);
								}
								Console.WriteLine(validationMessage.Message);
							}
								break;

							case ResultLevel.Passed:
							{
								if (options.Verbose)
								{
									using (new ColorChanger(ConsoleColor.Green))
									{
										Console.Write(Resources.Program_Run_Passed);
									}
									Console.WriteLine(validationMessage.Message);
								}
							}
								break;

							case ResultLevel.Info:
							{
								Console.WriteLine(validationMessage.Message);
							}
								break;
						}
					}
				});

				if (!options.Reformat)
				{
					WriteLineBold(Resources.Program_Run_Code_was_not_reformatted__To_reformat_code_use_the__F_command_line_option);	
				}
				
				var totalMessage = string.Format(Resources.Program_Run_Total_checks_Total_errors_found, ruleProcessor.TotalCheckCount, ruleProcessor.TotalErrorCount);
				WriteLineBold(totalMessage);
				
				if (Debugger.IsAttached)
				{
					Console.WriteLine(Resources.Program_Run_Press_any_key_to_continue);
					Console.ReadKey(true);
				}

				Environment.Exit(ruleProcessor.TotalErrorCount);
			}
			catch (Exception ex)
			{
				var message = string.Format(Resources.Program_Run_Unexpected_error, ex.Message);
				Logger.Error(ex, message);
				Exit(message, -4, ex);
			}
		}

		private static void WriteLineBold(string totalMessage)
		{
			using (new ColorChanger(ConsoleColor.White))
			{
				Console.WriteLine(Resources.Program_Run_Bold, totalMessage);
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
	}
}