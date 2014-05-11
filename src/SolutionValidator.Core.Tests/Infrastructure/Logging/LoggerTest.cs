#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggerTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests.Logging
{
	#region using...
	using System;
	using System.Diagnostics;
	using System.IO;
	using Catel.Logging;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class LoggerTest
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		// [assembly: log4net.Config.XmlConfigurator(ConfigFile = "SolutionValidator.log4net.config", Watch = true)]
		// placed in SolutionValidator.Core assembly, now testing if simply referencing it makes the logging work.
		// NOTE: The SolutionValidator.log4net.config project item is configured to be copied to the output folder when building
		[Test]
		public void ConfigurationTest()
		{
			var testMessage = string.Format("Test message: {0}", DateTime.Now.Ticks);
			const string logFileName = @"SolutionValidator.log";
			const string logFileCopyName = @"logCopy.txt";

			try
			{
				File.Delete(logFileCopyName);
			}
			catch (FileNotFoundException)
			{
			}

			// Catel does not write messages to log directly, but flushes to the log after 50 entries, or some elapsed time
			// So to make this test work, i just added 50= items so everything is flushed to the log.
			for (var i = 0; i <= 50; i++)
			{
				Logger.Warning(testMessage);
			}

			File.Copy(logFileName, logFileCopyName);

			var loggedText = File.ReadAllText(logFileCopyName);
			Trace.WriteLine(loggedText);
			Trace.WriteLine(testMessage);

			Assert.IsTrue(loggedText.Contains(testMessage));
		}
	}
}