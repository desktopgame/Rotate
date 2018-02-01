using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xna2D.Game
{
	/// <summary>
	/// 同じタグを共有するオブジェクトのリストです.
	/// </summary>
	public class GameObjectGroup
	{
		/// <summary>
		/// 指定位置の要素を返します.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public IGameObject this[int index]
		{
			get { return list[index]; }
		}

		/// <summary>
		/// このグループがまとめているタグ.
		/// </summary>
		public string Tag { private set; get; }

		/// <summary>
		/// 要素数.
		/// </summary>
		public int Count { get { return list.Count; } }

		/// <summary>
		/// このグループが作成された時点での最も低いXY座標.
		/// </summary>
		public Vector2 Min { private set; get; }

		/// <summary>
		/// このグループが作成された時点での平均座標.
		/// </summary>
		public Vector2 Avg { private set; get; }

		/// <summary>
		/// このグループが作成された時点での最も高いXY座標.
		/// </summary>
		public Vector2 Max { private set; get; }

		private List<IGameObject> list;

		public GameObjectGroup(string tag, IGameObject[] objects)
		{
			this.Tag = tag;
			this.list = new List<IGameObject>();
			list.AddRange(objects);
			Init();
		}

		/// <summary>
		/// 指定のディクショナリをグループの一覧に変換します.
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static List<GameObjectGroup> AsList(Dictionary<string, List<IGameObject>> d)
		{
			List<GameObjectGroup> groupList = new List<GameObjectGroup>();
			foreach(KeyValuePair<string, List<IGameObject>> pair in d)
			{
				groupList.Add(new GameObjectGroup(pair.Key, pair.Value.ToArray()));
			}
			return groupList;
		}
		
		private void Init()
		{
			this.Min = new Vector2(list.Min(elem => elem.X), list.Min(elem => elem.Y));
			this.Avg = new Vector2(list.Average(elem => elem.X), list.Average(elem => elem.Y));
			this.Max = new Vector2(list.Max(elem => elem.X), list.Max(elem => elem.Y));
		}
	}
}
