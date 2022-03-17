// ***********************************************************************
// Assembly         : HistoricalDictionary
// Author           : Ris Adams
// Created          : 03-17-2022
//
// Last Modified By : Ris Adams
// Last Modified On : 03-17-2022
// ***********************************************************************
// <copyright file="HistoricalDictionary.cs" company="Ris Adams">
//     Copyright © 2022 Ris Adams.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace HistoricalDictionary
{
  /// <summary>
  ///   Class HistoricalDictionary.
  /// </summary>
  /// <typeparam name="TKey">The type of the t key.</typeparam>
  /// <typeparam name="TValue">The type of the t value.</typeparam>
  public class HistoricalDictionary<TKey, TValue> where TKey : notnull
  {
    /// <summary>
    ///   The base
    /// </summary>
    private readonly Dictionary<TKey, IList<TimedValue<TValue>>> _base = new();

    /// <summary>
    ///   Adds the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Add(TKey key, TValue value) => Add(key, value, DateTime.UtcNow);

    /// <summary>
    ///   Adds a value for specified key.
    ///   if the asOf date already exists, its existing value will be replaced.
    ///   otherwise the new value will be appended
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="asOf">As of.</param>
    /// <exception cref="System.ArgumentNullException">key</exception>
    public void Add(TKey key, TValue value, DateTime asOf)
    {
      if (key == null) throw new ArgumentNullException(nameof(key));
      if (!_base.ContainsKey(key)) _base.Add(key, new List<TimedValue<TValue>>());
      foreach (var timedValue in _base[key])
      {
        if (asOf != timedValue.AsOf) continue;
        timedValue.Value = value;
        return;
      }

      _base[key].Add(new TimedValue<TValue>(value, asOf));
    }

    /// <summary>
    ///   Gets the most current value of the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>TValue.</returns>
    /// <exception cref="System.ArgumentNullException">key</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">key</exception>
    public TValue Get(TKey key)
    {
      if (key == null) throw new ArgumentNullException(nameof(key));
      if (!ContainsKey(key)) throw new ArgumentOutOfRangeException(nameof(key));
      return _base[key].OrderByDescending(t => t.AsOf).First().Value;
    }

    /// <summary>
    ///   Gets the value of the specified key on or after the requested time.
    ///   (i.e. If there are values for 7:00am and 8:00am and the requested asOf value is 7:30,  the 7:00 value will be
    ///   returned)
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="asOf">As of a specific date/time.</param>
    /// <returns>TValue.</returns>
    /// <exception cref="System.ArgumentNullException">key</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">key</exception>
    public TValue Get(TKey key, DateTime asOf)
    {
      if (key == null) throw new ArgumentNullException(nameof(key));
      if (!ContainsKey(key)) throw new ArgumentOutOfRangeException(nameof(key));

      var baseValues = _base[key].OrderByDescending(t => t.AsOf).ToArray();
      foreach (var tv in baseValues)
      {
        if (asOf >= tv.AsOf)
          return tv.Value;
      }

      return baseValues.First().Value;
    }

    /// <summary>
    ///   Deletes all values having the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <exception cref="System.ArgumentNullException">key</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">key</exception>
    public void Delete(TKey key)
    {
      if (key == null) throw new ArgumentNullException(nameof(key));
      if (!ContainsKey(key)) throw new ArgumentOutOfRangeException(nameof(key));
      _base.Remove(key);
    }

    /// <summary>
    ///   Empties all values from this dictionary.
    /// </summary>
    public void Clear() => _base.Clear();

    /// <summary>
    ///   Determines whether the specified key exists within the store
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
    public bool ContainsKey(TKey key) => _base.ContainsKey(key);

    /// <summary>
    ///   Counts the total number of keys available in this instance.
    /// </summary>
    /// <returns>System.Int32.</returns>
    public int Count() => _base.Keys.Count;

    /// <summary>
    ///   Counts total number of values available for the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>System.Int32.</returns>
    /// <exception cref="System.ArgumentNullException">key</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">key</exception>
    public int Count(TKey key)
    {
      if (key == null) throw new ArgumentNullException(nameof(key));
      if (!ContainsKey(key)) throw new ArgumentOutOfRangeException(nameof(key));
      return _base[key].Count;
    }
  }
}
