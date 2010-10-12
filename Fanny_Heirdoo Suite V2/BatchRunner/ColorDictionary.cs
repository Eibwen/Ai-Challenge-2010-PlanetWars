using System.Collections.Generic;
using System.Drawing;


public class ColorDictionary : Dictionary<RenderColor, Color>
{
	Dictionary<RenderColor, Pen> MyPencils = new Dictionary<RenderColor, Pen>();
	Dictionary<RenderColor, Brush> MyBrushes = new Dictionary<RenderColor, Brush>();
	bool PensAndBrushesCreated;
	/// <summary>
	/// Creates the pens and brushes.
	/// </summary>
	public void CreatePensAndBrushes()
	{
		if (!PensAndBrushesCreated)
		{
			foreach (var item in this)
			{
				SolidBrush myBrush = new SolidBrush(item.Value);
				MyBrushes.Add(item.Key, myBrush);
				MyPencils.Add(item.Key, new Pen(myBrush));
			}
		}
		PensAndBrushesCreated = true;
	}

	public void ResetClear()
	{
		foreach (Pen pencil in MyPencils.Values)
			pencil.Dispose();
		foreach (Brush myBrush in MyBrushes.Values)
			myBrush.Dispose();
		MyBrushes.Clear();
		MyPencils.Clear();
		PensAndBrushesCreated = false;
		Clear();
	}

	public Pen GetPen(RenderColor color)
	{
		CreatePensAndBrushes();
		return MyPencils[color];
	}

	public Brush GetBrush(RenderColor color)
	{
		CreatePensAndBrushes();
		return MyBrushes[color];
	}


	internal RenderColor GetDarkColor(int player)
	{
		return (RenderColor)player + 3;
	}
	internal RenderColor GetLightColor(int player)
	{
		return (RenderColor)player;
	}

	internal Brush GetDarkBrush(int player)
	{
		return GetBrush(GetDarkColor(player));
	}

	internal Brush GetLightBrush(int player)
	{
		return GetBrush(GetLightColor(player));
	}

	internal Pen GetDarkPen(int player)
	{
		return GetPen(GetDarkColor(player));
	}

	internal Pen GetLightPen(int player)
	{
		return GetPen(GetLightColor(player));
	}
}
