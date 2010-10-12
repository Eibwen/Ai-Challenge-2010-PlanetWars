using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using PlanetDebug;
using botDebug;

namespace BatchRunner
{
	public class Renderer : Panel
	{
		DoubleMapper Double = new DoubleMapper();

		public Renderer()
		{
			BarSpacing = 2;
			BarWidth = 6;
			DoubleBuffered = true;
			DrawGrid = true;
			DrawAttacklines = true;
			DrawFleetArrival = true;
			DrawPlanetStatistics = true;
			DrawUniverseStatistics = true;
			DrawPlayerOne = true;
			DrawPlayerTwo = true;
		}


		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// do nothing, stops flickering.
		}

		//PORT: some of these are disposable under widows, which means you need to clean them up.
		//by moving them out of the render function they leak a lot less, only once per instance.
		Font font100 = new Font("Courier New", 16, FontStyle.Regular);
		Font font75 = new Font("Courier Newf", 14, FontStyle.Regular);
		Font font50 = new Font("Courier New", 10, FontStyle.Regular);
		Font font25 = new Font("Courier New", 8, FontStyle.Regular);

		Font font100b = new Font("Courier New", 16, FontStyle.Bold);
		Font font75b = new Font("Courier New", 14, FontStyle.Bold);
		Font font50b = new Font("Courier New", 10, FontStyle.Bold);
		Font font25b = new Font("Courier New", 8, FontStyle.Bold);

		public bool DrawGrid { get; set; }
		public bool DrawAttacklines { get; set; }
		public bool DrawPlanetStatistics { get; set; }
		public bool DrawFleetArrival { get; set; }
		public bool DrawUniverseStatistics { get; set; }
		public bool DrawPlayerTwo { get; set; }


		public int BarSpacing { get; set; }
		public int BarWidth { get; set; }

		public bool DrawPlayerOne { get; set; }


		private Graphics _Canvas;
		private ColorDictionary MyColors;

