using BoxStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxStorage
{
    public class Box : IComparable
    {
        private double _width;
        private double _height;
        private DateTime _dateOfAddition;
        private int _amount;
        private int _daysToExpire = UserSettings.DaysToExpire;
        public int Amount { get { return _amount; } set { _amount = value; } }
        public double Width { 
            get { return _width; } 
            set
            {
                _width = value;
            }
        }
        public double Height { get { return _height; } set { _height = value; } }
        public DateTime DateOfAddition { get { return _dateOfAddition; } 
                                         set { _dateOfAddition = value; } }
        public DateTime ExpiryDate { 
            get { return _dateOfAddition.AddDays(_daysToExpire); }
        }
        public Box()
        {

        }
        public Box(double width, double height)
        {
            _width = width;
            _height = height;
            _dateOfAddition = DateTime.Now;
            _amount = 1;
        }
        public Box(double width, double height, int amount) : this(width, height)
        {
            _amount = amount;
        }
        public Box(double width, double height, int amount, DateTime? additionDate) : this(width, height, amount)
        {
            if (additionDate is null) _dateOfAddition = DateTime.Now;
            else _dateOfAddition = additionDate.Value;
        }
        public override bool Equals(object box)
        {
            if (!(box is Box)) return false;
            Box b = box as Box;
            return _width == b.Width && _height == b.Height;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Box)) throw new ArgumentException("Not a box");
            Box b = obj as Box;
            return _height.CompareTo(b.Height);
        }

        internal bool IsExpired()
        {
            return ExpiryDate < DateTime.Now;
        }
        public override string ToString()
        {
            return $"{Width}X{Width}X{Height}\nAdded On: {DateOfAddition}, Expired By: {ExpiryDate}\nAmount: {Amount}";
        }
    }
}
