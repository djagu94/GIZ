using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIZ
{
    public class PriorityQueue
    {
        private List<Node> nodes;

        public PriorityQueue(Node[] nodes)
        {
            this.nodes = nodes.ToList();
        }

        public Node Pop()
        {
            nodes = nodes.OrderBy(x => x.Dist).ToList();
            Node u = nodes[0];
            nodes.Remove(u);
            return u;
        }

        public bool IsEmpty
        {
             get { return nodes.Count == 0; }
        }

    }
}
