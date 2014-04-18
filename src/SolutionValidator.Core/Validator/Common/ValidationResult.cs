using System;
using System.Collections.Generic;
using SolutionValidator.Core.Properties;

namespace SolutionValidator.Core.Validator.Common
{
	public class ValidationResult
	{
		private readonly List<ValidationMessage> messages;
		private readonly string ruleDescription;

		public ValidationResult(Rule rule)
		{
			// Currently we do not need to reference the rule itself, and hopefully we will not
			// However it sounds practical to spare the calling client to know how the complete 
			// Validation Result metainfo is prepared:
			if (rule != null)
			{
				ruleDescription = rule.ToString();
			}
			else
			{
				ruleDescription = Resources.ValidationResult_ValidationResult_Unknown_rule;
			}


			messages = new List<ValidationMessage>();
			ErrorCount = 0;
			CheckCount = 0;
		}

		public bool IsValid
		{
			get { return ErrorCount == 0; }
		}

		public string RuleDescription
		{
			get { return ruleDescription; }
		}

		public int ErrorCount { get; private set; }
		public int CheckCount { get; private set; }

		public IEnumerable<ValidationMessage> Messages
		{
			get { return messages.AsReadOnly(); }
		}

		public void AddResult(ResultLevel resultLevel, string message, Action<ValidationResult> notify = null)
		{
			if (resultLevel == ResultLevel.Error)
			{
				ErrorCount++;
			}
			CheckCount++;
			messages.Add(new ValidationMessage {ResultLevel = resultLevel, Message = message});
			if (notify != null)
			{
				notify(this);
			}
		}
	}
}