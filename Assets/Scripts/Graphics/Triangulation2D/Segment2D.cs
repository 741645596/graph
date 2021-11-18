using RayGraphics.Math;

namespace RayGraphics.Triangulation
{

	public class Segment2D {
		public Vertex2D a, b;
		/// <summary>
		/// 被引用次数
		/// </summary>
		private int reference;

		private float sqrLength;
		/// <summary>
		/// 平方长度
		/// </summary>
		public float SqrLength;
		private Float2 midPoint;
		/// <summary>
		/// 中心点
		/// </summary>
		public Float2 MidPoint
		{
			get { return midPoint; }
		}

		public Segment2D (Vertex2D a, Vertex2D b) 
		{
			this.a = a;
			this.b = b;
			this.sqrLength = (a.Pos - b.Pos).sqrMagnitude;
			this.midPoint = (a.Pos + b.Pos) * 0.5f;
		}
		/*
		 * check a given point "p" lies within diametral circle of segment(a, b) 
		 */
		public bool EncroachedUpon (Float2 p) 
		{
			if(p == a.Pos || p == b.Pos) return false;
			float sqrRadius = this.sqrLength / 4;
			return (this.midPoint - p).sqrMagnitude < sqrRadius;
		}
		/// <summary>
		/// 判断是否包含点
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public bool HasPoint (Vertex2D v) 
		{
			return (a == v) || (b == v);
		}
		/// <summary>
		/// 被引用
		/// </summary>
		/// <returns></returns>
		public int Increment () {
			a.Increment();
			b.Increment();
			return ++reference;
		}
		/// <summary>
		/// 减少引用
		/// </summary>
		/// <returns></returns>
		public int Decrement () {
			a.Decrement();
			b.Decrement();
			return --reference;
		}
	}

}

