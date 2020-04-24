using System;

namespace ThermistorCalc.Lib
{

	/// <summary>
	///Class that manages the calculation of the Steinhart-Hart model of the resistance of a thermistor at different temperatures
	///This class extracts the Steinhart-Hart equation coefficient from 3 operating points (measurement for instance)
	///Then this class/object will be able to calculate the resistance of the thermistor given a temperature
	///This model seems applicable for both NTC and PTC thermistors 
	///References:
	/// - https://en.wikipedia.org/wiki/Steinhart%E2%80%93Hart_equation
	/// - Code heavily inspired from https://www.thinksrs.com/downloads/programs/therm%20calc/ntccalibrator/ntccalculator.html
	/// </summary>
	public class SteinhartHartModelCalculator: IThermistorModel
	{

		public double A { get; private set; }
		public double B { get; private set; }
		public double C { get; private set; }

		private SteinhartHartModelCalculator()
		{
		}

		/// <summary>
		/// Creates a calculator object (generates the A,B,C coefficients of the Steinhart-Hart equation)
		/// We need 3 operating points to extract the coefficients: (T1, R1), (T2,R2) & (T3,R3)
		/// </summary>
		/// <param name="T1">Temperature in °C at point #1</param>
		/// <param name="R1">Resistance of the thermistance in ohms at point#1</param>
		/// <param name="T2">Temperature in °C at point #2</param>
		/// <param name="R2">Resistance of the thermistance in ohms at point#2</param>
		/// <param name="T3">Temperature in °C at point #3</param>
		/// <param name="R3">Resistance of the thermistance in ohms at point#3</param>
		/// <returns>An object with the equation and the coefficients calculated</returns>
		public static SteinhartHartModelCalculator CreateFromValues(double T1, ulong R1, double T2, ulong R2, double T3, ulong R3)
		{
			var coeffs = CalculateCoefficients(T1, R1, T2, R2, T3, R3);

			return new SteinhartHartModelCalculator() { A = coeffs.A, B = coeffs.B, C = coeffs.C };
		}

		private static (double A, double B, double C) CalculateCoefficients(double T1, ulong R1, double T2, ulong R2, double T3, ulong R3)
		{
			//Convert from °C to °K
			T1 = IThermistorModel.KELV_TO_DEG_OFFSET + T1;
			T2 = IThermistorModel.KELV_TO_DEG_OFFSET + T2;
			T3 = IThermistorModel.KELV_TO_DEG_OFFSET + T3;

			if (R1 > 5 && R2 > 5 && R3 > 5 && R1 != R2 && R2 != R3 && R1 != R3 && T1 != T2)
			{
				double C = ((1 / T1 - 1 / T2) - (Math.Log(R1) - Math.Log(R2)) * (1 / T1 - 1 / T3) / (Math.Log(R1) - Math.Log(R3))) / ((Math.Pow(Math.Log(R1), 3) - Math.Pow(Math.Log(R2), 3)) - (Math.Log(R1) - Math.Log(R2)) * (Math.Pow(Math.Log(R1), 3) - Math.Pow(Math.Log(R3), 3)) / (Math.Log(R1) - Math.Log(R3)));
				double B = ((1 / T1 - 1 / T2) - C * (Math.Pow(Math.Log(R1), 3) - Math.Pow(Math.Log(R2), 3))) / (Math.Log(R1) - Math.Log(R2));
				double A = 1 / T1 - C * (Math.Log(R1)) * (Math.Log(R1)) * (Math.Log(R1)) - B * Math.Log(R1);

				return (A, B, C);
			}
			else
			{
				throw new Exception();
			}
		}

		/// <summary>
		/// Calculates a resistance given the temperature
		/// </summary>
		/// <param name="T">Temperature in °C</param>
		/// <returns>Resistance in Ohms</returns>
		public uint CalculateR(double T)
		{
			var X = (A - 1d / (T + IThermistorModel.KELV_TO_DEG_OFFSET)) / (2 * C);
			var Y = Math.Sqrt((B * B * B / C / C / C / 27) + (X * X));

			return (uint) (Math.Exp(Math.Pow((Y - X), (1d / 3d)) - Math.Pow((Y + X), (1d / 3d))));
		}


		/// <summary>
		/// Calculates the temperature given the resistance
		/// </summary>
		/// <param name="R">resistance in ohms</param>
		/// <returns>Temperature in °C</returns>
		public double CalculateT(uint R)
		{
			if (R < 1) throw new Exception();
			double Tsteinhart = 1 / (A + B * Math.Log(R) + C * Math.Pow(Math.Log(R), 3)) - IThermistorModel.KELV_TO_DEG_OFFSET;

			return Tsteinhart;
		}


	}
}
