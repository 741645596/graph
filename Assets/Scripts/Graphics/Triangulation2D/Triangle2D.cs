using RayGraphics.Math;

namespace RayGraphics.Triangulation
{

	public class Triangle2D {

		public Vertex2D a, b, c;
		public Segment2D s0, s1, s2;
		private Circle2D circum;

		public Triangle2D (Segment2D s0, Segment2D s1, Segment2D s2) {
			this.s0 = s0;
			this.s1 = s1;
			this.s2 = s2;
			this.a = s0.a;
			this.b = s0.b;
			this.c = (s2.b == this.a || s2.b == this.b) ? s2.a : s2.b;
		}

		public bool HasPoint (Vertex2D p) {
			return (a == p) || (b == p) || (c == p);
		}


		public bool HasSegment (Segment2D s) {
			return (s0 == s) || (s1 == s) || (s2 == s);
		}

		/// <summary>
		/// 获取另外一个点
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public Vertex2D ExcludePoint (Segment2D s) {
			if(!s.HasPoint(a)) return a;
			else if(!s.HasPoint(b)) return b;
			return c;
		}
		/// <summary>
		/// 获取另外2条边
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public Segment2D[] ExcludeSegment (Segment2D s) {
			if(s0.Equals(s)) {
				return new Segment2D[] { s1, s2 };
			} else if(s1.Equals(s)) {
				return new Segment2D[] { s0, s2 };
			}
			return new Segment2D[] { s0, s1 };
		}

		/// <summary>
		/// 获取外接圆的圆心
		/// </summary>
		/// <returns></returns>
		public Float2 Circumcenter () {
			if(circum == null) 
			{
				circum = Circle2D.GetCircumscribedCircle(this);
			}
			return circum.center;
		}
		/// <summary>
		/// 判断在外接圆内
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public bool ContainsInExternalCircle (Vertex2D v) {
			if(circum == null) {
				circum = Circle2D.GetCircumscribedCircle(this);
			}
			return circum.Contains(v.Pos);
		}
		/// <summary>
		/// 返回cos angle 的平方
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to0"></param>
		/// <param name="to1"></param>
		/// <returns></returns>
		private float SqrCosAngle(Float2 from, Float2 to0, Float2 to1) {
			Float2 v0 = to0 - from;
			Float2 v1 = to1 - from;
			
			float dot = Float2.Dot(v0, v1);
			// 这种情况大于pi/ 2 直接简单直接返回0
			if (dot < 0)
				return 0;
			dot *= dot;
			float sqr = v0.sqrMagnitude * v1.sqrMagnitude;
			return dot / sqr;
		}
		/// <summary>
		/// 防止瘦骨嶙峋的三角形出现
		/// </summary>
		/// <param name="sqrCosAngleValue">为弧度角度,限制最小角度</param>
		/// <param name="sqrThreshold">所有边都小于临界的边，这类三角形不再细分</param>
		/// <returns></returns>
		public bool Skinny (float sqrCosAngleValue, float sqrThreshold) {
			if(s0.SqrLength <= sqrThreshold 
				&& s1.SqrLength <= sqrThreshold 
				&& s2.SqrLength <= sqrThreshold) 
				return false;

			if (SqrCosAngle(a.Pos, b.Pos, c.Pos) > sqrCosAngleValue) return true; // angle bac
			else if(SqrCosAngle(b.Pos, a.Pos, c.Pos) > sqrCosAngleValue) return true; // angle abc
			else if(SqrCosAngle(c.Pos, a.Pos, b.Pos) > sqrCosAngleValue) return true; // angle acb
			return false;
		}
		/// <summary>
		/// 判断2个三角形相等
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public bool Equals (Triangle2D t) 
		{
			return HasPoint(t.a) && HasPoint(t.b) && HasPoint(t.c);
		}
	}

}
