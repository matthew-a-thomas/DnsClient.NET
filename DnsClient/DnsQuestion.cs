namespace DnsClient
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Core;

    /// <summary>
    /// The <see cref="DnsQuestion"/> class transports information of the lookup query performed by <see cref="IDnsQuery"/>.
    /// <para>
    /// A list of questions is returned by <see cref="IDnsQueryResponse"/> (although, the list will always contain only one <see cref="DnsQuestion"/>).
    /// </para>
    /// </summary>
    public class DnsQuestion
    {
        /// <summary>
        /// Gets the domain name the lookup was runnig for.
        /// </summary>
        /// <value>
        /// The name of the query.
        /// </value>
        public DnsString QueryName { get; }

        /// <summary>
        /// Gets the question class.
        /// </summary>
        /// <value>
        /// The question class.
        /// </value>
        public QueryClass QuestionClass { get; }

        /// <summary>
        /// Gets the type of the question.
        /// </summary>
        /// <value>
        /// The type of the question.
        /// </value>
        public PseudoResourceRecordType QuestionType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DnsQuestion"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="questionType">Type of the question.</param>
        /// <param name="questionClass">The question class.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="query"/> is null.</exception>
        public DnsQuestion(string query, PseudoResourceRecordType questionType, QueryClass questionClass)
            : this(DnsString.Parse(query), questionType, questionClass)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DnsQuestion"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="questionType">Type of the question.</param>
        /// <param name="questionClass">The question class.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="query"/> is null.</exception>
        public DnsQuestion(DnsString query, PseudoResourceRecordType questionType, QueryClass questionClass)
        {
            QueryName = query ?? throw new ArgumentNullException(nameof(query));
            QuestionType = questionType;
            QuestionClass = questionClass;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToString(0);
        }

        /// <summary>
        /// Returns the information of this instance in a friendly format with an optional <paramref name="offset"/>.
        /// </summary>
        /// <param name="offset">The optional offset which can be used for pretty printing.</param>
        /// <returns>The string representation of this instance.</returns>
        [SuppressMessage("ReSharper", "FormatStringProblem")]
        public string ToString(int offset)
        {
            return string.Format("{0," + offset + "} \t{1} \t{2}", QueryName.Original, QuestionClass, QuestionType);
        }
    }
}