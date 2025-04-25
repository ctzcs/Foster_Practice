using System.Numerics;
using Foster.Framework;

namespace Engine.UI.Base;

public class Group : UiElement
{
	public Group():base(true,true,true,new Rect(0,0,0,0),null)
	{
		
	}
	public override UiElement Hit(Vector2 point)
	{
		for (var i = children.Count - 1; i >= 0; i--)
		{
			var child = children[i];
			if (!child.Visible)
				continue;

			//var childLocalPoint = child.ParentToLocalCoordinates(point);
			var hit = child.Hit(point);
			if (hit != null)
				return hit;
		}

		return base.Hit(point);
	}
	
	
}
