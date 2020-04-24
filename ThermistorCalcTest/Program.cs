using System;
using ThermistorCalc.Lib;

namespace ThermistorCalcTest
{
	class Program
	{

		static void Main(string[] args)
		{
			SteinhartHartModelCalculator stHCalc = SteinhartHartModelCalculator.CreateFromValues(-35, 112800, -5, 19800, 35, 3100);
			BetaModelCalculator betaCalc = BetaModelCalculator.CreateFromValues(-35, 112800, 35, 3100);

			Console.WriteLine($"Coeffs Steinhart-Hart: A={stHCalc.A} B={stHCalc.B} C={stHCalc.C}");
			Console.WriteLine($"Coeffs Beta: Beta={betaCalc.Beta} R25={betaCalc.R25}");

			for (int t = -35; t <= 35; t++)
			{
				double R_sth = stHCalc.CalculateR(t);
				double R_b = betaCalc.CalculateR(t);
				Console.WriteLine($"T={t}(°C) \tSteinhart-Hart R={R_sth}(ohm)\tBeta R={R_b}");
			}

			Console.WriteLine("Press enter to exit");
			Console.ReadKey();
		}


	}
}
