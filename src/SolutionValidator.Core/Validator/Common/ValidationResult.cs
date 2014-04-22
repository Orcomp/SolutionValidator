// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationMessage.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.Common
{
    using System;
    using System.Collections.Generic;
    using Properties;

    public class ValidationResult
    {
        private readonly List<ValidationMessage> _messages;
        private readonly string _ruleDescription;

        public ValidationResult(Rule rule)
        {
            // Currently we do not need to reference the rule itself, and hopefully we will not
            // However it sounds practical to spare the calling client to know how the complete 
            // Validation Result metainfo is prepared:
            if (rule != null)
            {
                _ruleDescription = rule.ToString();
            }
            else
            {
                _ruleDescription = Resources.ValidationResult_ValidationResult_Unknown_rule;
            }

            _messages = new List<ValidationMessage>();
            ErrorCount = 0;
            CheckCount = 0;
        }

        public bool IsValid
        {
            get { return ErrorCount == 0; }
        }

        public string RuleDescription
        {
            get { return _ruleDescription; }
        }

        public int ErrorCount { get; private set; }
        public int CheckCount { get; private set; }

        public IEnumerable<ValidationMessage> Messages
        {
            get { return _messages.AsReadOnly(); }
        }

        public void AddResult(ResultLevel resultLevel, string message, Action<ValidationResult> notify = null)
        {
            if (resultLevel == ResultLevel.Error)
            {
                ErrorCount++;
            }

            CheckCount++;
            _messages.Add(new ValidationMessage { ResultLevel = resultLevel, Message = message });

            if (notify != null)
            {
                notify(this);
            }
        }
    }
}