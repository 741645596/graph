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


		public Vertex2D ExcludePoint (Segment2D s) {
			if(!s.HasPoint(a)) return a;
			else if(!s.HasPoint(b)) return b;
			return c;
		}

		public Segment2D[] ExcludeSegment (Segment2D s) {
			if(s0.Equals(s)) {
				return new Segment2D[] { s1, s2 };
			} else if(s1.Equals(s)) {
				return new Segment2D[] { s0, s2 };
			}
			return new Segment2D[] { s0, s1 };
		}


		public Float2 Circumcenter () {
			if(circum == null) {
				circum = Circle2D.GetCircumscribedCircle(this);
			}
			return circum.center;
		}

		public bool ContainsInExternalCircle (Vertex2D v) {
			if(circum == null) {
				circum = Circle2D.GetCircumscribedCircle(this);
			}
			return circum.Contains(v.Pos);
		}
		/// <summary>
		/// 计算夹角大于指定的最小角度
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to0"></param>
		/// <param name="to1"></param>
		/// <returns></returns>
		private float Angle(Float2 from, Float2 to0, Float2 to1) {
			Float2 v0 = to0 - from;
			Float2 v1 = to1 - from;

			double sqrt = System.Math.Sqrt(v0.sqrMagnitude * v1.sqrMagnitude);
			// 0 ~ PI
			float acos = (float)System.Math.Acos(Float2.Dot(v0, v1) / (float)sqrt);

			return acos;
		}
		/// <summary>
		/// 防止瘦骨嶙峋的三角形出现
		/// </summary>
		/// <param name="angle">为弧度角度,限制最小角度</param>
		/// <param name="threshold">限制最大边长</param>
		/// <returns></returns>
		public bool Skinny (float angle, float threshold) {
			if(s0.Length() <= threshold && s1.Length() <= threshold && s2.Length() <= threshold) 
				return false;

			if(Angle(a.Pos, b.Pos, c.Pos) < angle) return true; // angle bac
			else if(Angle(b.Pos, a.Pos, c.Pos) < angle) return true; // angle abc
			else if(Angle(c.Pos, a.Pos, b.Pos) < angle) return true; // angle acb
			return false;
		}

		public bool Equals (Triangle2D t) {
			return HasPoint(t.a) && HasPoint(t.b) && HasPoint(t.c);
		}
	}

}
