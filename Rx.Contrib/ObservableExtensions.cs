namespace Rx.Contrib
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

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
        [PublicAPI]
        public static IObservable<TSource> Await<TSource>(this IObservable<Task<TSource>> source) => source.Concat();

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
        [PublicAPI]
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source) => new AwaitableSubscription<TSource>(source);

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
        [PublicAPI]
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
                                                                            Action<TSource> onNext) =>
            new AwaitableSubscription<TSource>(source, onNext);

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
        [PublicAPI]
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
                                                                            Action<TSource> onNext,
                                                                            Action<Exception> onError) =>
            new AwaitableSubscription<TSource>(source, onNext, onError);

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
        [PublicAPI]
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
                                                                            Action<TSource> onNext,
                                                                            Action onCompleted) =>
            new AwaitableSubscription<TSource>(source, onNext, onCompleted);

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
        [PublicAPI]
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
                                                                            Action<TSource> onNext,
                                                                            Action<Exception> onError,
                                                                            Action onCompleted) =>
            new AwaitableSubscription<TSource>(source, onNext, onError, onCompleted);

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
        [PublicAPI]
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
                                                                            IObserver<TSource> observer,
                                                                            CancellationToken token) =>
            new AwaitableSubscription<TSource>(source, observer, token);

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
        [PublicAPI]
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
                                                                            CancellationToken token) =>
            new AwaitableSubscription<TSource>(source, token);

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
        [PublicAPI]
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
                                                                            Action<TSource> onNext,
                                                                            Action<Exception> onError,
                                                                            CancellationToken token) =>
            new AwaitableSubscription<TSource>(source, onNext, onError, token);

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
        [PublicAPI]
        public static IAwaitableSubscription AwaitableSubscription<TSource>(this IObservable<TSource> source,
                                                                            Action<TSource> onNext,
                                                                            Action<Exception> onError,
                                                                            Action onCompleted,
                                                                            CancellationToken token) =>
            new AwaitableSubscription<TSource>(source, onNext, onError, onCompleted, token);

        /// <summary>
        ///     Blocks the completion of the <paramref name="source"/> until the <paramref name="task"/>
        ///     is finished.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to block.
        /// </param>
        /// <param name="task">
        ///     Task to wait for.
        /// </param>
        /// <returns>
        ///     A new <see cref="IObservable{TSource}"/> which completes when the <paramref name="source"/>
        ///     and <paramref name="task"/> are finished.
        /// </returns>
        [PublicAPI]
        public static IObservable<TSource> BlockUntil<TSource>(this IObservable<TSource> source,
                                                               Task task) =>
            Observable.Create<TSource>(observer =>
                                           {
                                               var tcs = new TaskCompletionSource<ValueHolder<TSource>>(TaskCreationOptions.RunContinuationsAsynchronously);

                                               Exception delayedException = null;

                                               return source.Select(o => new ValueHolder<TSource>(o))
                                                            .Finally(() => task.ContinueWith(t =>
                                                                                                 {
                                                                                                     if (delayedException != null)
                                                                                                     {
                                                                                                         tcs.SetException(delayedException);
                                                                                                         return;
                                                                                                     }

                                                                                                     if (t.Exception != null)
                                                                                                     {
                                                                                                         tcs.SetException(t.Exception);
                                                                                                         return;
                                                                                                     }

                                                                                                     tcs.SetResult(new ValueHolder<TSource>(true));
                                                                                                 }))
                                                            .Catch<ValueHolder<TSource>, Exception>(ex =>
                                                                                                        {
                                                                                                            delayedException = ex;
                                                                                                            return tcs.Task.ToObservable();
                                                                                                        })
                                                            .Concat(tcs.Task.ToObservable())
                                                            .Where(o => !o.Ignore)
                                                            .Select(o => o.Value)
                                                            .Subscribe(observer);
                                           });

        /// <summary>
        ///     Concatenates <paramref name="observables"/> sequence to each other sequence until an <see cref="IObservable{TSource}"/> yields at least 1 message.
        /// </summary>
        /// <param name="observables">
        ///     <see cref="IObservable{TSource}"/> to be concatenated.
        /// </param>
        /// <typeparam name="TSource">
        ///     The generic type of the <paramref name="observables"/>.
        /// </typeparam>
        /// <returns>
        ///     An <see cref="IObservable{TSource}"/> sequence that contains the elements of the first sequence yielding at least one message, followed by those of next sequences.
        /// </returns>
        [PublicAPI]
        public static IObservable<TSource> ConcatUntilAny<TSource>(this IEnumerable<IObservable<TSource>> observables) =>
            observables.Select(obs => obs.Select(item => new
                                                             {
                                                                 IsEndMarker = false,
                                                                 Item = item
                                                             })
                                         .Concat(Observable.Return(new { IsEndMarker = true, Item = default(TSource) }))
                                         .Select((t,
                                                  i) => new
                                                            {
                                                                Index = i,
                                                                t.IsEndMarker,
                                                                t.Item
                                                            }))
                       .Concat()
                       .TakeUntil(x => x.Index > 0 && x.IsEndMarker)
                       .Where(t => !t.IsEndMarker)
                       .Select(t => t.Item);

        /// <summary>
        ///   Starts an observable with an <paramref name="interval" /> yielding the first message at the beginning.
        /// </summary>
        /// <param name="interval">
        ///   Period for producing the values in the resulting sequence. If this value is equal to TimeSpan.Zero, the timer will
        ///   recur as fast as possible.
        /// </param>
        /// <param name="scheduler">
        ///   Represents an object that schedules units of work.
        /// </param>
        /// <returns>
        ///   An observable sequence that produces a value at the beginning and after each period.
        /// </returns>
        [PublicAPI]
        public static IObservable<long> IntervalStartAtBeginning(TimeSpan interval, IScheduler scheduler) => Observable.Return(0L).Concat(Observable.Interval(interval, scheduler)).Select(x => x + 1);

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
        [PublicAPI]
        public static IObservable<long> IntervalStartAtBeginning(TimeSpan interval) => IntervalStartAtBeginning(interval, Scheduler.Default);

        /// <summary>
        ///     Repeats the source observable sequence until it successfully terminates when the <typeparamref name="TException"/> occurred.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TException">
        ///     The exception type which will be caught.
        /// </typeparam>
        /// <param name="source">
        ///     Observable sequence to repeat until it successfully terminates.
        /// </param>
        /// <returns>
        ///     An observable sequence producing the elements of the given sequence repeatedly until it terminates successfully when the <typeparamref name="TException"/> occurred.
        /// </returns>
        [PublicAPI]
        public static IObservable<TSource> Retry<TSource, TException>(this IObservable<TSource> source)
            where TException : Exception
        {
            var observable = Observable.Empty<TSource>();

            // ReSharper disable once AccessToModifiedClosure
            observable = source.Catch<TSource, TException>(_ => observable);

            return observable;
        }

        /// <summary>
        ///     Retries if there is an <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TException">
        ///     The type of exception on which it should retry.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to retry in case of an <typeparamref name="TException"/>.
        /// </param>
        /// <param name="where">
        ///     Filter is applied to caught exceptions.
        /// </param>
        /// <returns>
        ///     An observable sequence producing the elements of the given sequence repeatedly until it terminates successfully or with a different exception.
        /// </returns>
        [PublicAPI]
        public static IObservable<TSource> Retry<TSource, TException>(this IObservable<TSource> source,
                                                                      Func<TException, bool> where)
            where TException : Exception
        {
            IObservable<TSource> observable = null;

            // ReSharper disable once AccessToModifiedClosure
            observable = source.Catch<TSource, TException>(ex =>
                                                               {
                                                                   if (where(ex))
                                                                   {
                                                                       return observable;
                                                                   }

                                                                   return Observable.Throw<TSource>(ex);
                                                               });

            return observable;
        }

        /// <summary>
        ///     Retries if there is an <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TException">
        ///     The type of exception on which it should retry.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to retry in case of an <typeparamref name="TException"/>.
        /// </param>
        /// <param name="maxRetry">
        ///     How many times to retry.
        /// </param>
        /// <returns>
        ///     An observable sequence producing the elements of the given sequence repeatedly until it terminates successfully or with a different exception.
        /// </returns>
        [PublicAPI]
        public static IObservable<TSource> Retry<TSource, TException>(this IObservable<TSource> source,
                                                                      int maxRetry)
            where TException : Exception =>
            source.Retry<TSource, TException>(maxRetry, TimeSpan.Zero);

        /// <summary>
        ///     Retries if there is an <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TException">
        ///     The type of exception on which it should retry.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to retry in case of an <typeparamref name="TException"/>.
        /// </param>
        /// <param name="maxRetry">
        ///     How many times to retry.
        /// </param>
        /// <param name="after">
        ///     The delay when the observable will be re-executed.
        /// </param>
        /// <param name="condition">
        ///    The predicate to determine whether you have to retry.
        /// </param>
        /// <returns>
        ///     An observable sequence producing the elements of the given sequence repeatedly until it terminates successfully or with a different exception.
        /// </returns>
        public static IObservable<TSource> Retry<TSource, TException>(this IObservable<TSource> source,
                                                                      int maxRetry,
                                                                      TimeSpan after,
                                                                      Predicate<TException> condition)
            where TException : Exception =>
            source.Retry(maxRetry,
                         after,
                         condition,
                         Scheduler.Default);

        /// <summary>
        ///     Retries if there is an <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TException">
        ///     The type of exception on which it should retry.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to retry in case of an <typeparamref name="TException"/>.
        /// </param>
        /// <param name="maxRetry">
        ///     How many times to retry.
        /// </param>
        /// <param name="after">
        ///     The delay when the observable will be re-executed.
        /// </param>
        /// <param name="condition">
        ///    The predicate to determine whether you have to retry.
        /// </param>
        /// <param name="scheduler">
        ///    The scheduler to run the timer on.
        /// </param>
        /// <returns>
        ///     An observable sequence producing the elements of the given sequence repeatedly until it terminates successfully or with a different exception.
        /// </returns>
        [PublicAPI]
        public static IObservable<TSource> Retry<TSource, TException>(this IObservable<TSource> source,
                                                                      int maxRetry,
                                                                      TimeSpan after,
                                                                      Predicate<TException> condition,
                                                                      IScheduler scheduler)
            where TException : Exception =>
            Observable.Create<TSource>(o =>
                                           {
                                               IObservable<TSource> observable = null;
                                               var retryCount = maxRetry;
                                               observable = source.Catch<TSource, TException>(x =>
                                                                                                  {
                                                                                                      if (!condition(x))
                                                                                                      {
                                                                                                          return Observable.Throw<TSource>(x, scheduler);
                                                                                                      }

                                                                                                      if (retryCount == 0)
                                                                                                      {
                                                                                                          return Observable.Throw<TSource>(x, scheduler);
                                                                                                      }

                                                                                                      --retryCount;

                                                                                                      // ReSharper disable once AccessToModifiedClosure
                                                                                                      return Observable.Timer(after, scheduler).Select(_ => observable).Concat();
                                                                                                  });

                                               return observable.Subscribe(o);
                                           });

        /// <summary>
        ///     Retries if there is an <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TException">
        ///     The type of exception on which it should retry.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to retry in case of an <typeparamref name="TException"/>.
        /// </param>
        /// <param name="maxRetry">
        ///     How many times to retry.
        /// </param>
        /// <param name="after">
        ///     The delay when the observable will be re-executed.
        /// </param>
        /// <returns>
        ///     An observable sequence producing the elements of the given sequence repeatedly until it terminates successfully or with a different exception.
        /// </returns>
        [PublicAPI]
        public static IObservable<TSource> Retry<TSource, TException>(this IObservable<TSource> source, int maxRetry, TimeSpan after)
            where TException : Exception =>
            source.Retry<TSource, TException>(maxRetry,
                                              after,
                                              Scheduler.Default);

        /// <summary>
        ///     Retries if there is an <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TException">
        ///     The type of exception on which it should retry.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to retry in case of an <typeparamref name="TException"/>.
        /// </param>
        /// <param name="maxRetry">
        ///     How many times to retry.
        /// </param>
        /// <param name="after">
        ///     The delay when the observable will be re-executed.
        /// </param>
        /// <param name="scheduler">
        ///     The scheduler to run the timer on.
        /// </param>
        /// <returns>
        ///     An observable sequence producing the elements of the given sequence repeatedly until it terminates successfully or with a different exception.
        /// </returns>
        [PublicAPI]
        public static IObservable<TSource> Retry<TSource, TException>(this IObservable<TSource> source,
                                                                      int maxRetry,
                                                                      TimeSpan after,
                                                                      IScheduler scheduler)
            where TException : Exception =>
            Observable.Create<TSource>(o =>
                                           {
                                               IObservable<TSource> observable = null;
                                               var retryCount = maxRetry;
                                               observable = source.Catch<TSource, TException>(x =>
                                                                                                  {
                                                                                                      if (retryCount == 0)
                                                                                                      {
                                                                                                          return Observable.Throw<TSource>(x, scheduler);
                                                                                                      }

                                                                                                      retryCount--;

                                                                                                      return after != TimeSpan.Zero
                                                                                                                 // ReSharper disable once AccessToModifiedClosure
                                                                                                                 ? Observable.Timer(after, scheduler).Select(_ => observable).Concat()
                                                                                                                 // ReSharper disable once AccessToModifiedClosure
                                                                                                                 : observable;
                                                                                                  });

                                               return observable.Subscribe(o);
                                           });

        /// <summary>
        ///     Projects each element of an observable sequence into a new form, new source sequence elements cancel old uncompleted tasks returned by the previous call to <paramref name="selector"/> and filters uncompleted transformations.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///     A sequence of elements to invoke a transform function on.
        /// </param>
        /// <param name="selector">
        ///     A transform function to apply to each source element.
        /// </param>
        /// <param name="scheduler">
        ///     Represents an object that schedules units of work.
        /// </param>
        /// <returns>
        ///     An observable sequence whose elements are the result of invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="source"/> or <paramref name="selector"/> is null.
        /// </exception>
        [PublicAPI]
        public static IObservable<TResult> SelectAsyncCancelling<TSource, TResult>(this IObservable<TSource> source,
                                                                                   Func<CancellationToken, TSource, Task<TResult>> selector,
                                                                                   IScheduler scheduler) =>
            source.Select(o => Observable.FromAsync(cancellationToken => selector(cancellationToken, o), scheduler)).Switch();

        /// <summary>
        ///     Projects each element of an observable sequence into a new form, new source sequence elements cancel old uncompleted tasks returned by the previous call to <paramref name="selector"/> and filters uncompleted transformations.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///     A sequence of elements to invoke a transform function on.
        /// </param>
        /// <param name="selector">
        ///     A transform function to apply to each source element.
        /// </param>
        /// <returns>
        ///     An observable sequence whose elements are the result of invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="source"/> or <paramref name="selector"/> is null.
        /// </exception>
        [PublicAPI]
        public static IObservable<TResult> SelectAsyncCancelling<TSource, TResult>(this IObservable<TSource> source,
                                                                                   Func<CancellationToken, TSource, Task<TResult>> selector) =>
            source.SelectAsyncCancelling(selector, Scheduler.Default);

        /// <summary>
        ///     Projects each element of an observable sequence into a new form, <paramref name="selector"/> is executed in parallel.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam><param name="source">A sequence of elements to invoke a transform function on.
        /// </param>
        /// <param name="selector">
        ///     A transform function to apply to each source element.
        /// </param>
        /// <param name="scheduler">
        ///     Represents an object that schedules units of work.
        /// </param>
        /// <returns>
        ///     An observable sequence whose elements are the result of invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="source"/> or <paramref name="selector"/> is null.
        /// </exception>
        [PublicAPI]
        public static IObservable<TResult> SelectAsyncParallel<TSource, TResult>(this IObservable<TSource> source,
                                                                                 Func<TSource, CancellationToken, Task<TResult>> selector,
                                                                                 IScheduler scheduler) =>
            source.Select(x => Observable.FromAsync(ct => selector(x, ct), scheduler))
                  .Merge();

        /// <summary>
        ///     Projects each element of an observable sequence into a new form, <paramref name="selector"/> is executed in parallel.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam><param name="source">A sequence of elements to invoke a transform function on.
        /// </param>
        /// <param name="selector">
        ///     A transform function to apply to each source element.
        /// </param>
        /// <returns>
        ///     An observable sequence whose elements are the result of invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="source"/> or <paramref name="selector"/> is null.
        /// </exception>
        [PublicAPI]
        public static IObservable<TResult> SelectAsyncParallel<TSource, TResult>(this IObservable<TSource> source,
                                                                                 Func<TSource, CancellationToken, Task<TResult>> selector) =>
            source.SelectAsyncParallel(selector, Scheduler.Default);

        /// <summary>
        ///     Projects each element of an observable sequence into a new form, source sequence elements are queued until the task returned by the previous call to <paramref name="selector"/> is completed.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam><param name="source">A sequence of elements to invoke a transform function on.
        /// </param>
        /// <param name="selector">
        ///     A transform function to apply to each source element.
        /// </param>
        /// <param name="scheduler">
        ///     Represents an object that schedules units of work.
        /// </param>
        /// <returns>
        ///     An observable sequence whose elements are the result of invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="source"/> or <paramref name="selector"/> is null.
        /// </exception>
        [PublicAPI]
        public static IObservable<TResult> SelectAsyncSequentially<TSource, TResult>(this IObservable<TSource> source,
                                                                                     Func<TSource, CancellationToken, Task<TResult>> selector,
                                                                                     IScheduler scheduler) =>
            source.Select(x => Observable.FromAsync(ct => selector(x, ct), scheduler))
                  .Concat();

        /// <summary>
        ///     Projects each element of an observable sequence into a new form, source sequence elements are queued until the task returned by the previous call to <paramref name="selector"/> is completed.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam><param name="source">A sequence of elements to invoke a transform function on.
        /// </param>
        /// <param name="selector">
        ///     A transform function to apply to each source element.
        /// </param>
        /// <returns>
        ///     An observable sequence whose elements are the result of invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="source"/> or <paramref name="selector"/> is null.
        /// </exception>
        [PublicAPI]
        public static IObservable<TResult> SelectAsyncSequentially<TSource, TResult>(this IObservable<TSource> source,
                                                                                     Func<TSource, CancellationToken, Task<TResult>> selector) =>
            source.SelectAsyncSequentially(selector, Scheduler.Default);

        /// <summary>
        ///   Projects each element of an observable sequence into a new form, source sequence elements are ignored until
        ///   the task returned by the previous call to <paramref name="selector" /> is completed.
        /// </summary>
        /// <typeparam name="TSource">
        ///   The type of the elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///   The type of the elements in the result sequence, obtained by running the selector function for each element in the
        ///   source sequence.
        /// </typeparam>
        /// <param name="source">
        ///   A sequence of elements to invoke a transform function on.
        /// </param>
        /// <param name="selector">
        ///   A transform function to apply to each source element.
        /// </param>
        /// <returns>
        ///   An observable sequence whose elements are the result of invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="source" /> or <paramref name="selector" /> is null.
        /// </exception>
        [PublicAPI]
        public static IObservable<TResult> SelectAsyncSkipping<TSource, TResult>(this IObservable<TSource> source,
                                                                                 Func<CancellationToken, TSource, Task<TResult>> selector) =>
            Observable.Create<TResult>(o =>
                                           {
                                               var cancellationTokenSource = new CancellationTokenSource();
                                               var observable = source.Scan(new ValueHolder<Task<TResult>>(),
                                                                            (x,
                                                                             y) =>
                                                                                {
                                                                                    if (x.Value == null || x.Value.IsCompleted)
                                                                                    {
                                                                                        return new ValueHolder<Task<TResult>>(selector(cancellationTokenSource.Token, y));
                                                                                    }

                                                                                    return new ValueHolder<Task<TResult>>(x.Value, true);
                                                                                })
                                                                      .Where(vh => !vh.Ignore)
                                                                      .Select(vh => vh.Value)
                                                                      .Concat();

                                               return new CompositeDisposable(cancellationTokenSource, observable.Subscribe(o));
                                           });

        /// <summary>
        ///     Subscribes to an observable which was created with <see cref="CreateWithCancellationSupport{T}"/>.
        ///     It sends an cancellation request on disposal and waits for the observable to complete.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to subscribe to.
        /// </param>
        /// <returns>
        ///     An async disposable which sends an cancellation request on disposal and waits for the observable to complete.
        /// </returns>
        [PublicAPI]
        public static IAsyncDisposable SubscribeWithCancellationSupport<TSource>(this IObservable<TSource> source) => InternalSubscribeWithCancellationSupport(source, (token, tcs) => new ObserverWithCancellationSupport<TSource>(token, tcs));

        /// <summary>
        ///     Subscribes to an observable which was created with <see cref="CreateWithCancellationSupport{T}"/>.
        ///     It sends an cancellation request on disposal and waits for the observable to complete.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to subscribe to.
        /// </param>
        /// <param name="onNext">
        ///     Action to invoke for each element in the observable sequence.
        /// </param>
        /// <returns>
        ///     An async disposable which sends an cancellation request on disposal and waits for the observable to complete.
        /// </returns>
        [PublicAPI]
        public static IAsyncDisposable SubscribeWithCancellationSupport<TSource>(this IObservable<TSource> source,
                                                                                 Action<TSource> onNext) =>
            InternalSubscribeWithCancellationSupport(source, (token, tcs) => new ObserverWithCancellationSupport<TSource>(token, tcs, onNext));

        /// <summary>
        ///     Subscribes to an observable which was created with <see cref="CreateWithCancellationSupport{T}"/>.
        ///     It sends an cancellation request on disposal and waits for the observable to complete.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to subscribe to.
        /// </param>
        /// <param name="onNext">
        ///     Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onError">
        ///     Action to invoke upon exceptional termination of the observable sequence.
        /// </param>
        /// <returns>
        ///     An async disposable which sends an cancellation request on disposal and waits for the observable to complete.
        /// </returns>
        [PublicAPI]
        public static IAsyncDisposable SubscribeWithCancellationSupport<TSource>(this IObservable<TSource> source,
                                                                                 Action<TSource> onNext,
                                                                                 Action<Exception> onError) =>
            InternalSubscribeWithCancellationSupport(source, (token, tcs) => new ObserverWithCancellationSupport<TSource>(token, tcs, onNext, onError));

        /// <summary>
        ///     Subscribes to an observable which was created with <see cref="CreateWithCancellationSupport{T}"/>.
        ///     It sends an cancellation request on disposal and waits for the observable to complete.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to subscribe to.
        /// </param>
        /// <param name="onNext">
        ///     Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onCompleted">
        ///     Action to invoke upon graceful termination of the observable sequence.
        /// </param>
        /// <returns>
        ///     An async disposable which sends an cancellation request on disposal and waits for the observable to complete.
        /// </returns>
        [PublicAPI]
        public static IAsyncDisposable SubscribeWithCancellationSupport<TSource>(this IObservable<TSource> source,
                                                                                 Action<TSource> onNext,
                                                                                 Action onCompleted) =>
            InternalSubscribeWithCancellationSupport(source, (token, tcs) => new ObserverWithCancellationSupport<TSource>(token, tcs, onNext, null, onCompleted));

        /// <summary>
        ///     Subscribes to an observable which was created with <see cref="CreateWithCancellationSupport{T}"/>.
        ///     It sends an cancellation request on disposal and waits for the observable to complete.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to subscribe to.
        /// </param>
        /// <param name="onNext">
        ///     Action to invoke for each element in the observable sequence.
        /// </param>
        /// <param name="onError">
        ///     Action to invoke upon exceptional termination of the observable sequence.
        /// </param>
        /// <param name="onCompleted">
        ///     Action to invoke upon graceful termination of the observable sequence.
        /// </param>
        /// <returns>
        ///     An async disposable which sends an cancellation request on disposal and waits for the observable to complete.
        /// </returns>
        [PublicAPI]
        public static IAsyncDisposable SubscribeWithCancellationSupport<TSource>(this IObservable<TSource> source,
                                                                                 Action<TSource> onNext,
                                                                                 Action<Exception> onError,
                                                                                 Action onCompleted) =>
            InternalSubscribeWithCancellationSupport(source, (token, tcs) => new ObserverWithCancellationSupport<TSource>(token, tcs, onNext, onError, onCompleted));

        /// <summary>
        ///     Returns the elements from the source observable sequence until the <paramref name="predicate"/> is true. 
        ///     In difference to TakeWhile this method returns the item which matches the predicate.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements in the source sequence.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence to propagate elements for.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <returns>
        ///     An observable sequence containing the elements of the source sequence up to the point the other sequence interrupted further propagation.
        /// </returns>
        [PublicAPI]
        public static IObservable<TSource> TakeUntil<TSource>(this IObservable<TSource> source,
                                                              Func<TSource, bool> predicate) =>
            Observable.Create<TSource>(o =>
                                           {
                                               var tcs = new TaskCompletionSource<ValueHolder<TSource>>(TaskCreationOptions.RunContinuationsAsynchronously);
                                               var observableTask = tcs.Task.ToObservable();

                                               return source.Do(x =>
                                                                    {
                                                                        if (predicate(x))
                                                                        {
                                                                            tcs.SetResult(new ValueHolder<TSource>(x));
                                                                        }
                                                                    })
                                                            .Finally(() => tcs.TrySetResult(new ValueHolder<TSource>(true))) // INFO: [lmbbub1 31.05.16 08:35] Ensure that the task is set to complete even when the predicate is never met.
                                                            .Select(x => new ValueHolder<TSource>(x))
                                                            .TakeWhile(x => !tcs.Task.IsCompleted)
                                                            .Concat(observableTask)
                                                            .Where(vh => !vh.Ignore)
                                                            .Select(vh => vh.Value)
                                                            .Subscribe(o);
                                           });

        /// <summary>
        ///     Creates an observable sequence with the possibility to get a subscription where the observable can be notified to stop producing items.
        ///     Use any of the <see cref="SubscribeWithCancellationSupport{TSource}(System.IObservable{TSource})"/> methods to get an <see cref="IAsyncDisposable"/>.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type of items which is returned by the new observable.
        /// </typeparam>
        /// <param name="subscribe">
        ///     The method which is called upon subscription.
        /// </param>
        /// <returns>
        ///     An observable sequence with the possibility to get a subscription where the observable can be notified to stop producing items.
        /// </returns>
        [PublicAPI]
        public static IObservable<TResult> CreateWithCancellationSupport<TResult>(Action<IObserver<TResult>, CancellationToken> subscribe) =>
            Observable.Create<TResult>(obs =>
                                           {
                                               var baseObserver = GetBaseObserver(obs);
                                               var token = baseObserver.Token;
                                               subscribe(obs, token);
                                               return Disposable.Create(baseObserver,
                                                                        x =>
                                                                            {
                                                                                if (!x.IsCompleted)
                                                                                {
                                                                                    throw new InvalidOperationException("Don't unsubscribe!");
                                                                                }
                                                                            });
                                           });

        #endregion

        #region Private Members

        private static IAsyncDisposable InternalSubscribeWithCancellationSupport<TSource>(IObservable<TSource> source,
                                                                                          Func<CancellationToken, TaskCompletionSource<object>, ObserverWithCancellationSupport<TSource>> observerFactory)
        {
            var taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            var cancellationTokenSource = new CancellationTokenSource();

            source.Subscribe(observerFactory(cancellationTokenSource.Token, taskCompletionSource));

            return AsyncDisposable.Create(() =>
                                              {
                                                  cancellationTokenSource.Cancel();
                                                  return new ValueTask(taskCompletionSource.Task);
                                              });
        }

        private static ObserverWithCancellationSupport<T> GetBaseObserver<T>(IObserver<T> observer)
        {
            var observerField = observer.GetType().GetTypeInfo().GetField("observer", BindingFlags.Instance | BindingFlags.NonPublic);
            Debug.Assert(observerField != null, "Apparently the implementation of Rx.NET changed");
            return (ObserverWithCancellationSupport<T>)observerField.GetValue(observer);
        }

        #endregion
    }
}