#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="OutputLogListener.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Logging
{
	#region using...
	using Catel.Logging;

	#endregion

	public class OutputLogListener : FileLogListener
	{
		public OutputLogListener()
		{
			IgnoreCatelLogging = true;
			IsDebugEnabled = true;
		}

		protected override string FormatLogEvent(ILog log, string message, LogEvent logEvent, object extraData)
		{
			return message;
		}
	}
}