using System;
using System.Collections.Generic;
using System.Text;

namespace Paging
{
    public interface IPagingList<T> : IPagingList, IEnumerable<T>
    {

    }
}
