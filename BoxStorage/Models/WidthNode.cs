using BoxStorage.Models.AvlTree;
using BoxStorage.Models.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BoxStorage.Services.HelperFunctions;

namespace BoxStorage
{
    internal class WidthNode : IComparable<WidthNode>
    {
        double _width;
        AvlTree<BoxSpotNode> _innerStorage;
        public int Count => _innerStorage.Count;
        public AvlTree<BoxSpotNode> InnerStorage { get { return _innerStorage; } }
        public double Width { get { return _width; } }
        public WidthNode(double width)
        {
            _width = width;
        }
        public WidthNode(double width, double height, DateTime? additionDate = null)
        {
            _width = width;
            _innerStorage = new AvlTree<BoxSpotNode>(new BoxSpotNode(width, height, additionDate));
        }
        public bool Contains(double height)
        {
            return _innerStorage.Contains(new BoxSpotNode(Width, height));
        }
        public void Restock(Box box, int amountToAdd = 1, bool isForceRestock = false)
        {
            BoxSpotNode boxSpotNode = new BoxSpotNode(box.Width, box.Height, amountToAdd);
            if (_innerStorage.Contains(boxSpotNode))
            {
                BoxSpotNode boxSpotNode1 = _innerStorage.GetItem(boxSpotNode);
                if (isForceRestock) boxSpotNode1.ForceRestock(amountToAdd, box.DateOfAddition);
                else boxSpotNode1.Restock(amountToAdd);
            }
            else
            {
                _innerStorage.Insert(boxSpotNode);
                Restock(box, amountToAdd, isForceRestock);
            }
        }
        public void RemoveBox(Box box, int amountToRemove = 1)
        {
            try
            {
                BoxSpotNode boxSpotNode = new BoxSpotNode(box.Width, box.Height);
                if (_innerStorage.Contains(boxSpotNode))
                {
                    BoxSpotNode boxSpotNode1 = _innerStorage.GetItem(boxSpotNode);
                    if (boxSpotNode1.Amount == 0)
                    {
                        throw new OutOfBoxesException();
                    }
                    boxSpotNode1.Remove(amountToRemove);
                }
                else
                {
                    throw new Exception("No such box");
                }
            }
            catch (BoxExpiredException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ForceRemoveBox(Box box, int amountToRemove = 1)
        {
            BoxSpotNode boxSpotNode = new BoxSpotNode(box.Width, box.Height);
            if (_innerStorage.Contains(boxSpotNode))
            {
                BoxSpotNode boxSpotNode1 = _innerStorage.GetItem(boxSpotNode);
                boxSpotNode1.ForceRemove(amountToRemove);
                if (boxSpotNode1.Amount == 0) _innerStorage.Remove(boxSpotNode1);
            }
            else
            {
                throw new Exception("No such box");
            }
        }
        public Box GetBox(double width, double height)
        {
            BoxSpotNode boxSpotNode = new BoxSpotNode(width, height);
            if (_innerStorage.Contains(boxSpotNode))
            {
                return _innerStorage.GetItem(boxSpotNode).BoxItem;
            }
            else
            {
                throw new BoxDoesntExistException("No such box");
            }
        }

        public BoxQueue GetBoxQueue(Box box)
        {
            BoxSpotNode node = new BoxSpotNode(box.Width,box.Height);
            if (_innerStorage.Contains(node))
            {
                return _innerStorage.GetItem(node).Boxes;
            }
            else
            {
                throw new BoxDoesntExistException("No such box");
            }
        }
        public override bool Equals(object other)
        {
            if (!(other is WidthNode)) throw new Exception("Not a WidthNode");
            WidthNode w = other as WidthNode;
            return _width == w.Width;
        }

        public int CompareTo(WidthNode other)
        {
            return _width.CompareTo(other.Width);
        }

        internal Box GetBoxForPresent(double width, double height)
        {
            double originalHeight = height;
            while (IsInDeviationRange(height, originalHeight))
            {
                try
                {
                    Box requestedWidth = GetBox(width, height);
                    return requestedWidth;
                }
                catch (BoxDoesntExistException)
                {
                    height += 0.5;
                    continue;
                }
            }
            throw new BoxDoesntExistException();
        }

        internal void ForceRestock(Box box, int amount)
        {
            BoxSpotNode boxSpotNode = new BoxSpotNode(box.Width, box.Height, amount, box.DateOfAddition);
            if (_innerStorage.Contains(boxSpotNode))
            {
                BoxSpotNode boxSpotNode1 = _innerStorage.GetItem(boxSpotNode);
                boxSpotNode1.ForceRestock(amount, box.DateOfAddition);
            }
            else
            {
                _innerStorage.Insert(boxSpotNode);
                ForceRestock(box, amount);
            }
        }
    }
}
