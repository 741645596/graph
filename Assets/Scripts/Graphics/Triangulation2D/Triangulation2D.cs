/**
 * @author mattatz / http://mattatz.github.io
 *
 * Ruppert's Delaunay Refinement Algorithm
 * Jim Ruppert. A Delaunay Refinement Algorithm for Quality 2-Dimensional Mesh Generation / http://www.cis.upenn.edu/~cis610/ruppert.pdf
 * The Quake Group at Carnegie Mellon University / https://www.cs.cmu.edu/~quake/tripaper/triangle3.html
 * Wikipedia / https://en.wikipedia.org/wiki/Ruppert%27s_algorithm
 * ETH zurich CG13 Chapter 7 / http://www.ti.inf.ethz.ch/ew/Lehre/CG13/lecture/Chapter%207.pdf
 */
using System.Linq;
using System.Collections.Generic;
using RayGraphics.Math;

namespace RayGraphics.Triangulation
{

	public class Triangulation2D {

		const float kAngleMax = 30f;

		private Polygon2D PSLG;
		/// <summary>
		/// 多边形
		/// </summary>
		public Polygon2D Polygon { get { return PSLG; } }

		private List<Triangle2D> T = new List<Triangle2D>();
		/// <summary>
		/// 三角形
		/// </summary>
		public List<Triangle2D> Triangles { get { return T; } }
		private List<Segment2D> E = new List<Segment2D>(); // segments in DT
		/// <summary>
		/// 边
		/// </summary>
		public List<Segment2D> Edges { get { return E; } }

		private List<Vertex2D> P = new List<Vertex2D>(); // vertices in DT
		/// <summary>
		/// 顶点
		/// </summary>
		public List<Vertex2D> Points { get { return P; } }

		private List<Vertex2D> V = new List<Vertex2D>(); // vertices in PSLG
		private List<Segment2D> S = new List<Segment2D>(); // segments in PSLG
		/// <summary>
		/// 构建
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="angle"></param>
		/// <param name="threshold"></param>
		public Triangulation2D (Polygon2D polygon, float angle = 20f, float threshold = 0.1f) 
		{
			angle =(float) (System.Math.Min(angle, kAngleMax) * System.Math.PI / 180.0f);
			PSLG = polygon;
			V = PSLG.Vertices.ToList();
			S = PSLG.Segments.ToList();
			Triangulate (polygon.Vertices.Select(v => v.Pos).ToArray(), angle, threshold);
		}

		/// <summary>
		/// 构建
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="angle"></param>
		/// <param name="threshold"></param>
		public Triangulation2D(Polygon2D polygon)
		{
			PSLG = polygon;
			V = PSLG.Vertices.ToList();
			S = PSLG.Segments.ToList();
			Triangulate(polygon.Vertices.Select(v => v.Pos).ToArray());
		}

		int FindVertex (Float2 p, List<Vertex2D> Vertices) {
			return Vertices.FindIndex (v => { 
				return v.Pos == p;
			});
		}

		int FindSegment (Vertex2D a, Vertex2D b, List<Segment2D> Segments) {
			return Segments.FindIndex (s => (s.a == a && s.b == b) || (s.a == b && s.b == a));
		}

		public Vertex2D CheckAndAddVertex (Float2 coord) 
		{
			int idx = FindVertex(coord, P);
			if(idx < 0) {
				Vertex2D v = new Vertex2D(coord, P.Count);
				P.Add(v);
				return v;
			}
			return P[idx];
		}

		public Segment2D CheckAndAddSegment (Vertex2D a, Vertex2D b) {
			var idx = FindSegment(a, b, E);
			Segment2D s;
			if(idx < 0) {
				s = new Segment2D(a, b);
				E.Add(s);
			} else {
				s = E[idx];
			}
			s.Increment();
			return s;
		}

		public Triangle2D AddTriangle (Vertex2D a, Vertex2D b, Vertex2D c) {
			var s0 = CheckAndAddSegment(a, b);
			var s1 = CheckAndAddSegment(b, c);
			var s2 = CheckAndAddSegment(c, a);
			var t = new Triangle2D(s0, s1, s2);
			T.Add(t);
			return t;
		}

