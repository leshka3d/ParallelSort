using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ParallelArraySort
{
   public interface IQueue<T>
    {
        void Put(T n);
        void Next();
        T Get();
        T Look();
        int Length { get; }
    }
}
