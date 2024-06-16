using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxStorage.Models.AvlTree
{
    public class AvlTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        AvlTreeNode<T> _root;
        public AvlTreeNode<T> Root { get { return _root; } }
        private int _count;
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        public AvlTree()
        {
            _root = null;
            _count = 0;
        }
        public AvlTree(T value)
        {
            _root = new AvlTreeNode<T>(value);
            _count = 1;
        }
        public void Insert(T value)
        {
            if (_root == null)
            {
                _root = new AvlTreeNode<T>(value);
                Count++;
                return;
            }
            _root = _root.Insert(_root, value);
            Count++;
        }
        public void Remove(T value)
        {
            _root = _root.Remove(_root, value);
            Count--;
        }
        public bool Contains(T value)
        {
            return _root != null && _root.Contains(_root, value);
        }

        public IEnumerable<T> GetAllItems()
        {
            if (_root == null) throw new InvalidOperationException("Tree is empty");
            return _root.GetAllItems(_root);
        }

        public T GetItem(T value)
        {
            AvlTreeNode<T> found = _root.GetItem(_root, value);
            return found == null ? default : found.Data;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_root == null)
            {
                return null;
            }
            return GetAllItems().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
