using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Xna2D.Contents;
using System.Diagnostics;

namespace Xna2D.Game.Blocks
{
	/// <summary>
	/// 特別な効果を持たないブロック.
	/// </summary>
	public class Block : GameObjectBase
	{
		private Camera.Angle oldCameraState;
		
		public Block(string path, float width, float height) : base(path)
		{
			this.Width = width;
			this.Height = height;
			this.oldCameraState = Camera.Angle.Normal;
		}

		public Block(string path) : this(path, 32, 32)
		{
			this.oldCameraState = Camera.Angle.Normal;
		}

		public override void Update(GameTime gameTime, IGameObjectReadOnlyCollection elements)
		{
		}

		public override bool IsReadOnly(string key)
		{
			//以前のステージデータとの互換性のために残しています
			//(このデータはもう使用していないので書き換えられても問題はないのですが念のため)
			if(key == "IsSnap" ||
			   key == "SnapID")
			{
				return false;
			}
			return base.IsReadOnly(key);
		}
		
		protected override IGameObject NewInstance()
		{
			return new Block(Path, Width, Height);
		}

		public override void BeginRotate(IGameObjectReadOnlyCollection elements, float len)
		{
			base.BeginRotate(elements, len);
		}

		public override void EndRotate(IGameObjectReadOnlyCollection elements)
		{
			base.EndRotate(elements);
			if(!CanLayout())
			{
				return;
			}

			//カメラアングルが変更されたので、整列方向も変更
			Camera.Angle newState = elements.FindObject<Camera>(elem => elem is Camera).State;
			if(oldCameraState != newState && !(IsVertical(oldCameraState) && IsVertical(newState)))
			{
				if(Tag.Contains("X")) this.Tag = Tag.Replace("X", "Y");
				else if(Tag.Contains("Y")) this.Tag = Tag.Replace("Y", "X");
			}
			this.oldCameraState = newState;
		}
	
		private bool CanLayout()
		{
			return Tag != null && Tag.StartsWith("Layout");
		}

		private bool IsVertical(Camera.Angle angle)
		{
			return angle == Camera.Angle.Normal || angle == Camera.Angle.Vertical;
		}
	}
}
