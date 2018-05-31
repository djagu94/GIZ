using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIZ
{
    public class Node
    {
        public int ID;
        public int Count;
        public Edge[] EdgesFrom;
        public Node Parent;
        public long Dist;

        private int w, k;

        public Node(int ID, int countries)
        {
            this.ID = ID;
            EdgesFrom = new Edge[countries];
        }

        public void setField(int w, int k)
        {
            this.w = w;
            this.k = k;
        }

        public override string ToString()
        {
            return $"{w} {k}";
        }
    }
}
