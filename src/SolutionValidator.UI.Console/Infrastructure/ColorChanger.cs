#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorChanger.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.UI.Console.Infrastructure
{
	#region using...
	using System;

	#endregion

	internal class ColorChanger : IDisposable
	{
		#region Fields
		private readonly ConsoleColor savedForegroundColor;
		#endregion

		#region Constructors
		public ColorChanger(ConsoleColor foregroundColor)
		{
			savedForegroundColor = Console.ForegroundColor;
			Console.ForegroundColor = foregroundColor;
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			Console.ForegroundColor = savedForegroundColor;
		}
		#endregion
	}
}