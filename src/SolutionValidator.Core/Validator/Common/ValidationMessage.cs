// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationMessage.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.Common
{
    public class ValidationMessage
    {
        public ResultLevel ResultLevel { get; set; }
        public string Message { get; set; }
        public bool Processed { get; set; }
    }
}