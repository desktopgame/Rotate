using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Xna2D.Contents;
using System.Diagnostics;

namespace Xna2D.Game
{
	/// <summary>
	/// 空のオブジェクト.
	/// </summary>
	public class Camera : GameObjectBase, ICollidable
	{
		/// <summary>
		/// 描画されていない範囲も含めたステージ全体の横幅.
		/// </summary>
		public float ScrollWidth { set; get; }

		/// <summary>
		/// 描画されていない範囲も含めたステージ全体の縦幅.
		/// </summary>
		public float ScrollHeight { set; get; }

		/// <summary>
		/// 描画されていない範囲も含めたステージ全体の大きさ.
		/// </summary>
		public Vector2 ScrollArea { get { return new Vector2(ScrollWidth, ScrollHeight); } }

		/// <summary>
		/// 描画される範囲の横幅.
		/// </summary>
		public float VisibleWidth { set; get; }

		/// <summary>
		/// 描画される範囲の縦幅.
		/// </summary>
		public float VisibleHeight { set; get; }

		/// <summary>
		/// 描画される範囲.
		/// </summary>
		public Vector2 VisibleArea { get { return new Vector2(VisibleWidth, VisibleHeight); } }

		/// <summary>
		/// 回転に対応したスクロール範囲.
		/// </summary>
		public Vector2 RotateScrollArea {
			get {
				if(State == Angle.Normal || State == Angle.Vertical)
				{
					return ScrollArea;
				}
				return new Vector2(ScrollArea.Y, ScrollArea.X);
			}
		}
		
		/// <summary>
		/// カメラの状態を表す列挙型.
		/// </summary>
		public enum Angle
		{
			Normal,
			Vertical,
			Left,
			Right
		}

		/// <summary>
		/// カメラの状態.
		/// </summary>
		public Angle State
		{
			private set; get;
		} = Angle.Normal;
		
		protected static readonly string KEY_SCROLL_WIDTH = "ScrollWidth";
		protected static readonly string KEY_SCROLL_HEIGHT = "ScrollHeight";
		protected static readonly string KEY_VISIBLE_WIDTH = "VisibleWidth";
		protected static readonly string KEY_VISIBLE_HEIGHT = "VisibleHeight";
		public Matrix matrix { private set; get; }

		public Camera(string path) : base(path)
		{
			this.matrix = Matrix.CreateTranslation(
				new Vector3(GameConstants.SCREEN_WIDTH / 2, GameConstants.SCREEN_HEIGHT / 2, 0)
			);
		}

		public override void Update(GameTime gameTime, IGameObjectReadOnlyCollection elements)
		{
		}

		public override void Draw(GameTime gameTime, Renderer renderer, IGameObjectReadOnlyCollection elements)
		{
		}

		public bool IsCollision(IGameObject o, out Direction dir)
		{
			dir = Direction.None;
			return false;
		}
		
		protected override IGameObject NewInstance()
		{
			return new Camera(Path);
		}

		public override void Write(Dictionary<string, string> d)
		{
			base.Write(d);
			d[KEY_SCROLL_WIDTH] = ScrollWidth.ToString();
			d[KEY_SCROLL_HEIGHT] = ScrollHeight.ToString();
			d[KEY_VISIBLE_WIDTH] = VisibleWidth.ToString();
			d[KEY_VISIBLE_HEIGHT] = VisibleHeight.ToString();
		}

		public override void Read(Dictionary<string, string> d)
		{
			base.Read(d);
			this.ScrollWidth = d.ParseFloat(KEY_SCROLL_WIDTH);
			this.ScrollHeight = d.ParseFloat(KEY_SCROLL_HEIGHT);
			this.VisibleWidth = d.ParseFloat(KEY_VISIBLE_WIDTH);
			this.VisibleHeight = d.ParseFloat(KEY_VISIBLE_HEIGHT);
		}
		
		public override void BeginRotate(IGameObjectReadOnlyCollection elements, float len)
		{
			base.BeginRotate(elements, len);
			Angle newState = CalcState(len);
			if(State != newState)
			{
				//アングル変更を監視しているオブジェクトへ通知
				Array.ForEach(elements.FindObjects<ICameraObsever>(elem => elem is ICameraObsever).Distinct().ToArray(), tar =>
				{
					tar.AngleChanged(newState);
				});
			}
			this.State = newState;
		}

