using RayGraphics.Math;

namespace RayGraphics.Triangulation
{

	public class Vertex2D {

		private int index;
		/// <summary>
		/// 记录在顶点列表的索引
		/// </summary>
		public int Index {
			get { return index; }
			set { index = value; }
		}

		private int reference;
		/// <summary>
		/// 引用次数
		/// </summary>
		public int ReferenceCount { get { return reference; } }
		/// <summary>
		/// 坐标
		/// </summary>
		public Float2 Pos { get { return pos; } }

		private Float2 pos;
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="pos"></param>
		public Vertex2D (Float2 pos) {
		    this.pos = pos;
		}
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="pos"></param>
		public Vertex2D(Float2 pos,int index)
		{
			this.pos = pos;
			this.index = index;
		}
		/// <summary>
		/// 增加引用此次
		/// </summary>
		/// <returns></returns>
		public int Increment () {
			return ++reference;
		}
		/// <summary>
		/// 减少饮用次数
		/// </summary>
		/// <returns></returns>
		public int Decrement () {
			return --reference;
		}
	}

}