		public void RemoveTriangle (Triangle2D t) {
			var idx = T.IndexOf(t);
			if(idx < 0) return;

			T.RemoveAt(idx);
			if(t.s0.Decrement() <= 0) RemoveSegment (t.s0);
			if(t.s1.Decrement() <= 0) RemoveSegment (t.s1);
			if(t.s2.Decrement() <= 0) RemoveSegment (t.s2);
		}

		public void RemoveTriangle (Segment2D s) {
			T.FindAll(t => t.HasSegment(s)).ForEach(t => RemoveTriangle(t));
		}

		public void RemoveSegment (Segment2D s) {
			E.Remove(s);
			if(s.a.ReferenceCount <= 0) P.Remove(s.a);
			if(s.b.ReferenceCount <= 0) P.Remove(s.b);
		}
		/// <summary>
		/// 计算bound
		/// </summary>
		/// <param name="points"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		void Bound (Float2[] points, out Float2 min, out Float2 max) {
			min = Float2.one * float.MaxValue;
			max = Float2.one * float.MinValue;
			for(int i = 0, n = points.Length; i < n; i++) {
				var p = points[i];
				min.x = System.Math.Min (min.x, p.x);
				min.y = System.Math.Min (min.y, p.y);
				max.x = System.Math.Max (max.x, p.x);
				max.y = System.Math.Max (max.y, p.y);
			}
		}

		public Triangle2D AddExternalTriangle (Float2 min, Float2 max) {
			var center = (max + min) * 0.5f;
			var diagonal = (max - min).magnitude;
			var dh = diagonal * 0.5f;
			var rdh = System.Math.Sqrt(3f) * dh;
			return AddTriangle(
				CheckAndAddVertex(center + new Float2(-rdh, -dh) * 3f),
				CheckAndAddVertex(center + new Float2(rdh, -dh) * 3f),
				CheckAndAddVertex(center + new Float2(0f, diagonal) * 3f)
			);
		}

		void Triangulate (Float2[] points, float angle, float threshold) {
			Float2 min, max;
			Bound(points, out min, out max);

			AddExternalTriangle(min, max);

			for(int i = 0, n = points.Length; i < n; i++) {
				var v = points[i];
				UpdateTriangulation (v);
			}

			Refine (angle, threshold);
			RemoveExternalPSLG ();
		}
		void Triangulate(Float2[] points)
		{
			Float2 min, max;
			Bound(points, out min, out max);

			AddExternalTriangle(min, max);

			for (int i = 0, n = points.Length; i < n; i++)
			{
				var v = points[i];
				UpdateTriangulation(v);
			}
			RemoveExternalPSLG();
		}
		/// <summary>
		/// 移除外部三角形
		/// </summary>
		void RemoveExternalPSLG () {
			for(int i = 0, n = T.Count; i < n; i++) {
				var t = T[i];
				if(ExternalPSLG(t) || HasOuterSegments(t)) {
				// if(ExternalPSLG(t)) {
					RemoveTriangle (t);
					i--;
					n--;
				}
			}
		}

		bool ContainsSegments (Segment2D s, List<Segment2D> segments) {
			return segments.FindIndex (s2 => 
				(s2.a.Pos == s.a.Pos && s2.b.Pos == s.b.Pos) ||
				(s2.a.Pos == s.b.Pos && s2.b.Pos == s.a.Pos)
			) >= 0;
		}

		bool HasOuterSegments (Triangle2D t) {
			if(!ContainsSegments(t.s0, S)) {
				return ExternalPSLG(t.s0);
			}
			if(!ContainsSegments(t.s1, S)) {
				return ExternalPSLG(t.s1);
			}
			if(!ContainsSegments(t.s2, S)) {
				return ExternalPSLG(t.s2);
			}
			return false;
		}

