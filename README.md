# ThermistorCalc.Lib
This is a C#/.Net standard library designed to calculate thermistor resistance depending to the temperature.
This library features both SteinhartHart and Beta models. See [dedicated wikipedia page](https://en.wikipedia.org/wiki/Steinhart%E2%80%93Hart_equation) for more information.

## Steinhart-Hart model
For the Steinhart-Hart model, you need 3 operation points (means 3 resistance/temparature value pairs)

```
//Creation of the Steinhart-Hart model requiring 3 operation points
SteinhartHartModelCalculator stHCalc = SteinhartHartModelCalculator.CreateFromValues(-35, 112800, -5, 19800, 35, 3100);
//calculate resistance for a given temperature
double R_sth = stHCalc.CalculateR(25);
```

## Beta model
The Beta model is a less accurate one, and may be sufficient in many cases. You only need 2 operation points.

```
//Creation of the Beta model requiring only 2 operation points
BetaModelCalculator betaCalc = BetaModelCalculator.CreateFromValues(-35, 112800, 35, 3100);
double R_b = betaCalc.CalculateR(25);
```
