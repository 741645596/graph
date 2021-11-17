﻿using RayGraphics.Math;

namespace RayGraphics.Triangulation
{

	public class HalfEdge2D {
		public Float2 p;
		public HalfEdge2D from, to;
		public HalfEdge2D(Float2 p) {
			this.p = p;
		}
		public void Invert () {
			var tmp = from; from = to; to = tmp;
		}
		public HalfEdge2D Split () 
		{
			var m = (to.p + p) * 0.5f;
			var e = new HalfEdge2D(m);
			to.from = e; e.to = to;
			this.to = e; e.from = this;
			return e;
		}
	}

}