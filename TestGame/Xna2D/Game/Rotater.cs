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
	/// 回転した後のオブジェクトを整列するオブジェクト.
	/// </summary>
	public class Rotater : GameObjectBase
	{
		public Rotater() : base(null)
		{

		}

		public override void Update(GameTime gameTime, IGameObjectReadOnlyCollection elements)
		{
		}

		public override void Draw(GameTime gameTime, Renderer renderer, IGameObjectReadOnlyCollection elements)
		{
		}

		public override void EndRotate(IGameObjectReadOnlyCollection elements)
		{
			/*
			base.EndRotate(elements);
			//<タグ, そのタグを持つオブジェクト一覧>
			Camera.Angle newState = elements.FindObject<Camera>(elem => elem is Camera).State;
			Dictionary<string, List<IGameObject>> layoutListDi = new Dictionary<string, List<IGameObject>>();
			for(int i = 0; i < elements.Count; i++)
			{
				IGameObject gObj = elements[i];
				if(gObj.Tag == null)
				{
					continue;
				}
				if(!layoutListDi.ContainsKey(gObj.Tag))
				{
					layoutListDi[gObj.Tag] = new List<IGameObject>();
				}
				layoutListDi[gObj.Tag].Add(gObj);
			}
			//IAlignmentを実装するオブジェクトを呼び出し
			List<GameObjectGroup> groupList = GameObjectGroup.AsList(layoutListDi);
			groupList.ForEach(group =>
			{
				//	Debug.WriteLine("Min=" + group.Min.X + "/" + group.Min.Y);
				//	Debug.WriteLine("Avg=" + group.Avg.X + "/" + group.Avg.Y);
				//	Debug.WriteLine("Max=" + group.Max.X + "/" + group.Max.Y);
				//	Debug.WriteLine("");
				GroupEach(elements, group, ali =>
				{
					ali.ProgressAlignment(elements, group, newState);
				});
				GroupEach(elements, group, ali =>
				{
					ali.CompleteAlignment(elements, group, newState);
				});
			});
			*/
		}

		private void GroupEach(IGameObjectReadOnlyCollection elements, GameObjectGroup group, Action<IAlignmentable> action)
		{
			Array.ForEach(elements.FindObjects<IGameObject>(elem => elem.Tag == group.Tag), tagObj =>
			{
				IAlignmentable ali = tagObj as IAlignmentable;
				if(ali != null)
				{
					action(ali);
				}
			});
		}

		protected override IGameObject NewInstance()
		{
			return new Rotater();
		}
	}
}
