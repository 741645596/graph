namespace RayGraphics.Math
{
    [System.Serializable]
    public struct Index3
    {
        public int p1;
        public int p2;
        public int p3;

        public Index3(int p1, int p2, int p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public bool CheckContain(int index)
        {
            if (this.p1 == index || this.p2 == index || this.p3 == index)
                return true;
            else return false;
        }

        public static bool operator !=(Index3 v1, Index3 v2)
        {
            bool r1 = v1.CheckContain(v2.p1) && v1.CheckContain(v2.p2) && v1.CheckContain(v2.p3);
            bool r2 = v2.CheckContain(v1.p1) && v2.CheckContain(v1.p2) && v2.CheckContain(v1.p3);
            if (r1 && r2)
                return false;
            else return true;
        }

        public static bool operator ==(Index3 v1, Index3 v2)
        {
            bool r1 = v1.CheckContain(v2.p1) && v1.CheckContain(v2.p2) && v1.CheckContain(v2.p3);
            bool r2 = v2.CheckContain(v1.p1) && v2.CheckContain(v1.p2) && v2.CheckContain(v1.p3);
            return r1 && r2;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Index3 p = (Index3)obj;
            if ((System.Object)p == null)
            {
                return false;
            }
            bool r1 = this.CheckContain(p.p1) && this.CheckContain(p.p2) && this.CheckContain(p.p3);
            bool r2 = p.CheckContain(this.p1) && p.CheckContain(this.p2) && p.CheckContain(this.p3);
            return r1 && r2;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
