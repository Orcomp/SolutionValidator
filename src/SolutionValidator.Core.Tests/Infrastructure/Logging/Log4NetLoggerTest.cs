using System;
using System.Diagnostics;
using System.IO;
using Catel.Logging;
using NUnit.Framework;

namespace SolutionValidator.Core.Tests.Infrastructure.Logging
{
	[TestFixture]
	public class Log4NetLoggerTest
	{
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		// [assembly: log4net.Config.XmlConfigurator(ConfigFile = "SolutionValidator.log4net.config", Watch = true)]
		// placed in SolutionValidator.Core assembly, now testing if simply referencing it makes the logging work.
		// NOTE: The SolutionValidator.log4net.config project item is configured to be copied to the output folder when building
		[Test]
		public void ConfigurationTest()
		{
			string testMessage = string.Format("Test message: {0}", DateTime.Now.Ticks);
			const string logFileName = @"SolutionValidator.log";
			const string logFileCopyName = @"logCopy.txt";

			try
			{
				File.Delete(logFileCopyName);
			}
			catch (FileNotFoundException)
			{
			}

			Logger.Warning(testMessage);   
         
			File.Copy(logFileName, logFileCopyName);

			string loggedText = File.ReadAllText(logFileCopyName);
			Trace.WriteLine(loggedText);
			Trace.WriteLine(testMessage);

			Assert.IsTrue(loggedText.Contains(testMessage));
		}
	}
}