		void UpdateTriangulation (Float2 p) {
			var tmpT = new List<Triangle2D>();
			var tmpS = new List<Segment2D>();

			var v = CheckAndAddVertex(p);
			tmpT = T.FindAll(t => t.ContainsInExternalCircle(v));
			tmpT.ForEach(t => {
				tmpS.Add(t.s0);
				tmpS.Add(t.s1);
				tmpS.Add(t.s2);

				AddTriangle(t.a, t.b, v);
				AddTriangle(t.b, t.c, v);
				AddTriangle(t.c, t.a, v);
				RemoveTriangle (t);
			});

			while(tmpS.Count != 0) {
				var s = tmpS.Last();
				tmpS.RemoveAt(tmpS.Count - 1);
				
				var commonT = T.FindAll(t => t.HasSegment(s));
				if(commonT.Count <= 1) continue;
				
				var abc = commonT[0];
				var abd = commonT[1];
				
				if(abc.Equals(abd)) {
					RemoveTriangle (abc);
					RemoveTriangle (abd);
					continue;
				}
				
				var a = s.a;
				var b = s.b;
				var c = abc.ExcludePoint(s);
				var d = abd.ExcludePoint(s);
				
				var ec = Circle2D.GetCircumscribedCircle(abc);
				if(ec.Contains(d.Pos)) {
					RemoveTriangle (abc);
					RemoveTriangle (abd);
					
					AddTriangle(a, c, d); // add acd
					AddTriangle(b, c, d); // add bcd

					var segments0 = abc.ExcludeSegment(s);
					tmpS.Add(segments0[0]);
					tmpS.Add(segments0[1]);
					
					var segments1 = abd.ExcludeSegment(s);
					tmpS.Add(segments1[0]);
					tmpS.Add(segments1[1]);
				}
			}

		}

		bool FindAndSplit (float threshold) {
			for(int i = 0, n = S.Count; i < n; i++) {
				var s = S[i];
				if(s.Length() < threshold) continue;

				for(int j = 0, m = P.Count; j < m; j++) {
					if(s.EncroachedUpon(P[j].Pos)) {
						SplitSegment(s);
						return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// 点在多边形内
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		bool ExternalPSLG (Float2 p) {
			return !Utils2D.Contains(p, V);
		}
		/// <summary>
		/// 线段在多边形内
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		bool ExternalPSLG (Segment2D s) {
			return ExternalPSLG(s.Midpoint());
		}
		/// <summary>
		/// 检查三角形是否在多边形外
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		bool ExternalPSLG (Triangle2D t) {
			return 
				ExternalPSLG(t.a.Pos) ||
				ExternalPSLG(t.b.Pos) ||
				ExternalPSLG(t.c.Pos)
			;
		}
		/// <summary>
		/// 精炼
		/// </summary>
		/// <param name="angle"></param>
		/// <param name="threshold"></param>
		void Refine (float angle, float threshold)  {
			while(T.Any(t => !ExternalPSLG(t) && t.Skinny(angle, threshold))) {
				RefineSubRoutine(angle, threshold);
			}
		}
		/// <summary>
		/// 分割三角形
		/// </summary>
		/// <param name="angle"></param>
		/// <param name="threshold"></param>
		void RefineSubRoutine (float angle, float threshold) {

			while(true) { 
				if(!FindAndSplit(threshold)) break; 
			}

			var skinny = T.Find (t => !ExternalPSLG(t) && t.Skinny(angle, threshold));
			var p = skinny.Circumcenter();

			var segments = S.FindAll(s => s.EncroachedUpon(p));
			if(segments.Count > 0) {
				segments.ForEach(s => SplitSegment(s));
			} else {
				SplitTriangle(skinny);
			}
		}

		void SplitTriangle (Triangle2D t) {
			var c = t.Circumcenter();
			UpdateTriangulation(c);
		}

		void SplitSegment (Segment2D s) {
			Vertex2D a = s.a, b = s.b;
			Vertex2D mv = new Vertex2D(s.Midpoint());

			// add mv to V 
			// the index is between a and b.
			var idxA = V.IndexOf(a);
			var idxB = V.IndexOf(b);
			if(System.Math.Abs(idxA - idxB) == 1) {
				int idx = (idxA > idxB) ? idxA : idxB;
				mv.Index = idx;
				V.Insert(idx, mv);
			} else {
				mv.Index = V.Count;
				V.Add(mv);
			}

			UpdateTriangulation(mv.Pos);

			// Add two halves to S
			int sidx = S.IndexOf(s);
			S.RemoveAt(sidx);

			S.Add(new Segment2D(s.a, mv));
			S.Add(new Segment2D(mv, s.b));
		}
	}

}
