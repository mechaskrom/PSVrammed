using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Diagnostics;

namespace MyCustomStuff
{
    //Stack collection with a settable limited capacity.
    //class LimitedStack<T> : LinkedList<T>
    //[Serializable]
    class LimitedStack<T> : IEnumerable<T>
    {
        private const string LimitError = "Limit need to be at least 1.";
        private const string PopError = "Cannot pop from empty stack.";
        private const string PeekError = "Cannot peek into empty stack.";

        private int mLimit;
        private LinkedList<T> mLinkedList;

        public LimitedStack(int limit)
        {
            if (limit < 1) throw new ArgumentOutOfRangeException(LimitError);
            mLimit = limit;
            mLinkedList = new LinkedList<T>();
        }

        public int Limit
        {
            get { return mLimit; }
        }

        public int Count
        {
            get { return mLinkedList.Count; }
        }

        public bool IsEmpty
        {
            get { return mLinkedList.Count <= 0; }
        }

        public bool HasObject
        {
            get { return mLinkedList.Count > 0; }
        }

        private bool IsFull
        {
            get { return mLinkedList.Count > mLimit; }
        }

        public void Push(T item)
        {
            mLinkedList.AddFirst(item);
            if (IsFull) mLinkedList.RemoveLast();
            logDebug("push");
        }

        public T Pop()
        {
            if (HasObject)
            {
                T item = mLinkedList.First.Value;
                mLinkedList.RemoveFirst();
                logDebug("pop");
                return item;
            }
            else throw new InvalidOperationException(PopError);
        }

        public T Peek()
        {
            if (HasObject) return mLinkedList.First.Value;
            else throw new InvalidOperationException(PeekError);
        }

        public void Clear()
        {
            mLinkedList.Clear();
        }

        public List<T> ToList()
        {
            return mLinkedList.ToList();
        }

        [Conditional("LOGSTUFF")]
        private void logDebug(string op)
        {
            //long size = 0;
            //size = Misc.getObjectSize(mItems);
            //Log.add(op + ": count=" + mLinkedList.Count + ", size=" + size);

            Log.add(op + ": count=" + mLinkedList.Count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LimitedStack<T>.Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private LinkedList<T> GetLinkedList()
        {
            return mLinkedList;
        }

        private class Enumerator : IEnumerator<T>
        {
            private LinkedList<T> mLinkedList;
            private LinkedList<T>.Enumerator mLinkedListEnumerator;

            public Enumerator(LimitedStack<T> limitedStack)
            {
                mLinkedList = limitedStack.GetLinkedList();
                mLinkedListEnumerator = mLinkedList.GetEnumerator();
            }

            public T Current
            {
                get { return mLinkedListEnumerator.Current; }
            }

            public bool MoveNext()
            {
                return mLinkedListEnumerator.MoveNext();
            }

            public void Reset()
            {
                mLinkedListEnumerator = mLinkedList.GetEnumerator();
            }

            public void Dispose()
            {
                mLinkedListEnumerator.Dispose();
            }

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }

    //***********************************************************************************
    //***********************************************************************************

    //Using a circular buffer instead of a linked list isn't faster.
    //Probably is faster if need to randomly access items in stack though.
    class LimitedStackArrayTest<T>
    {
        private const string LimitError = "Limit need to be at least 1.";
        private const string PopError = "Cannot pop from empty stack.";
        private const string PeekError = "Cannot peek into empty stack.";

        private T[] mItems;
        private int mTop;
        private int mBottom;

        public LimitedStackArrayTest(int limit)
        {
            if (limit < 1) throw new ArgumentOutOfRangeException(LimitError);
            mItems = new T[limit + 1];
            mTop = 1;
            mBottom = 0;
        }

        public int Count
        {
            get
            {
                int count = mTop - mBottom - 1;
                if (count < 0) count += Length;
                return count;
            }
        }

        public int Capacity
        {
            get { return Length - 1; }
        }

        private int Length
        {
            get { return mItems.Length; }
        }

        private bool IsFull
        {
            get { return mTop == mBottom; }
        }

        public void Push(T item)
        {
            if (IsFull)
            {
                if (++mBottom >= Length) mBottom -= Length;
            }
            if (++mTop >= Length) mTop -= Length;
            mItems[mTop] = item;
            logDebug("push");
        }

        public T Pop()
        {
            if (Count > 0)
            {
                T item = mItems[mTop];
                mItems[mTop--] = default(T);
                if (mTop < 0) mTop += Length;
                logDebug("pop");
                return item;
            }
            else throw new InvalidOperationException(PopError);
        }

        public T Peek()
        {
            if (Count > 0)
            {
                return mItems[mTop];
            }
            else throw new InvalidOperationException(PeekError);
        }

        public void Clear()
        {
            if (Count > 0)
            {
                for (int i = 0; i < Length; i++)
                {
                    mItems[i] = default(T);
                }
                mTop = 1;
                mBottom = 0;
            }
        }

        [Conditional("LOGSTUFF")]
        private void logDebug(string op)
        {
            long size = 0;
            //size = Misc.getObjectSize(mItems);

            Log.add(op + ": top=" + mTop +
                ", bottom=" + mBottom + ", count=" + Count + ", size=" + size);
        }
    }
}
