// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Context.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator
{
    using System.IO;
    using Catel;

    public class Context
    {
        public Context(string repositoryRootPath)
        {
            Argument.IsNotNullOrWhitespace(() => repositoryRootPath);

            RepositoryRootPath = repositoryRootPath;
            
        }

        public string RepositoryRootPath { get; private set; }

        public void ValidateContext()
        {
            var repositoryPath = Path.GetFullPath(RepositoryRootPath);
            if (!Directory.Exists(repositoryPath))
            {
                throw new SolutionValidatorException("Repository path '{0}' does not exist", repositoryPath);
            }
        }
    }
}