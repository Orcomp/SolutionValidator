// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatorContext.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Models
{
    using Catel.Data;

    public class ValidatorContext : ModelBase
    {
        public ValidatorContext()
        {            
            FolderStructure = new FolderStructure();
            ProjectFile = new ProjectFile();
        }

        public FolderStructure FolderStructure { get; private set; }
        
        public ProjectFile ProjectFile { get; private set; }        
    }
}