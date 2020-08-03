using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ParallelArraySort
{
   
   public class ArrayQueue<T> : IQueue<T> 
    {
        int _reuseIndex = 0;
        readonly int _chunkSize ;

        int _readArr = 0;
        int _readPos = 0;

        int _putArr = 0;
        int _putPos = 0;

        T[][] _data = new T[33][];

       // ArrayPool<int> arrayPool;
        public int Length
        {
            get { return (_putArr - _readArr) * _chunkSize + _putPos - _readPos; }
        }
        public ArrayQueue(int n)
        {
            if (n < 128) n = 128; 
            _chunkSize = n >> 4;
            _data[0] = new T[_chunkSize];

            //arrayPool = ArrayPool<int>.Create(_chunkSize, 4);
            //_data[0] = arrayPool.Rent(_chunkSize);
        }
        
        public void Next ()
        {
            _readPos++; 
            if (_readPos >= _chunkSize)
            {
                _readPos = 0;
                //arrayPool.Return(_data[_readArr]);
                // _data[ReadArr] = null; 
                _readArr++; 
            }
            
        }
        public T Get ()
        {          
            var r = _data[_readArr][_readPos];
            Next();
            return r;
        }

        public T Look ()
        {
            return _data[_readArr][_readPos];
        }
        public void Put (T n)
        {
            _data[_putArr][_putPos] = n;
            _putPos++;
            if (_putPos >= _chunkSize)
            {
                _putPos = 0;
                _putArr++;
                //_data[_putArr] = arrayPool.Rent (_chunkSize);
                 if ((_reuseIndex < _readArr))
                  {
                      _data[_putArr] = _data[_reuseIndex];
                      _reuseIndex++;                  
                  }
                  else 
                  {
                      _data[_putArr] = new T[_chunkSize];
                  }
            }
        }

    }
}
