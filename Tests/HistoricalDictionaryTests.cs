using System;
using HistoricalDictionary;
using Xunit;

namespace Tests
{
  public class HistoricalDictionaryTests
  {
    [Fact]
    public void CanCreateDict()
    {
      var d = new HistoricalDictionary<string, int>();
      Assert.NotNull(d);
    }

    [Fact]
    public void CanSupportMultipleKeys()
    {
      var d = new HistoricalDictionary<string, int>();
      Assert.NotNull(d);

      var baseTime = new DateTime(2022, 03, 17);

      d.Add("a", 1, baseTime);
      d.Add("a", 2, baseTime.AddDays(1));
      d.Add("a", 3, baseTime.AddDays(2));
      d.Add("b", 2, baseTime);

      Assert.Equal(1, d.Get("a", baseTime));
      Assert.Equal(1, d.Get("a", baseTime.AddHours(12)));
      Assert.Equal(2, d.Get("a", baseTime.AddHours(24)));
      Assert.Equal(3, d.Get("a", baseTime.AddHours(48)));
      Assert.Equal(3, d.Get("a"));

      Assert.Equal(2, d.Count());
      Assert.Equal(3, d.Count("a"));
    }

    [Fact]
    public void CanSupportDeletion()
    {
      var d = new HistoricalDictionary<string, int>();
      Assert.NotNull(d);

      d.Add("a", 1);
      d.Add("a", 2);
      d.Add("a", 3);
      d.Add("b", 2);

      d.Delete("a");

      Assert.Equal(1, d.Count());
      Assert.Throws<ArgumentOutOfRangeException>(() =>
      {
        d.Count("a");
      });

      d.Clear();

      Assert.Throws<ArgumentNullException>(() =>
      {
        d.Delete(null);
      });

      Assert.Equal(0, d.Count());
    }

    [Fact]
    public void CanSupportContainsKey()
    {
      var d = new HistoricalDictionary<string, int>();
      d.Add("a",1);

      Assert.True(d.ContainsKey("a"));
      Assert.False(d.ContainsKey("b"));
    }

    [Fact]
    public void SameTimePeriod_GetsReplaced()
    {
      var d = new HistoricalDictionary<string, int>();
      Assert.NotNull(d);

      var baseTime = new DateTime(2022, 03, 17);

      d.Add("a", 1, baseTime);
      d.Add("a", 2, baseTime.AddDays(1));
      d.Add("a", 3, baseTime.AddDays(1));
      d.Add("b", 2, baseTime);

      Assert.Equal(1, d.Get("a", baseTime));
      Assert.Equal(1, d.Get("a", baseTime.AddHours(12)));
      Assert.Equal(3, d.Get("a", baseTime.AddHours(24)));
      Assert.Equal(3, d.Get("a", baseTime.AddHours(48)));
      Assert.Equal(3, d.Get("a"));

      Assert.Equal(2, d.Count());
      Assert.Equal(2, d.Count("a"));
    }
  }
}
