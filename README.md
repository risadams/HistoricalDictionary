# HistoricalDictionary

## Introduction

This is a lightweight library to add a history state to a dictionary.

## Installation

via nuget.org: <https://www.nuget.org/packages/HistoricalDictionary/1.0.0>

## Example

```csharp
var history = new HistoricalDictionary<string, string>();
history.Add("key1", "value1");
history.Add("key1", "value2", DateTime.Now);

var value = history.Get("key1");
var value = history.Get("key1", DateTime.Now);
```

## Contribute

If you think this could be better, please [open an issue](https://github.com/risadams/HistoricalDictionary/issues/new)!

Please note that all interactions in this organization fall under our [Code of Conduct](CODE_OF_CONDUCT.md).

## License

[MIT](LICENSE) © 1996+ Ris Adams
