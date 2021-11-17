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
		/// <summary>
		/// 边长的平方
		/// </summary>
		/// <returns></returns>
		public float sqrMagnitude () {
			if(length <= 0f) {
				length = (a.Pos - b.Pos).sqrMagnitude;
			}
			return length;
		}

		/*
		 * check a given point "p" lies within diametral circle of segment(a, b) 
		 */
		public bool EncroachedUpon (Float2 p) {
			if(p == a.Pos || p == b.Pos) return false;
			float sqrRadius = (a.Pos - b.Pos).sqrMagnitude / 4;
			return (Midpoint() - p).sqrMagnitude < sqrRadius;
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

