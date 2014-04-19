// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorChanger.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Infrastructure
{
    using System;

    class ColorChanger : IDisposable
	{
		private readonly ConsoleColor savedForegroundColor;

		public ColorChanger(ConsoleColor foregroundColor)
		{
			savedForegroundColor = System.Console.ForegroundColor;
			Console.ForegroundColor = foregroundColor;
		}

		public void Dispose()
		{
			Console.ForegroundColor = savedForegroundColor;
		}
	}
}
