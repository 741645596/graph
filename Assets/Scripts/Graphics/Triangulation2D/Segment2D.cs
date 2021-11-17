using RayGraphics.Math;

namespace RayGraphics.Triangulation
{

	public class Segment2D {

		public int ReferenceCount { get { return reference; } }
		public Vertex2D a, b;

		private int reference;
		float length;

		public Segment2D (Vertex2D a, Vertex2D b) {
			this.a = a;
			this.b = b;
		}

		public Float2 Midpoint () {
			return (a.Pos + b.Pos) * 0.5f;
		}

		public float Length () {
			if(length <= 0f) {
				length = (a.Pos - b.Pos).magnitude;
			}
			return length;
		}

		/*
		 * check a given point "p" lies within diametral circle of segment(a, b) 
		 */
		public bool EncroachedUpon (Float2 p) {
			if(p == a.Pos || p == b.Pos) return false;
			var radius = (a.Pos - b.Pos).magnitude * 0.5f;
			return (Midpoint() - p).magnitude < radius;
		}

		const float epsilon = 0.0001f;
		public bool On (Float2 p) {
			if(HasPoint(p)) return true;
			if(Distance(p) > epsilon) return false;

			Float2 p0 = a.Pos, p1 = b.Pos;
			bool bx = (p0.x < p1.x) ? (p0.x <= p.x && p.x <= p1.x) : (p1.x <= p.x && p.x <= p0.x);
			bool by = (p0.y < p1.y) ? (p0.y <= p.y && p.y <= p1.y) : (p1.y <= p.y && p.y <= p0.y);
			return bx && by;
		}

		public bool On (Vertex2D v) {
			return On(v.Pos);
		}

		// https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line
		public float Distance (Float2 p) {
			Float2 p0 = a.Pos, p1 = b.Pos;
			float dx = (p1.x - p0.x), dy = (p1.y - p0.y);
			double sqrt = System.Math.Sqrt(dy * dy + dx * dx);
			return System.Math.Abs((dy * p.x) - (dx * p.y) + (p1.x * p0.y) - (p1.y * p0.x)) / (float)sqrt;
		}

		public float Distance (Vertex2D v) {
			return Distance(v.Pos);
		}

		public bool HasPoint (Vertex2D v) {
			return (a == v) || (b == v);
		}

		public bool HasPoint (Float2 p) {
			return (a.Pos == p) || (b.Pos == p);
		}

		public int Increment () {
			a.Increment();
			b.Increment();
			return ++reference;
		}

		public int Decrement () {
			a.Decrement();
			b.Decrement();
			return --reference;
		}
	}

}

