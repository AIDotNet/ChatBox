using System.Linq;
using System.Runtime.CompilerServices;
using SharpToken;

namespace ChatBox.Internal;

public class TokenHelper
{
    private static GptEncoding? GptEncoding { get; set; }

    /// <summary>
    /// Initializes the token encoding asynchronously. 
    /// This method retrieves the encoding for "cl100k_base" if it hasn't been initialized yet.
    /// </summary>
    public static async Task InitToken()
    {
        await Task.Run(async () =>
        {
            GptEncoding ??= GptEncoding.GetEncoding("cl100k_base");
        });
    }

    /// <summary>
    /// Calculates the total number of tokens for the provided content strings.
    /// </summary>
    /// <param name="content">An array of strings for which the total token count will be calculated.</param>
    /// <returns>The total number of tokens across all input strings.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTotalTokens(params string?[] content)
    {
        return content.Sum((s => RefGetTokens(ref s)));
    }

    /// <summary>
    /// Counts the number of tokens for the specified string using a reference parameter.
    /// </summary>
    /// <param name="content">The string for which the token count is requested, passed by reference.</param>
    /// <returns>The number of tokens in the specified string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RefGetTokens(ref string content)
    {
        return GptEncoding?.CountTokens(content) ?? 0;
    }

    /// <summary>
    /// Counts the number of tokens in the specified string.
    /// </summary>
    /// <param name="content">The string for which the token count is requested.</param>
    /// <returns>The number of tokens in the specified string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTokens(string content)
    {
        return GptEncoding?.CountTokens(content) ?? 0;
    }
}