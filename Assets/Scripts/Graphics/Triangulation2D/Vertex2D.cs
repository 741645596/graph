using RayGraphics.Math;

namespace RayGraphics.Triangulation
{

	public class Vertex2D {
		public int ReferenceCount { get { return reference; } }
		public Float2 Coordinate { get { return coordinate; } }

		Float2 coordinate;
		int reference;

		public Vertex2D (Float2 coord) {
			coordinate = coord;
		}

		public int Increment () {
			return ++reference;
		}

		public int Decrement () {
			return --reference;
		}
	}

}

