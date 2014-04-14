using System;
using System.Dynamic;
using SolutionValidator.Core.Validator.Common;
using SolutionValidator.Core.Validator.FolderStructure;

namespace SolutionValidator.Core.Validator.ProjectFile.Rules
{
	public abstract class ProjectFileRule : Rule
	{
		protected readonly IProjectFileHelper projectFileHelper;
		
		protected ProjectFileRule(IProjectFileHelper projectFileHelper)
		{
			this.projectFileHelper = projectFileHelper;
		}

		// Custom Whitebox sorry...
		//public dynamic UnitTestPeek
		//{
		//	get
		//	{
		//		dynamic result = new ExpandoObject();
		//		return result;
		//	}
		//}
	}
}