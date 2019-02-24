using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public static class TaskExtention
    {
        public static async Task<TResult> DoAsync<TSource, TResult>(this Task<TSource> @this, Func<TSource, Task<TResult>> func)
        {
            return await func(await @this);
        }
    }
}
