namespace DnsClient
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class TaskExtensions
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken, Action onCancel)
        {
            var tcs = new TaskCompletionSource<bool>();

            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                {
                    try
                    {
                        onCancel();
                    }
                    catch
                    {
                        // ignored
                    }

                    throw new OperationCanceledException(cancellationToken);
                }
            }

            return await task.ConfigureAwait(false);
        }
    }
}