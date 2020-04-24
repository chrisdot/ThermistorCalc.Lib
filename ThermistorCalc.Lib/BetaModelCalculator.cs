using System;

namespace ThermistorCalc.Lib
{

	/// <summary>
	/// Beta parameter equation (simplified Steinhart-Hart equestion model) for NTC thermistor only
	/// See https://en.wikipedia.org/wiki/Thermistor#B_or_%CE%B2_parameter_equation
	/// </summary>
	public class BetaModelCalculator : IThermistorModel
	{

		private BetaModelCalculator()
		{
		}

		public double Beta { get; private set; }
		public uint R25 { get; private set; }


		/// <summary>
		/// Creates a calculator object (generates the Beta parameter of the simplifier Steinhart-Hart equation known as Beta parameter equation)
		/// We need only 2 operating points (compared to 3 for Steinhart-Hart model) to extract the Beta parameter
		/// </summary>
		/// <param name="T1">Temperature in °C at point #1</param>
		/// <param name="R1">Resistance of the thermistance in ohms at point#1</param>
		/// <param name="T2">Temperature in °C at point #2</param>
		/// <param name="R2">Resistance of the thermistance in ohms at point#2</param>
		/// <returns>An object with the equation and the beta parameter calculated</returns>
		public static BetaModelCalculator CreateFromValues(double T1, ulong R1, double T2, ulong R2)
		{
			var coeffs = CalculateCoefficients(T1, R1, T2, R2);

			return new BetaModelCalculator() { Beta = coeffs.Beta, R25 = coeffs.R25 };
		}

		private static (double Beta, uint R25) CalculateCoefficients(double T1, ulong R1, double T2, ulong R2)
		{

			T1 = IThermistorModel.KELV_TO_DEG_OFFSET + T1;
			T2 = IThermistorModel.KELV_TO_DEG_OFFSET + T2;

			double T0 = IThermistorModel.KELV_TO_DEG_OFFSET + 25;

			double beta = -T1 * T2 * Math.Log(R1 / R2) / (T1 - T2);
			uint r25 = (uint)(R1 / (Math.Exp(-beta * (T1 - T0) / T1 / T0)));

			return (beta, r25);
		}


		public uint CalculateR(double T)
		{
			return (uint)(R25 * Math.Exp(-Beta * (T - 25) / (T + IThermistorModel.KELV_TO_DEG_OFFSET) / 298.15d));
		}

		public double CalculateT(uint R)
		{
			return 1d / (1d / (IThermistorModel.KELV_TO_DEG_OFFSET + 25) + 1d / Beta * Math.Log(R / R25)) - IThermistorModel.KELV_TO_DEG_OFFSET;
		}
	}
}
