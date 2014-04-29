// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryInfo.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.Common
{
	using System.IO;

	public class RepositoryInfo
    {
        public RepositoryInfo(string repositoryRootPath)
        {
			RepositoryRootPath = repositoryRootPath.Trim().Replace("/", "\\");
        }

        public string RepositoryRootPath { get; private set; }
    }
}