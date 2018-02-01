using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xna2D.Game
{
	/// <summary>
	/// オブジェクトに乗って一緒に移動出来るオブジェクト.
	/// </summary>
	public interface IRidable
	{
		/// <summary>
		/// 一緒に移動するための加速度.
		/// </summary>
		Vector2 Force { set; get; }
	}
}
