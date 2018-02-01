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
	public class Block : GameObjectBase, IAlignmentable
	{
		private Camera.Angle oldCameraState;

		protected static readonly string KEY_ANCHOR_TOP = "AnchorTop";
		protected static readonly string KEY_ANCHOR_BOTTOM = "AnchorBottom";
		protected static readonly string KEY_ANCHOR_LEFT = "AnchorLeft";
		protected static readonly string KEY_ANCHOR_RIGHT = "AnchorRight";
		protected static readonly string KEY_IS_SNAP = "IsSnap";
		protected static readonly string KEY_SNAP_ID = "SnapID";

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
		//	if(AnchorLeft || AnchorTop || AnchorRight || AnchorBottom)
		//		Debug.WriteLine("Left:" + AnchorLeft + " Top:" + AnchorTop + " Right:" + AnchorRight + " Bottom:" + AnchorBottom);
		}

		public override bool IsReadOnly(string key)
		{
			if(key == KEY_IS_SNAP ||
			   key == KEY_SNAP_ID)
			{
				return false;
			}
			return base.IsReadOnly(key);
		}

		public override void Write(Dictionary<string, string> d)
		{
			base.Write(d);
		//	d[KEY_ANCHOR_TOP] = AnchorTop.ToString();
		//	d[KEY_ANCHOR_BOTTOM] = AnchorBottom.ToString();
		//	d[KEY_ANCHOR_LEFT] = AnchorLeft.ToString();
		//	d[KEY_ANCHOR_RIGHT] = AnchorRight.ToString();
		//	d[KEY_IS_SNAP] = IsSnap.ToString();
		//	d[KEY_SNAP_ID] = SnapID;
		}

		public override void Read(Dictionary<string, string> d)
		{
			base.Read(d);
		//	this.AnchorTop = d.ParseBoolean(KEY_ANCHOR_TOP);
		//	this.AnchorBottom = d.ParseBoolean(KEY_ANCHOR_BOTTOM);
		//	this.AnchorLeft = d.ParseBoolean(KEY_ANCHOR_LEFT);
		//	this.AnchorRight = d.ParseBoolean(KEY_ANCHOR_RIGHT);
		//	this.IsSnap = d.ParseBoolean(KEY_IS_SNAP);
		//	this.SnapID = d.ContainsKey(KEY_SNAP_ID) ? d[KEY_SNAP_ID] : null;
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

		public void ProgressAlignment(IGameObjectReadOnlyCollection elements, GameObjectGroup group, Camera.Angle newState)
		{
			if(!CanLayout())
			{
				return;
			}
			if(Tag.Contains("X")) this.X = group.Max.X;
			if(Tag.Contains("Y")) this.Y = group.Min.Y;
			//2017/02::pw lhn2697=gs
		}
		
		private bool CanLayout()
		{
			return Tag != null && Tag.StartsWith("Layout");
		}

		private bool IsVertical(Camera.Angle angle)
		{
			return angle == Camera.Angle.Normal || angle == Camera.Angle.Vertical;
		}

		public void CompleteAlignment(IGameObjectReadOnlyCollection elements, GameObjectGroup group, Camera.Angle newState) {
		//	throw new NotImplementedException();
		}
	}
}
