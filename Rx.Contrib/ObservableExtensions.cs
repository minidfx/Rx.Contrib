namespace Rx.Contrib
{
    using System;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public static class ObservableExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Returns an <see cref="IObservable{TSource}" /> containing the result of the <paramref name="source" /> task.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   The <see cref="Task" /> representing an asynchronous operation.
        /// </param>
        /// <returns>
        ///   A new <see cref="IObservable{TSource}" /> which completes when the <paramref name="source" /> task is finished.
        /// </returns>
        public static IObservable<TSource> Await<TSource>(this IObservable<Task<TSource>> source)
        {
            return source.Select(t => t.GetAwaiter().GetResult());
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the result of
        ///   <paramref name="subscribeAction" /> on the <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="subscribeAction">
        ///   Function returning the original subscription on the <paramref name="source" />.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAsyncDisposable Awaitable<TSource>(this IObservable<TSource> source,
                                                          Func<IObservable<TSource>, IDisposable> subscribeAction)
        {
            return new AwaitableSubscription<TSource>(source, subscribeAction);
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the result of the
        ///   <paramref name="subscribeAction" /> on the <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="subscribeAction">
        ///   <see cref="Action" /> which subscribe to the <paramref name="source" /> with an <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAsyncDisposable Awaitable<TSource>(this IObservable<TSource> source,
                                                          Action<IObservable<TSource>> subscribeAction)
        {
            return new AwaitableSubscription<TSource>(source, subscribeAction);
        }

        #endregion
    }
}