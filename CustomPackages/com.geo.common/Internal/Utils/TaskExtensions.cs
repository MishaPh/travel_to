using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Geo.Common.Internal.Utils
{

    public static class TaskExtensions
    {
        public static void Forget(this Task task)
        {
            if (!task.IsCompleted || task.IsFaulted)
            {
                _ = ForgetAwaited(task);
            }

            return;

            static async Task ForgetAwaited(Task task)
            {
                try
                {
                    await task.ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
    }
}
