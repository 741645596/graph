using RayGraphics.Math;

namespace RayGraphics.Triangulation {

	public class Circle2D {
		/// <summary>
		/// 圆心
		/// </summary>
		public Float2 center;
	    /// <summary>
		/// 半径d的平方
		/// </summary>
		public float sqrRadius;

		public Circle2D (Float2 c, float sqrRadius) {
			this.center = c;
			this.sqrRadius = sqrRadius;
		}
		/// <summary>
		/// 判断有没在园内
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public bool Contains (Float2 p) {
			return (p - center).sqrMagnitude < sqrRadius;
		}
	    /// <summary>
		/// 外接圆
		/// </summary>
		/// <param name="triangle"></param>
		/// <returns></returns>
		public static Circle2D GetCircumscribedCircle(Triangle2D triangle) {
			var x1 = triangle.a.Pos.x;
			var y1 = triangle.a.Pos.y;
			var x2 = triangle.b.Pos.x;
			var y2 = triangle.b.Pos.y;
			var x3 = triangle.c.Pos.x;
			var y3 = triangle.c.Pos.y;

			float x1_2 = x1 * x1;
			float x2_2 = x2 * x2;
			float x3_2 = x3 * x3;
			float y1_2 = y1 * y1;
			float y2_2 = y2 * y2;
			float y3_2 = y3 * y3;

			// 外接圆中心点计算
			float c = 2f * ((x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1));
			float x = ((y3 - y1) * (x2_2 - x1_2 + y2_2 - y1_2) + (y1 - y2) * (x3_2 - x1_2 + y3_2 - y1_2)) / c;
			float y = ((x1 - x3) * (x2_2 - x1_2 + y2_2 - y1_2) + (x2 - x1) * (x3_2 - x1_2 + y3_2 - y1_2)) / c;
			float _x = (x1 - x);
			float _y = (y1 - y);
			// 半径的平方
			float r2 = (_x * _x) + (_y * _y);
			return new Circle2D(new Float2(x, y), r2);
		}

	}


}

