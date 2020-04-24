namespace ThermistorCalc.Lib
{
	interface IThermistorModel
	{
		public const double KELV_TO_DEG_OFFSET = 273.15;

		uint CalculateR(double T);
		double CalculateT(uint R);

	}
}
