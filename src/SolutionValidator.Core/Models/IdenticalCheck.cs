#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IdenticalCheck.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Models
{
	#region using...
	using Catel.Data;

	#endregion

	public class IdenticalCheck : ModelBase

	{
		public string PropertyName { get; set; }

		public string OtherPropertyName { get; set; }
	}
}