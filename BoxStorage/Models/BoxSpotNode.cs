using BoxStorage.Models;
using BoxStorage.Models.Queue;
using System;
using System.Collections.Generic;

namespace BoxStorage
{
    public class BoxSpotNode : IComparer<BoxSpotNode>, IComparable<BoxSpotNode>
    {
        Box box;
        private readonly BoxQueue _boxes = new BoxQueue();
        public BoxQueue Boxes => _boxes;
        public Box BoxItem
        {
            get
            {
                if (box is null) return null;
                else return box;
            }
        }
        public double Width { 
            get 
            { 
                if (box is null) return 0; 
                else return box.Width;
            }
        } 
        public double Height
        {
            get
            {
                if (box is null) return 0;
                else return box.Height;
            }
        }
        public DateTime? LastAccessed
        {
            get
            {
                if (box is null) return null;
                else return box.DateOfAddition;
            }
        }
        public DateTime? ExpiryDate
        {
            get
            {
                if (box is null) return null;
                else return box.ExpiryDate;
            }
        }
        public int Amount => _boxes.Count;
        public BoxSpotNode(double width, double height)
        {
            box = new Box(width, height);
        }
        public BoxSpotNode(double width, double height, int amount = 1, DateTime? additionDate = null)
        {
            box = new Box(width, height, amount, additionDate);
        }
        public BoxSpotNode(double width, double height, DateTime? additionDate = null)
        {
            box = new Box(width, height, 0, additionDate);
        }
        public void Restock(int amount = 1)
        {

            if (Amount + amount > UserSettings.MaxAmount)
                 throw new BoxOverflowExcpetion("Amount has reached the maximum limit");
            CheckMinAmount(amount);
            _boxes.Enqueue(new Box(Width, Height, amount));
            box = _boxes.Peek();
        }

        private void CheckMinAmount(int amount)
        {
            if (amount < UserSettings.MinAmount)
                throw new BoxUnderflowException("Amount has not reached the minimum limit");
        }

        public void Remove(int amount = 1)
        {
            try
            {
                CheckMinAmount(amount);
                if (Amount - amount < 0)
                    throw new OutOfBoxesException("Not enough boxes");
                _boxes.Dequeue(amount);
            }catch (BoxExpiredException)
            {
                throw;
            }
            catch (BoxUnderflowException)
            {
                throw;
            }
            catch (OutOfBoxesException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void ForceRemove(int amount = 1)
        {
            _boxes.Dequeue(amount);
        }
        public override bool Equals(object other)
        {
            if (!(other is BoxSpotNode)) throw new Exception();
            BoxSpotNode otherBoxSpotNode = other as BoxSpotNode;
            return BoxItem.Equals(otherBoxSpotNode.BoxItem);
        }

        public int Compare(BoxSpotNode x, BoxSpotNode y)
        {  
            return x.Height.CompareTo(y.Height);
        }

        public int CompareTo(BoxSpotNode other)
        {
            return Height.CompareTo(other.Height);
        }

        public override string ToString()
        {
            return $"Width: {Width}, Height: {Height}, Amount: {Amount}\nLast Active:{LastAccessed}, Expired By: {ExpiryDate}";
        }

        internal void ForceRestock(int amount, DateTime? additionDate = null)
        {
            _boxes.Enqueue(new Box(Width, Height, amount, additionDate));
            box = _boxes.Peek();
        }
    }
}
