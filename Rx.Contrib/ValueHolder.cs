namespace Rx.Contrib
{
    /// <summary>
    ///   Model wrapping a message yielded by an observable.
    /// </summary>
    /// <typeparam name="TSource">
    ///   The source type.
    /// </typeparam>
    public struct ValueHolder<TSource>
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ValueHolder{TSource}" /> struct.
        /// </summary>
        /// <param name="o">
        ///   Message wrapped by this struct.
        /// </param>
        /// <param name="ignore">
        ///   Determines if the message has to be ignored or not.
        /// </param>
        public ValueHolder(TSource o,
                           bool ignore = false)
            : this()
        {
            this.Value = o;
            this.Ignore = ignore;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ValueHolder{TSource}" /> struct.
        /// </summary>
        /// <param name="ignore">
        ///   Determines if the message has to be ignored or not.
        /// </param>
        public ValueHolder(bool ignore)
            : this()
        {
            this.Ignore = ignore;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether the message has to be ignored or not.
        /// </summary>
        public bool Ignore { get; private set; }

        /// <summary>
        ///   Gets the message wrapped.
        /// </summary>
        public TSource Value { get; private set; }

        #endregion
    }
}