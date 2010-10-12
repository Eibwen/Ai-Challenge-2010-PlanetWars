using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;


#region Porter classes, I use these classes so I have to change as little as possible in the code itself.
/* I have tried to change as little as possible, trying to make future ports easy.
 * 
 * This meant making strange wrapper classes just to work around naming convention differences
 * I copied some code (Winner for example) from the sample classes, those were pre-ported.
 * 
 * To get a list of all the tricks I pulled for the conversion, look for //PORT
 */

 
public class Map<Tkey, Tvalue> : Dictionary<Tkey, Tvalue>
{

	internal void put(Tkey key, Tvalue value)
	{
		Add(key, value);
	}

	internal Tvalue get(Tkey index)
	{
		return this[index];
	}
}
public class TreeMap<Tkey, Tvalue> : Map<Tkey, Tvalue> { }

public class IntegerMapper
{
	public int parseInt(string value)
	{
		return int.Parse(value, CultureInfo.InvariantCulture);
	}
}
public class DoubleMapper
{
	public Double MAX_VALUE = Double.MaxValue;
	public Double MIN_VALUE = Double.MinValue;

	internal double parseDouble(string value)
	{
		return double.Parse(value, CultureInfo.InvariantCulture);
	}
}
 
#endregion

