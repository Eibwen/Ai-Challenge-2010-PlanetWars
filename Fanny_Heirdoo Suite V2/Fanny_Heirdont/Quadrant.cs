

using System;
public class Quadrant
{
	public double X;
	public double Y;
	internal Quadrant Calculate(int totalShipCount)
	{
		Quadrant result = new Quadrant();
		result.X = X / totalShipCount;
		result.Y = Y / totalShipCount;
		return result;
	}

	internal double Delta(Planet planet)
	{
		double dx = (planet.X - X);
		double dy = (planet.Y - Y);
		return Math.Sqrt((dx * dx) + (dy * dy));
	}
}
