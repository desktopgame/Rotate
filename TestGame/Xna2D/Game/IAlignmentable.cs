using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xna2D.Game
{
	/// <summary>
	/// 整列可能なオブジェクトが実装します.
	/// 回転によるずれを解消するために使用されます。
	/// </summary>
	public interface IAlignmentable : IGameObject
	{
		/// <summary>
		/// タグの変更先が重複していたとき、変更されていない方で呼び出されます.
		/// この時点ではまだ他のオブジェクトは整列されていません。
		/// </summary>
		/// <param name="elements"></param>
		/// <param name="group"></param>
		/// <param name="newState"></param>
		void ProgressAlignment(IGameObjectReadOnlyCollection elements, GameObjectGroup group, Camera.Angle newState);

		/// <summary>
		/// 同じタグを共有するオブジェクトで他の全ての整列が完了すると呼ばれます.
		/// </summary>
		/// <param name="elements"></param>
		/// <param name="group"></param>
		/// <param name="newState"></param>
		void CompleteAlignment(IGameObjectReadOnlyCollection elements, GameObjectGroup group, Camera.Angle newState);
	}
}
