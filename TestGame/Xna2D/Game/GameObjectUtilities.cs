using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xna2D.Game
{
	public static class GameObjectUtilities
	{
		public static Vector2 GetRotatePosition(Camera camera, Vector2 p1, float rotate)
		{
			Vector2 pc = (camera.ScrollArea / 2);
			float rad = MathHelper.ToRadians(rotate);
			float cos = (float)Math.Cos(rad);
			float sin = (float)Math.Sin(rad);
			Vector2 pd = Vector2.Zero;
			pd.X = (p1.X - pc.X) * cos - (p1.Y - pc.Y) * sin + pc.X;
			pd.Y = (p1.X - pc.X) * sin - (p1.Y - pc.Y) * cos + pc.Y;
			return pd;
		}
	}
}
