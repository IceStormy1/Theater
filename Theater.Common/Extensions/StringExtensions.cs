namespace Theater.Common.Extensions
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string input)
        {
            if(string.IsNullOrWhiteSpace(input))
                return input;

            return input.Length > 1 
                ? char.ToLowerInvariant(input[0]) + input[1..] 
                : input.ToLowerInvariant();
        }
    }
}
