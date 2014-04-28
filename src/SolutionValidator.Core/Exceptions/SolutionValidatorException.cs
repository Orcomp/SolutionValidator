// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SolutionValidatorException.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator
{
    using System;

    public class SolutionValidatorException : Exception
    {
        public SolutionValidatorException(string message, params object[] args)
            : base(string.Format(message, args))
        {
            
        }
    }
}