		/// <summary>
		/// Gets or sets the shortes route.
		/// </summary>
		/// <value>The shortes route.</value>
		public IEnumerable<IPlanet> ShortestRoute { get; set; }
		// Renders the current state of the game to a graphics object
		//
		// The offset is a number between 0 and 1 that specifies how far we are
		// past this game state, in units of time. As this parameter varies from
		// 0 to 1, the fleets all move in the forward direction. This is used to
		// fake smooth animation.
		//
		// On success, return an image. If something goes wrong, returns null.
		public void Render(Game gameData, Image bgImage, // Background image
					ColorDictionary colors, Graphics canvas, botDebugBase getPlayerStats)
		{

			_Canvas = canvas;
			MyColors = colors;
			try
			{
				int width = (int)(Width * 0.9);
				int height = (int)(Height * 0.9);
				PointF offset = new PointF(Width - width - 30, Height - height - 15);

				List<Planet> planets = gameData.Planets;
				List<Fleet> fleets = gameData.Fleets;

				if (bgImage != null)
				{
					_Canvas.DrawImage(bgImage, 0, 0);
				}
				else
				{
					_Canvas.FillRectangle(Brushes.LightGray, _Canvas.ClipBounds);
				}
				// Determine the dimensions of the viewport in game coordinates.
				double _top = Double.MAX_VALUE;
				double _left = Double.MAX_VALUE;
				double _right = Double.MIN_VALUE;
				double _bottom = Double.MIN_VALUE;
				int maxGrowthRate = 0;
				foreach (Planet p in planets)
				{
					if (p.X < _left) _left = p.X;
					if (p.X > _right) _right = p.X;
					if (p.Y > _bottom) _bottom = p.Y;
					if (p.Y < _top) _top = p.Y;
					if (p.GrowthRate > maxGrowthRate) maxGrowthRate = p.GrowthRate;
				}
				_left--; _right++; _top--; _bottom++;

				int _xRange = (int)_right - (int)_left;
				int _yRange = (int)_bottom - (int)_top;

				PointF sizePerUnit = new PointF((float)width / (_xRange),
												 (float)height / (_yRange));

				double minSizeFactor = 5.0 + (sizePerUnit.X / maxGrowthRate);

				Point[] planetPos = new Point[planets.Count];


				RectangleF bounds = new RectangleF((float)(offset.X), (float)(offset.Y), (float)width, (float)height);

				PointF origin = new PointF((float)(bounds.Left - (_left * sizePerUnit.X)),
										   (float)(bounds.Top - (_top * sizePerUnit.Y)));

				if (DrawGrid)
				{
					for (float iGridX = (int)_left; iGridX <= _xRange; iGridX++)
					{
						float newX = origin.X + (iGridX * sizePerUnit.X);
						PointF gridTop = new PointF(newX, bounds.Top);
						PointF gridBottom = new PointF(newX, bounds.Bottom);

						_Canvas.DrawLine(MyColors.GetPen(RenderColor.GridLine), gridTop, gridBottom);

						string linesString = iGridX.ToString();

						WriteTextInElLipseWithBorder(RenderColor.GridDark, RenderColor.GridLight, linesString, font50, gridTop, true, false);
						WriteTextInElLipseWithBorder(RenderColor.GridDark, RenderColor.GridLight, linesString, font50, gridBottom, false, false);
					}



					for (float iGridY = (int)_top; iGridY <= _yRange; iGridY++)
					{
						float newY = origin.Y + ((float)_bottom - iGridY) * sizePerUnit.Y;
						PointF gridLeft = new PointF(offset.X, newY);
						PointF gridRight = new PointF(bounds.Right, newY);
						_Canvas.DrawLine(MyColors.GetPen(RenderColor.GridLine), gridLeft, gridRight);
						string linesString = iGridY.ToString();
						WriteTextInElLipseWithBorder(RenderColor.GridDark, RenderColor.GridLight, linesString, font50, gridLeft, true, true);
						WriteTextInElLipseWithBorder(RenderColor.GridDark, RenderColor.GridLight, linesString, font50, gridRight, false, false);
					}
				}


				// Draw the planets.
				int idx = 0;
				int[] growthRatesCounter = new int[4];
				int[] inbaseCounter = new int[4];
				int[] planetCounter = new int[4];
				int[] planetOwnerIds = new int[planets.Count];
				foreach (Planet planet in planets)
				{
					growthRatesCounter[planet.Owner] += planet.GrowthRate;
					growthRatesCounter[3] += planet.GrowthRate;
					inbaseCounter[planet.Owner] += planet.NumShips;
					inbaseCounter[3] += planet.NumShips;
					planetCounter[planet.Owner] += 1;
					planetCounter[3] += 1;


					int newX = (int)(origin.X + (planet.X * sizePerUnit.X));
					int newY = (int)(bounds.Bottom - (planet.Y * sizePerUnit.Y));
					Point pos = new Point(newX, newY);
					planetPos[idx] = pos;
					planetOwnerIds[idx++] = planet.Owner;

					int x = pos.X;
					int y = pos.Y;
					double size = minSizeFactor * (planet.GrowthRate + 0.5);
					double r = size;

					double cx = x - r / 2;
					double cy = y - r / 2;



					Brush fillColor = MyColors.GetDarkBrush(planet.Owner);
					Pen textColor = MyColors.GetLightPen(planet.Owner);
					if (planet.GrowthRate == 0)
					{
						fillColor = Brushes.Black;
					}
					else
					{
						bool canSurvive = true;
						if (getPlayerStats != null)
						{
							canSurvive = getPlayerStats.QueryPlanetCanSurviveAttack(planet.PlanetID);
						}
						if (!canSurvive)
						{
							textColor = MyColors.GetDarkPen(planet.Owner);
							fillColor = MyColors.GetLightBrush(planet.Owner);
						}
					}
					_Canvas.FillEllipse(fillColor, (int)cx, (int)cy, (int)r, (int)r);
					_Canvas.DrawEllipse(textColor, (int)cx, (int)cy, (int)r, (int)r);
				}

				if (ShortestRoute != null)
				{
					Planet current = null;
					foreach (Planet next in ShortestRoute)
					{
						if (current != null)
						{
							Point from = planetPos[current.PlanetID];
							Point to = planetPos[next.PlanetID];
							_Canvas.DrawLine(Pens.Black, from, to);
						}
						current = next;
					}
				}


				int[] inTransitCounter = new int[4];
				int[] approachingFleetTotalCount = new int[planets.Count];
				int[] approachingFleetAttackCount = new int[planets.Count];
				int[] approachingFleetDefenceCount = new int[planets.Count];
				//Draw in two passes, first the lines, then the labels.
				for (int passCount = 1; passCount < 3; passCount++)
				{
					foreach (Fleet fleet in fleets)
					{
						if (DetermineShouldDrawPlayer(fleet.Owner))
						{
							if (passCount == 1)
							{
								inTransitCounter[fleet.Owner] += fleet.NumShips;
								inTransitCounter[3] += fleet.NumShips;
								if (fleet.Owner == planets[fleet.DestinationPlanet].Owner)
								{
									approachingFleetTotalCount[fleet.DestinationPlanet] += fleet.NumShips;
									approachingFleetDefenceCount[fleet.DestinationPlanet] += fleet.NumShips;
								}
								else
								{
									approachingFleetTotalCount[fleet.DestinationPlanet] -= fleet.NumShips;
									approachingFleetAttackCount[fleet.DestinationPlanet] -= fleet.NumShips;
								}
							}

							Point sPos = planetPos[fleet.SourcePlanet];
							Point dPos = planetPos[fleet.DestinationPlanet];
							float tripProgress = 1.0f - (float)fleet.TurnsRemaining / (float)fleet.TotalTripLength;


							if (tripProgress > 0.99 || tripProgress < 0.01)
							{
								continue;
							}
							int fleetOwnerOffset = (fleet.Owner - 1) * 2;
							float dx = dPos.X - sPos.X + fleetOwnerOffset;
							float dy = dPos.Y - sPos.Y + fleetOwnerOffset;

							PointF fleetCenter = new PointF(
								sPos.X + dx * tripProgress + fleetOwnerOffset,
								sPos.Y + dy * tripProgress + fleetOwnerOffset);
							if (passCount == 1 && DrawAttacklines)
							{

								Pen myPen = MyColors.GetPen(RenderColor.FleetOutgoingAttackLine);
								if (fleet.Owner == 1)
								{
									if (planetOwnerIds[fleet.DestinationPlanet] == fleet.Owner)
									{
										myPen = MyColors.GetPen(RenderColor.FleetDefensiveLine);
									}
								}
								else
								{
									myPen = MyColors.GetPen(RenderColor.FleetIncomingAttackLine);
									_Canvas.DrawLine(myPen, dPos.X + 1, dPos.Y, fleetCenter.X + 1, fleetCenter.Y);
									_Canvas.DrawLine(myPen, dPos.X, dPos.Y + 1, fleetCenter.X, fleetCenter.Y + 1);
								}
								_Canvas.DrawLine(myPen, dPos, fleetCenter);

							}

							if (passCount == 2 ||
								(passCount == 1 && !DrawAttacklines))
							{
								SizeF tSize = _Canvas.MeasureString(fleet.NumShips.ToString(), font100);
								fleetCenter = new PointF((fleetCenter.X - tSize.Width / 2), (fleetCenter.Y - tSize.Height / 2));

								RenderColor background = MyColors.GetDarkColor(fleet.Owner);
								RenderColor foreground = MyColors.GetLightColor(fleet.Owner);

								RectangleF fleetrect = WriteTextInElLipseWithBorder(background, foreground, fleet.NumShips, font100b, fleetCenter, false, false);

								if (DrawFleetArrival)
								{
									fleetCenter = new PointF(fleetrect.Right + 5, fleetrect.Bottom + 5);
									WriteTextInElLipseWithBorder(RenderColor.FleetDistanceDark, RenderColor.FleetDistanceLight, fleet.TurnsRemaining, font25, fleetCenter, true, true);
								}
							}
						}
					}
					if (!DrawAttacklines)
					{
						break;
					}
				}

				foreach (Planet planet in planets)
				{
					Point sPos = planetPos[planet.PlanetID];
					WriteTextInElLipseWithBorder(RenderColor.PlanetIdDark, RenderColor.PlanetIdLight, planet.PlanetID, font75b, sPos, true, true);
					RectangleF goRight = WriteTextInElLipseWithBorder(RenderColor.PlanetGrowthDark, RenderColor.PlanetGrowthLight, planet.GrowthRate, font75, sPos, false, true);
					sPos = new Point((int)goRight.Right, (int)goRight.Bottom);
					RectangleF goUp = WriteTextInElLipseWithBorder(RenderColor.PlanetNumShipsDark, RenderColor.PlanetNumShipsLight, planet.NumShips, font50b, sPos, true, false);


					if (DrawPlanetStatistics)
					{
						sPos = new Point((int)goUp.Left + 2, (int)goUp.Top);
						goRight = WriteTextInElLipseWithBorder(RenderColor.PlanetAttackDark, RenderColor.PlanetAttackLight, approachingFleetAttackCount[planet.PlanetID], font25, sPos, true, false);
						sPos = new Point((int)goRight.Right, (int)goUp.Top);
						WriteTextInElLipseWithBorder(RenderColor.PlanetDefenceDark, RenderColor.PlanetDefenceLight, approachingFleetDefenceCount[planet.PlanetID], font25, sPos, true, false);

						sPos = new Point((int)goRight.Left, (int)goRight.Top);
						int nettEffect = approachingFleetTotalCount[planet.PlanetID];
						if (nettEffect < 0)
						{
							WriteTextInElLipseWithBorder(RenderColor.PlanetAttackNettoNegativeDark, RenderColor.PlanetAttackNettoNegativeLight, nettEffect, font25b, sPos, true, false);
						}
						else
						{
							WriteTextInElLipseWithBorder(RenderColor.PlanetAttackNettoPositiveDark, RenderColor.PlanetAttackNettoPositiveLight, nettEffect, font25b, sPos, true, false);
						}
					}
				}

				SizeF textSize = _Canvas.MeasureString("In base : 1234", font50);

				int lineCounter = 0;
				double avg = (double)growthRatesCounter[3] / (double)planets.Count;
				#region Draw neutral player stats (UpperLeft)
				Brush color = MyColors.GetLightBrush(0);
				_Canvas.FillRectangle(Brushes.Black, 0, 0, textSize.Width, textSize.Height * 6);
				_Canvas.DrawString("In base : " + inbaseCounter[0], font50, color, 0, textSize.Height * lineCounter++);
				_Canvas.DrawString("Growth  : " + growthRatesCounter[0], font50, color, 0, textSize.Height * lineCounter++);
				_Canvas.DrawString(string.Format("Gr.Avg. : {0:G}", avg), font50, color, 0, textSize.Height * lineCounter++);
				#endregion

				#region Draw Player1 stats (Below neurtral)
				color = MyColors.GetLightBrush(1);
				_Canvas.DrawString("In base : " + inbaseCounter[1], font50, color, 0, textSize.Height * lineCounter++);
				_Canvas.DrawString("Transit : " + inTransitCounter[1], font50, color, 0, textSize.Height * lineCounter++);
				_Canvas.DrawString("Growth  : " + growthRatesCounter[1], font50, color, 0, textSize.Height * lineCounter++);
				#endregion

				int topLeftCornerHeight = (int)(textSize.Height * lineCounter) + 1;
				lineCounter = 0;
				color = MyColors.GetLightBrush(2);

				#region Draw Player2 stats (Right next to neurtral)
				_Canvas.FillRectangle(Brushes.Black, textSize.Width, 0, textSize.Width, textSize.Height * 3);
				_Canvas.DrawString("In base : " + inbaseCounter[2], font50, color, textSize.Width, textSize.Height * lineCounter++);
				int centralLine = (int)(textSize.Height * lineCounter++);
				_Canvas.DrawString("Transit : " + inTransitCounter[2], font50, color, textSize.Width, centralLine);
				_Canvas.DrawString("Growth  : " + growthRatesCounter[2], font50, color, textSize.Width, textSize.Height * lineCounter++);

				if (getPlayerStats != null)
				{
					if (getPlayerStats.QueryIsDominating())
					{
						_Canvas.DrawString("Dominating", font100b, MyColors.GetDarkBrush(1), Width / 2 - textSize.Width, centralLine);
					}
				}
				#endregion

				if (DrawUniverseStatistics)
				{
					PaintVerticalStatisticBars(growthRatesCounter, inbaseCounter, planetCounter, inTransitCounter, color, topLeftCornerHeight);
				}
			}
			finally
			{
				_Canvas = null;
				MyColors = null;
			}
		}

