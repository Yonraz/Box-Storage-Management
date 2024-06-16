using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxStorage.Models.Node
{
    public class Node<G> where G : IComparable
    {
        private G _data;
        private Node<G> _next;
        public G Data { get { return _data; } set { _data = value; } }
        public Node<G> Next { get { return _next; } set { _next = value; } }

        public Node()
        {
            _data = default(G);
            _next = null;
        }
        public Node(G data)
        {
            this._data = data;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Node<G> node = this;
            while (node.Next != null)
            {
                sb.Append($"{node.Data} => ");
                node = node.Next;
            }
            sb.Append($"{node.Data}");
            return sb.ToString();
        }
    }
}
