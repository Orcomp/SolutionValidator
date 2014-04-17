using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolutionValidator.UI.Console.Infrastructure
{
	class ColorChanger : IDisposable
	{
		private readonly ConsoleColor savedForegroundColor;

		public ColorChanger(ConsoleColor foregroundColor)
		{
			savedForegroundColor = System.Console.ForegroundColor;
			System.Console.ForegroundColor = foregroundColor;
		}

		public void Dispose()
		{
			System.Console.ForegroundColor = savedForegroundColor;
		}
	}
}