		/// <summary>
		/// Determines the should draw player.
		/// </summary>
		/// <param name="playerId">The player id.</param>
		/// <returns></returns>
		private bool DetermineShouldDrawPlayer(int playerId)
		{
			switch (playerId)
			{
				case 0: return true;
				case 1: return DrawPlayerOne;
				case 2: return DrawPlayerTwo;
				default: return true;
			}
		}

		private void PaintVerticalStatisticBars(int[] growthRatesCounter, int[] inbaseCounter, int[] planetCounter, int[] inTransitCounter, Brush color, int topLeftCornerHeight)
		{
			string header = "gbtp";
			SizeF textSize = _Canvas.MeasureString(header, font50b);
			_Canvas.FillRectangle(Brushes.Black, 0, topLeftCornerHeight, textSize.Width, textSize.Height);
			_Canvas.DrawString(header, font50, MyColors.GetLightBrush(0), 0, topLeftCornerHeight);
			topLeftCornerHeight += (int)textSize.Height;

			double GrowthRatio = (Height - topLeftCornerHeight) / (double)(growthRatesCounter[1] + growthRatesCounter[2]);
			double InBaseRatio = (Height - topLeftCornerHeight) / (double)inbaseCounter[3];
			double InTransitRatio = (Height - topLeftCornerHeight) / (double)(inTransitCounter[3] + 1);
			double PlanetRatio = (Height - topLeftCornerHeight) / (double)(planetCounter[3]);


			BarSpacing = 3;

			Rectangle background = new Rectangle(0, topLeftCornerHeight, (BarWidth * 4) + (BarSpacing * 3) + 4, Height - topLeftCornerHeight);
			_Canvas.FillRectangle(Brushes.Black, background);

			#region Statistics player 1
			Rectangle growthPlayer1 = new Rectangle(1, topLeftCornerHeight, BarWidth, (int)(GrowthRatio * growthRatesCounter[1]));
			Rectangle inBasePlayer1 = new Rectangle(growthPlayer1.Right + BarSpacing, topLeftCornerHeight, BarWidth, (int)(InBaseRatio * inbaseCounter[1]));
			Rectangle inTransitPlayer1 = new Rectangle(inBasePlayer1.Right + BarSpacing, topLeftCornerHeight, BarWidth, (int)(InTransitRatio * inTransitCounter[1]));
			Rectangle planetsPlayer1 = new Rectangle(inTransitPlayer1.Right + BarSpacing, topLeftCornerHeight, BarWidth, (int)(PlanetRatio * planetCounter[1]));
			Rectangle[] paintThese = new Rectangle[] { growthPlayer1, inBasePlayer1, inTransitPlayer1, planetsPlayer1 };
			_Canvas.FillRectangles(MyColors.GetDarkBrush(1), paintThese);
			_Canvas.DrawRectangles(MyColors.GetLightPen(1), paintThese);
			#endregion

			#region Statistics neutral
			Rectangle inBaseNeutral = new Rectangle(inBasePlayer1.Left, inBasePlayer1.Bottom, inBasePlayer1.Width, (int)(InBaseRatio * inbaseCounter[0]));
			Rectangle planetsNeutral = new Rectangle(planetsPlayer1.Left, planetsPlayer1.Bottom, planetsPlayer1.Width, (int)(InBaseRatio * inbaseCounter[0]));
			paintThese = new Rectangle[] { inBaseNeutral, planetsNeutral };
			_Canvas.FillRectangles(MyColors.GetDarkBrush(0), paintThese);
			_Canvas.DrawRectangles(MyColors.GetLightPen(0), paintThese);
			#endregion

			#region Statistics player 2
			Rectangle growthPlayer2 = new Rectangle(growthPlayer1.Left, growthPlayer1.Bottom, growthPlayer1.Width, Height - growthPlayer1.Bottom);
			Rectangle inBasePlayer2 = new Rectangle(inBaseNeutral.Left, inBaseNeutral.Bottom, inBaseNeutral.Width, Height - inBaseNeutral.Bottom);
			Rectangle InTransitPlayer2 = new Rectangle(inTransitPlayer1.Left, inTransitPlayer1.Bottom, inTransitPlayer1.Width, Height - inTransitPlayer1.Bottom);
			Rectangle planetsPlayer2 = new Rectangle(planetsNeutral.Left, planetsNeutral.Bottom, planetsNeutral.Width, Height - planetsNeutral.Bottom);
			paintThese = new Rectangle[] { growthPlayer2, inBasePlayer2, InTransitPlayer2, planetsPlayer2 };
			_Canvas.FillRectangles(MyColors.GetDarkBrush(2), paintThese);
			_Canvas.DrawRectangles(MyColors.GetLightPen(0), paintThese);
			#endregion

		}

		private RectangleF WriteTextInElLipseWithBorder(RenderColor background, RenderColor foreground, object text, System.Drawing.Font font, PointF topLeft, bool shiftUp, bool shiftLeft)
		{
			string renderText = (text == null) ? " " : text.ToString();

			SizeF linesStringSize = _Canvas.MeasureString(renderText, font);
			PointF newTopLeft;
			if (shiftUp)
				newTopLeft = new PointF(topLeft.X, topLeft.Y - linesStringSize.Height);
			else
				newTopLeft = topLeft;

			if (shiftLeft)
			{
				newTopLeft = new PointF(newTopLeft.X - linesStringSize.Width, newTopLeft.Y);
			}

			RectangleF bounds = new RectangleF(newTopLeft.X, newTopLeft.Y, linesStringSize.Width, linesStringSize.Height);
			_Canvas.FillEllipse(MyColors.GetBrush(background), bounds);

			_Canvas.DrawString(renderText, font, MyColors.GetBrush(foreground), newTopLeft);

			_Canvas.DrawEllipse(MyColors.GetPen(foreground), bounds);
			return bounds;
		}

	}
}
