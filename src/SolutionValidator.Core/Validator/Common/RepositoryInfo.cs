// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryInfo.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.Common
{
    public class RepositoryInfo
    {
        public RepositoryInfo(string rootPath)
        {
            RootPath = rootPath.Trim().Replace("/", "\\");
        }

        public string RootPath { get; private set; }
    }
}