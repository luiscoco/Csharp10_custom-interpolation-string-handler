using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

var numbers = Enumerable.Range(0, 10).ToArray();
Numbers.Write($"Call {numbers:(###) ###-####} For A Good Time!");
Numbers.Write('x', $"Call {numbers:(xxx) xxx-xxxx} For A Good Time!");
Numbers.Range(numbers, $"Call ({0..3}) {3..6}-{6..9} For A Good Time!");

public static class Numbers
{
    public static void Write(PlaceholderInterpolatedStringHandler builder)
    {
        Write('#', builder);
    }

    public static void Write(char placeholder,
        [InterpolatedStringHandlerArgument("placeholder")] PlaceholderInterpolatedStringHandler builder
    )
    {
        Console.WriteLine(builder.GetFormattedText());
    }

    public static void Range<T>(
        T[] args,
        [InterpolatedStringHandlerArgument("args")]
        RangeInterpolatedStringHandler<T> handler)
    {
        Console.WriteLine(handler.ToString());
    }
}


/// <summary>
/// Credit to Khalid @buhakmeh
/// </summary>
[InterpolatedStringHandler]
public readonly struct PlaceholderInterpolatedStringHandler
{
    private char Placeholder { get; }
    private StringBuilder Builder { get; }

    public PlaceholderInterpolatedStringHandler(int literalLength, int formattedCount, char placeholder = '#')
        => (Placeholder, Builder) = (placeholder, new StringBuilder());

    public void AppendLiteral(string s) => Builder.Append(s);
    internal string GetFormattedText() => Builder.ToString();

    public void AppendFormatted(IEnumerable t, string format)
    {
        var enumerator = t.GetEnumerator();
        foreach (var c in format)
        {
            if (c == Placeholder && enumerator.MoveNext())
                Builder.Append(enumerator.Current);
            else
                Builder.Append(c);
        }
    }
}

/// <summary>
/// Credit to Kristian Hellang (@khellang)
/// </summary>
/// <typeparam name="T"></typeparam>
[InterpolatedStringHandler]
public readonly struct RangeInterpolatedStringHandler<T>
{
    public RangeInterpolatedStringHandler(int literalLength, int formattedCount, T[] args)
        => (Args, Builder) = (args, new StringBuilder());

    private StringBuilder Builder { get; }
    private T[] Args { get; }

    public void AppendFormatted(Range range)
        => Builder.Append(string.Concat(Args[range]));

    public void AppendLiteral(string value)
        => Builder.Append(value);

    public string ToString() =>
        Builder.ToString();
}