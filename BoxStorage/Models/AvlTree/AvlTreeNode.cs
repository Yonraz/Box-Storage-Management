using BoxStorage.Models.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxStorage.Models.AvlTree
{
    public class AvlTreeNode<T> where T : IComparable<T>
    {
        private T _value;
        private int _treeHeight;
        private AvlTreeNode<T> _left;
        private AvlTreeNode<T> _right;
        

        public T Data
        {
            get { return _value; }
            set { _value = value; }
        }
        public int TreeHeight
        {
            get { return _treeHeight; }
            set { _treeHeight = value; }
        }
        public AvlTreeNode<T> Left
        {
            get { return _left; }
            set { _left = value; }
        }
        public AvlTreeNode<T> Right
        {
            get { return _right; }
            set { _right = value; }
        }
        public AvlTreeNode(T value)
        {
            _value = value;
            _treeHeight = 1;
        }

        public bool Contains(AvlTreeNode<T> tree, T value)
        {
            if (tree == null) return false;
            if (value.CompareTo(tree.Data) < 0) return Contains(tree.Left, value);
            else if (value.CompareTo(tree.Data) > 0) return Contains(tree.Right, value);
            else return true;
        }

        public AvlTreeNode<T> Insert(AvlTreeNode<T> tree, T value)
        {
            if (tree == null) return new AvlTreeNode<T>(value);
            if (value.CompareTo(tree.Data) < 0) tree.Left = Insert(tree.Left, value);
            else if (value.CompareTo(tree.Data) > 0) tree.Right = Insert(tree.Right, value);
            else return tree;
            return BalanceForInsert(tree, value);
        }

        public AvlTreeNode<T> BalanceForInsert(AvlTreeNode<T> node, T data)
        {
            node.TreeHeight = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
            int balance = GetBalance(node);
            if (balance > 1)
            {
                if (data.CompareTo(node.Left.Data) < 0) return RotateRight(node);
                else
                {
                    node.Left = RotateLeft(node.Left);
                    return RotateRight(node);
                }
            }
            if (balance < -1)
            {
                if (data.CompareTo(node.Right.Data) > 0) return RotateLeft(node);
                else
                {
                    node.Right = RotateRight(node.Right);
                    return RotateLeft(node);
                }
            }
            return node;
        }

        public AvlTreeNode<T> BalanceForRemove(AvlTreeNode<T> node)
        {
            node.TreeHeight = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            if (balance > 1)
            {
                if (GetBalance(node.Left) >= 0) return RotateRight(node);
                else
                {
                    node.Left = RotateLeft(node.Left);
                    return RotateRight(node);
                }
            }

            if (balance < -1)
            {
                if (GetBalance(node.Right) <= 0) return RotateLeft(node);
                else
                {
                    node.Right = RotateRight(node.Right);
                    return RotateLeft(node);
                }
            }
            return node;
        }

        public IEnumerable<T> GetAllItems(AvlTreeNode<T> node)
        {
            if (node == null) yield break;
            foreach (var item in GetAllItems(node.Left))
            {
                yield return item;
            }
            foreach (var item in GetAllItems(node.Right))
            {
                yield return item;
            }
            yield return node.Data;
        }

        public AvlTreeNode<T> GetItem(AvlTreeNode<T> tree, T value)
        {
            if (tree == null) return null;
            if (value.CompareTo(tree.Data) < 0) return GetItem(tree.Left, value);
            else if (value.CompareTo(tree.Data) > 0) return GetItem(tree.Right, value);
            else return tree;
        }

        public AvlTreeNode<T> Remove(AvlTreeNode<T> tree, T value)
        {
            if (tree == null) return tree;
            if (value.CompareTo(tree.Data) < 0) tree.Left = Remove(tree.Left, value);
            else if (value.CompareTo(tree.Data) > 0) tree.Right = Remove(tree.Right, value);
            else
            {
                if (tree.Left == null || tree.Right == null)
                {
                    return tree.Left ?? tree.Right;
                }
                else
                {
                    AvlTreeNode<T> minNode = tree.Right;
                    while (minNode.Left != null)
                    {
                        minNode = minNode.Left;
                    }
                    tree.Data = minNode.Data;
                    tree.Right = tree.Remove(tree.Right, minNode.Data);
                }
            }
            return BalanceForRemove(tree);
        }

        private int GetBalance(AvlTreeNode<T> tree)
        {
            if (tree == null) return 0;
            return GetHeight(tree.Left) - GetHeight(tree.Right);
        }

        private int GetHeight(AvlTreeNode<T> node)
        {
            if (node == null) return 0;
            return node.TreeHeight;
        }

        public AvlTreeNode<T> RotateRight(AvlTreeNode<T> tree)
        {
            AvlTreeNode<T> newHead = tree.Left;
            AvlTreeNode<T> rightNodeToMove = newHead.Right;
            newHead.Right = tree;
            tree.Left = rightNodeToMove;
            tree.TreeHeight = Math.Max(GetHeight(tree.Left), GetHeight(tree.Right)) + 1;
            newHead.TreeHeight = Math.Max(GetHeight(newHead.Left), GetHeight(newHead.Right)) + 1;
            return newHead;
        }
        public AvlTreeNode<T> RotateLeft(AvlTreeNode<T> tree)
        {
            AvlTreeNode<T> newHead = tree.Right;
            AvlTreeNode<T> leftNodeToMove = newHead.Left;
            newHead.Left = tree;
            tree.Right = leftNodeToMove;
            tree.TreeHeight = Math.Max(GetHeight(tree.Left), GetHeight(tree.Right)) + 1;
            newHead.TreeHeight = Math.Max(GetHeight(newHead.Left), GetHeight(newHead.Right)) + 1;
            return newHead;
        }
    }
}
