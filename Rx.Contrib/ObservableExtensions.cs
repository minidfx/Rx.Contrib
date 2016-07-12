using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rx.Contrib
{
    /// <summary>
    ///   Add some useful methods for Rx.
    /// </summary>
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
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the subscription on the
        ///   <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source)
        {
            return new AwaitableSubscription<TSource>(source);
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the subscription on the
        ///   <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///   Action to invoke for each element in the observable sequence.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
            Action<TSource> onNext)
        {
            return new AwaitableSubscription<TSource>(source, onNext);
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the subscription on the
        ///   <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///   Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onError">
        ///   Action to invoke upon exceptional termination of the observable sequence.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
            Action<TSource> onNext,
            Action<Exception> onError)
        {
            return new AwaitableSubscription<TSource>(source, onNext, onError);
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the subscription on the
        ///   <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///   Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onCompleted">
        ///   Action to invoke upon graceful termination of the observable sequence.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
            Action<TSource> onNext,
            Action onCompleted)
        {
            return new AwaitableSubscription<TSource>(source, onNext, onCompleted);
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the subscription on the
        ///   <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///   Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onError">
        ///   Action to invoke upon exceptional termination of the observable sequence.
        /// </param>
        /// <param name="onCompleted">
        ///   Action to invoke upon graceful termination of the observable sequence.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
            Action<TSource> onNext,
            Action<Exception> onError,
            Action onCompleted)
        {
            return new AwaitableSubscription<TSource>(source, onNext, onError, onCompleted);
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the subscription on the
        ///   <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="observer">
        ///   Observer to subscribe to the sequence.
        /// </param>
        /// <param name="token">
        ///   CancellationToken that can be signaled to unsubscribe from the source sequence.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
            IObserver<TSource> observer,
            CancellationToken token)
        {
            return new AwaitableSubscription<TSource>(source, observer, token);
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the subscription on the
        ///   <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="token">
        ///   CancellationToken that can be signaled to unsubscribe from the source sequence.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
            CancellationToken token)
        {
            return new AwaitableSubscription<TSource>(source, token);
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the subscription on the
        ///   <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///   Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="token">
        ///   CancellationToken that can be signaled to unsubscribe from the source sequence.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
            Action<TSource> onNext,
            CancellationToken token)
        {
            return new AwaitableSubscription<TSource>(source, onNext, token);
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the subscription on the
        ///   <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///   Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onError">
        ///   Action to invoke upon exceptional termination of the observable sequence.
        /// </param>
        /// <param name="token">
        ///   CancellationToken that can be signaled to unsubscribe from the source sequence.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
            Action<TSource> onNext,
            Action<Exception> onError,
            CancellationToken token)
        {
            return new AwaitableSubscription<TSource>(source, onNext, onError, token);
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the subscription on the
        ///   <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///   Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onCompleted">
        ///   Action to invoke upon graceful termination of the observable sequence.
        /// </param>
        /// <param name="token">
        ///   CancellationToken that can be signaled to unsubscribe from the source sequence.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
            Action<TSource> onNext,
            Action onCompleted,
            CancellationToken token)
        {
            return new AwaitableSubscription<TSource>(source, onNext, onCompleted, token);
        }

        /// <summary>
        ///   Returns an <see cref="IAsyncDisposable" /> that you can wait for until the subscription on the
        ///   <paramref name="source" /> is disposed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   Source sequence to propagate elements for.
        /// </param>
        /// <param name="onNext">
        ///   Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onError">
        ///   Action to invoke upon exceptional termination of the observable sequence.
        /// </param>
        /// <param name="onCompleted">
        ///   Action to invoke upon graceful termination of the observable sequence.
        /// </param>
        /// <param name="token">
        ///   CancellationToken that can be signaled to unsubscribe from the source sequence.
        /// </param>
        /// <returns>
        ///   An instance of <see cref="IAsyncDisposable" /> awaitable.
        /// </returns>
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
            Action<TSource> onNext,
            Action<Exception> onError,
            Action onCompleted,
            CancellationToken token)
        {
            return new AwaitableSubscription<TSource>(source, onNext, onError, onCompleted, token);
        }

        /// <summary>
        ///   Starts an observable with an <paramref name="interval" /> yielding the first message at the beginning.
        /// </summary>
        /// <param name="interval">
        ///   Period for producing the values in the resulting sequence. If this value is equal to TimeSpan.Zero, the timer will
        ///   recur as fast as possible.
        /// </param>
        /// <returns>
        ///   An observable sequence that produces a value at the beginning and after each period.
        /// </returns>
        public static IObservable<long> IntervalStartAtBegining(TimeSpan interval)
        {
            return Observable.Return(0L).Concat(Observable.Interval(interval)).Select(x => x + 1);
        }

        #endregion
    }
}