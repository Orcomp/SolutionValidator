// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileSystemHelper.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.FolderStructure
{
    using System.Collections.Generic;

    public interface IFileSystemHelper
    {
        bool Exists(string folder, string searchPattern = null);
        IEnumerable<string> GetFolders(string root, string pattern);
    }
}