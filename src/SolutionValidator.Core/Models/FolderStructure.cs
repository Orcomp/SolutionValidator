// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderStructure.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SolutionValidator.Models
{
    using Catel.Data;
    using Catel.Logging;

    public class FolderStructure : ModelBase
    {
        #region Constants
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        public FolderStructure()
        {
            DefinitionFilePath = ".folderStructure";
            Check = true;
        }
        #endregion

        #region Properties
        public string DefinitionFilePath { get; set; }

        public bool Check { get; set; }
        #endregion
    }
}