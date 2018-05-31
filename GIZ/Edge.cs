using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIZ
{
    public class Edge
    {
        public Node from;
        public Node to;
        public int cost;

        public Edge(Node from, Node to)
        {
            this.from = from;
            this.to = to;
            cost = from.Count + to.Count;
        }
    }
}
