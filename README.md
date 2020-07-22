# ThermistorCalc.Lib
This is a C#/.Net standard library designed to calculate thermistor resistance depending to the temperature.
This library features both Steinhart-Hart and Beta models. See [dedicated wikipedia page](https://en.wikipedia.org/wiki/Steinhart%E2%80%93Hart_equation) for more information.

I came up with this solution only because excel could not handle that, due to the lack of floating point precision (it seems, I didn't investigate further as I was already upset :-)). 
My initial goal was to generate an embeddable conversion tabel from resistance to temperature into an arduino program.

## Steinhart-Hart model
For the Steinhart-Hart model, you need 3 operation points (means 3 resistance/temparature value pairs)

```
//Creation of the Steinhart-Hart model requiring 3 operation points
SteinhartHartModelCalculator stHCalc = SteinhartHartModelCalculator.CreateFromValues(-35, 112800, -5, 19800, 35, 3100);
//calculate resistance for 25°C
double R_sth = stHCalc.CalculateR(25);
```

## Beta model
The Beta model is a less accurate one, and may be sufficient in many cases. You only need 2 operation points.

```
//Creation of the Beta model requiring only 2 operation points
BetaModelCalculator betaCalc = BetaModelCalculator.CreateFromValues(-35, 112800, 35, 3100);
//calculate resistance for 25°C
double R_b = betaCalc.CalculateR(25);
```
