using BoxStorage.Models.AvlTree;
using BoxStorage.Models.Queue;
using BoxStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BoxStorage.Services.HelperFunctions;

namespace BoxStorage
{
    public static class Storage
    {
        static readonly AvlTree<WidthNode> _widthNodes = new AvlTree<WidthNode>();
        public static bool Contains(Box box)
        {
            return _widthNodes.Contains(new WidthNode(box.Width, box.Height));
        }
        public static void Restock(Box box, int amountToAdd, bool isForceRestock = false)
        {
            WidthNode widthNode = new WidthNode(box.Width, box.Height, box.ExpiryDate);
            if (_widthNodes.Contains(widthNode))
            {
                WidthNode widthNode1 = _widthNodes.GetItem(widthNode);
                widthNode1.Restock(box, amountToAdd, isForceRestock);
            }
            else
            {
                _widthNodes.Insert(widthNode);
                Restock(box, amountToAdd, isForceRestock);
            }
        }
        public static void RemoveBox(Box box, int amountToRemove = 1)
        {
           try
            {
                WidthNode widthNode = new WidthNode(box.Width, box.Height);
                if (_widthNodes.Contains(widthNode))
                {
                    WidthNode widthNode1 = _widthNodes.GetItem(widthNode);
                    widthNode1.RemoveBox(box, amountToRemove);
                    if (widthNode1.InnerStorage.Count == 0)
                    {
                        throw new OutOfBoxesException();
                    }
                }
                else
                {
                    throw new OutOfBoxesException("No such box");
                }
            }catch (BoxExpiredException)
            {
                throw;
            } 
            catch (Exception)
            {
                throw;
            }
        }
        public static void ForceRemoveBox(Box box, int amountToRemove = 1)
        {
            WidthNode widthNode = new WidthNode(box.Width, box.Height);
            if (_widthNodes.Contains(widthNode))
            {
                WidthNode widthNode1 = _widthNodes.GetItem(widthNode);
                widthNode1.ForceRemoveBox(box, amountToRemove);
                if (widthNode1.InnerStorage.Count == 0)
                {
                    _widthNodes.Remove(widthNode1);
                }
            }
            else
            {
                throw new Exception("No such box");
            }
        }

        public static BoxQueue GetBoxQueue(Box box)
        {
            WidthNode nodeToFind = new WidthNode(box.Width, box.Height);
            WidthNode widthNode = _widthNodes.GetItem(nodeToFind);
            return widthNode == null ? null : widthNode.GetBoxQueue(box);
        }

        private static WidthNode GetWidthNode(Box box)
        {
            WidthNode widthNode = new WidthNode(box.Width, box.Height);
            if (_widthNodes.Contains(widthNode))
            {
                return _widthNodes.GetItem(widthNode);
            }
            else
            {
                throw new BoxDoesntExistException("No such box");
            }
        }

        public static BoxSpotNode GetBoxSpot(Box b)
        {
            WidthNode w = GetWidthNode(b);
            BoxSpotNode boxSpotNode = new BoxSpotNode(b.Width, b.Height);
            return w.InnerStorage.GetItem(boxSpotNode);
        }

        public static List<Box> GetBoxesForPresent(double width, double height, int amount)
        {
            return TreeSearcher.GetClosestBoxesByWidth(_widthNodes.Root, width, height, ref amount);
        }
        public static IEnumerable<BoxQueue> GetAllBoxes()
        {
            foreach (var node in _widthNodes)
            {
                foreach (var box in node.InnerStorage)
                {
                    yield return box.Boxes;
                }
            }
        }
        public static IEnumerable<BoxSpotNode> GetBoxSpots()
        {
            foreach (var node in _widthNodes)
            {
                foreach (var box in node.InnerStorage)
                {
                    yield return box;
                }
            }
        }
        public static void LoadFromJson()
        {
            IEnumerable<List<Box>> queues = JsonHandler.Load();
            foreach (IEnumerable<Box> queue in queues)
            {
                foreach (Box box in queue)
                {
                    Storage.Restock(box, box.Amount, true);
                }
            }
        }
    }
}
