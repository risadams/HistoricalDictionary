// ***********************************************************************
// Assembly         : HistoricalDictionary
// Author           : Ris Adams
// Created          : 03-17-2022
//
// Last Modified By : Ris Adams
// Last Modified On : 03-17-2022
// ***********************************************************************
// <copyright file="TimedValue.cs" company="Ris Adams">
//     Copyright © 2022 Ris Adams.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace HistoricalDictionary
{
  /// <summary>
  ///   Class TimedValue. This class cannot be inherited.
  ///   Implements the <see cref="System.IEquatable{HistoricalDictionary.TimedValue{TValue}}" />
  /// </summary>
  /// <typeparam name="TValue">The type of the t value.</typeparam>
  /// <seealso cref="System.IEquatable{HistoricalDictionary.TimedValue{TValue}}" />
  internal sealed class TimedValue<TValue> : IEquatable<TimedValue<TValue>>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="TimedValue{TValue}" /> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="asOf">As of.</param>
    public TimedValue(TValue value, DateTime asOf)
    {
      Value = value;
      AsOf  = asOf;
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="TimedValue{TValue}" /> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public TimedValue(TValue value) : this(value, DateTime.UtcNow) { }

    /// <summary>
    ///   Gets as of.
    /// </summary>
    /// <value>As of.</value>
    public DateTime AsOf { get; }

    /// <summary>
    ///   Gets the value.
    /// </summary>
    /// <value>The value.</value>
    public TValue Value { get; internal set; }

    /// <summary>
    ///   Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
    ///   <see langword="false" />.
    /// </returns>
    public bool Equals(TimedValue<TValue>? other)
    {
      if (other is null) return false;
      if (ReferenceEquals(this, other)) return true;
      return AsOf.Equals(other.AsOf) && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
    }

    /// <summary>
    ///   Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is TimedValue<TValue> other && Equals(other);

    /// <summary>
    ///   Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode() => HashCode.Combine(AsOf, Value);
  }
}
