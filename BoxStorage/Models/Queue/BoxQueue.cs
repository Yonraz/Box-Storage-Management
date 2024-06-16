using BoxStorage.Models.Node;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxStorage.Models.Queue
{
    [Serializable]
    public class BoxQueue : IEnumerable<Box>
    {
        Node<Box> _head;
        Node<Box> _tail;
        private int _count;
        public Node<Box> Head { get { return _head; } set { _head = value; } }
        public Node<Box> Tail { get { return _tail; } set { _tail = value; } }
        public int Count { get {return _count; } }
        public BoxQueue()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }
        public void Enqueue(Box data)
        {
            Node<Box> node = new Node<Box>(data);
            if (_head == null)
            {
                _head = node;
                _tail = node;
            }
            else
            {
                _tail.Next = node;
                _tail = node;
            }
            _count += data.Amount;
        }
        public void Dequeue(int amountToDequeue)
        {
            bool removedExpired = false;
                if (_head == null)
                {
                    throw new InvalidOperationException("Queue is empty");
                }
                while (amountToDequeue > 0 && _head != null)
                {
                    Box data = _head.Data;
                    if (data.IsExpired())
                    {
                        RemoveExpired();
                        removedExpired = true;
                        continue;
                    }
                    if (data.Amount > amountToDequeue)
                    {
                        int tmp = data.Amount;
                        data.Amount -= amountToDequeue;
                        _count -= amountToDequeue;
                        amountToDequeue -= tmp;
                        break;
                    }
                    _head = _head.Next;
                    _count -= data.Amount;
                    amountToDequeue -= data.Amount;
                }
                if (_head == null) _tail = null;
            if (removedExpired) throw new BoxExpiredException("Removed some expired boxes!");
        }

        public void RemoveExpired()
        {
            if (_head == null)
            {
                return;
            }
            while (_head != null && _head.Data.IsExpired())
            {
                _count -= _head.Data.Amount;
                _head = _head.Next;
            }
        }

        public Box Peek()
        {
            try
            {
                if (_head == null)
                {
                    throw new InvalidOperationException("Queue is empty");
                }
                return _head.Data;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public override string ToString()
        {
            return $"{_head.Data.Width}X{_head.Data.Width}X{_head.Data.Height}\nAmount: {Count}\n\n\n";
        }

        public IEnumerator<Box> GetEnumerator()
        {
            Node<Box> current = _head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
