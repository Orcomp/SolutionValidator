// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.Models
{
    using Catel;
    using Catel.IO;

    public static class ContextExtensions
    {
        public static string GetFullPath(this Context context, string relativePath)
        {
            Argument.IsNotNull(() => context);
            Argument.IsNotNull(() => relativePath);

            return Path.Combine(context.WorkingDirectory, relativePath);
        }
    }
}