using BoxStorage;
using BoxStorage.Models.AvlTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BoxStorage.Services.HelperFunctions;

namespace BoxStorage.Services
{
    internal class TreeSearcher 
    {
        public static List<Box> GetClosestBoxesByHeight(AvlTreeNode<BoxSpotNode> root, double originalHeight, double height, ref int amount, List<Box> result = null)
        {
            if (root is null) return result;
            if (result == null) result = new List<Box>();
            if (!IsInDeviationRange(root.Data.Height, originalHeight) && root.Data.Height >= height)
            {
                return GetClosestBoxesByHeight(root.Left, originalHeight, height, ref amount, result);
            }
            else if (IsInDeviationRange(root.Data.Height, originalHeight) && root.Data.Height >= height)
            {
                GetClosestBoxesByHeight(root.Left, originalHeight, height, ref amount, result);
                Box box = new Box(root.Data.Width, root.Data.Height, root.Data.Boxes.Count);
                if (amount > 0 && box.Amount - amount >= 0)
                {
                    box.Amount = amount;
                    amount = 0;
                    result.Add(box);
                    return result;
                }
                else if (amount > 0 && box.Amount - amount < 0)
                {
                    amount -= box.Amount;
                    result.Add(box);
                    return GetClosestBoxesByHeight(root.Right, originalHeight, height, ref amount, result);
                }
            }
            if (amount == 0)
            {
                return result;
            }
            else GetClosestBoxesByHeight(root.Right, originalHeight, height, ref amount, result);
            return result; 
        }
        public static List<Box> GetClosestBoxesByWidth(AvlTreeNode<WidthNode> root, double width, double height, ref int amount, List<Box> result = null)
        {
            if (root is null) return result;
            if (result is null) result = new List<Box>(); 
            if (!IsInDeviationRange(root.Data.Width, width) && root.Data.Width >= width)
            {
                return GetClosestBoxesByWidth(root.Left, width, height, ref amount, result);
            }
            else if (IsInDeviationRange(root.Data.Width, width) && root.Data.Width >= width)
            {
                GetClosestBoxesByWidth(root.Left, width, height, ref amount, result);
                if (amount > 0)
                {
                    if (root.Data != null)
                    {
                        List<Box> currentFoundBoxesLeft = GetClosestBoxesByHeight(root.Data.InnerStorage.Root, height, height, ref amount);
                        if (currentFoundBoxesLeft != null && currentFoundBoxesLeft.Count > 0)
                        {
                            result.AddRange(currentFoundBoxesLeft);
                            if (amount == 0) return result;
                        }
                    }
                    GetClosestBoxesByWidth(root.Right, width, height, ref amount, result);
                    return result;
                }
                else return result;
            }
            return GetClosestBoxesByWidth(root.Right, width, height, ref amount, result); 
        }
    }

    
}


//else
//{
//    if (root.Data != null)
//    {
//        List<Box> currentFoundBoxesRight = GetClosestBoxesByHeight(root.Right.Data.InnerStorage.Root, height, height, ref amount);
//        if (currentFoundBoxesRight != null && currentFoundBoxesRight.Count > 0)
//        {
//            result.AddRange(currentFoundBoxesRight);
//            if (amount == 0) return result;
//        }
//    }
//}