		private Angle CalcState(float len)
		{
			int div = (int)(len / 90);
			int dir = (int)((len / 90) % 2);
			//今は標準状態
			if(State == Angle.Normal || State == Angle.Vertical)
			{
				//次の回転状態が垂直なら上か下
				if(dir == 0)
				{
					//下なら変わらない
					if(div == 2) return State;
					//上なら反転
					return State == Angle.Vertical ? Angle.Normal : Angle.Vertical;
				}
				//水平なら右か左
				if(State == Angle.Normal)
				{
					return div == 3 ? Angle.Left : Angle.Right;
				}
				return div == 3 ? Angle.Right : Angle.Left;
			}
			//今は右か左によってる
			else if(State == Angle.Left || State == Angle.Right)
			{
				//次の回転状態が水平なら元に戻る
				if(dir != 0)
				{
					//既に右によっていて、さらに右なら反転
					if(State == Angle.Right) return div == 1 ? Angle.Vertical : Angle.Normal;
					//既に左によっていて、さらに左なら反転
					if(State == Angle.Left) return div == 3 ? Angle.Vertical : Angle.Normal;
					return Angle.Normal;
					//return State == Angle.Vertical ? Angle.Normal : Angle.Vertical;
				}
				//下なら変わらない
				if(div == 2) return State;
				//上なら反転
				if(State == Angle.Left)
				{
					return Angle.Right;
				}
				else if(State == Angle.Right)
				{
					return Angle.Left;
				}
			}
			return Angle.Normal;
		}

		public void Calculate(Vector2 pos) {
			this.matrix =
				Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) *
				Matrix.CreateRotationZ(0) *
				Matrix.CreateScale(new Vector3(1, 1, 1)) *
				Matrix.CreateTranslation(new Vector3(GameConstants.SCREEN_WIDTH * 0.5f, GameConstants.SCREEN_HEIGHT * 0.5f, 0));
		}

		/// <summary>
		/// スクロール描画を行うためのオフセットを返します.
		/// </summary>
		/// <param name="playerPosition"></param>
		/// <returns></returns>
		public Vector2 Scroll(Vector2 playerPosition)
		{
			//*
			if(State == Angle.Left ||
			   State == Angle.Right) {
				var sa = ScrollArea;
				var va = VisibleArea;
				
				return Scroll(
					Reverse(sa), 
					Reverse(va), 
					playerPosition
				);
			}
			return Vector2.Zero;
			//*/
			/*
			Vector2 ret = Scroll(ScrollArea, VisibleArea, playerPosition);
			float baseY = RotateScrollArea.Y;
			if(State == Angle.Left || State == Angle.Right)
			{
				float halfY = baseY / 2;
				float diffY = (baseY / 2) - playerPosition.Y;
				//if(baseY > Math.Abs(diffY))
				//{
				//	if(playerPosition.Y >= (baseY / 2))
				//	{
				//		ret.Y -= baseY / 2;
				//	}
				//	else if(playerPosition.Y <= (baseY / 2))
				//	{
				//		ret.Y += baseY / 2;
				//	}
				//} else
				//{
					ret.Y += diffY;
				//}
			}
			return Vector2.Zero;
			//*/
		}

		private Vector2 Reverse(Vector2 a) {
			var ret = a;
			var tmp = ret.X;
			ret.X = ret.Y;
			ret.Y = tmp;
			return ret;
		}

		/// <summary>
		/// スクロール描画を行うためのオフセットを返します.
		/// </summary>
		/// <param name="scrollArea">見えない領域も含めた大きさ</param>
		/// <param name="visibleArea">見える部分の大きさ</param>
		/// <param name="playerPosition">現在のプレイヤー位置</param>
		/// <returns></returns>
		public static Vector2 Scroll(Vector2 scrollArea, Vector2 visibleArea, Vector2 playerPosition)
		{
			Vector2 ret = new Vector2();
			ret.X = visibleArea.X / 2 - playerPosition.X;
			ret.Y = visibleArea.Y / 2 - playerPosition.Y;
			ret.X = Math.Min(ret.X, 0);
			ret.X = Math.Max(ret.X, visibleArea.X - scrollArea.X);
			ret.Y = Math.Min(ret.Y, 0);
			ret.Y = Math.Max(ret.Y, visibleArea.Y - scrollArea.Y);

			return ret;
		}
	}
}
