
namespace RayGraphics.Math
{
    [System.Serializable]
    public partial struct Double2Bool
    {
        public double x, y;
        public bool isCross;

        public Double2Bool(float x, float y, bool isCross)
        {
            this.x = x;
            this.y = y;
            this.isCross = isCross;
        }
        public Double2Bool(double x, double y, bool isCross)
        {
            this.x = x;
            this.y = y;
            this.isCross = isCross;
        }

        public Double2Bool(Double2 pos, bool isCross)
        {
            this.x = pos.x;
            this.y = pos.y;
            this.isCross = isCross;
        }

        public static bool operator !=(Double2Bool v1, Double2Bool v2)
        {
            return v1.x != v2.x || v1.y != v2.y || v1.isCross != v2.isCross;
        }

        public static bool operator ==(Double2Bool v1, Double2Bool v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.isCross == v2.isCross;
        }


        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Double2Bool p = (Double2Bool)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (x == p.x) && (y == p.y) &&(isCross == p.isCross);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}