using System;
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
