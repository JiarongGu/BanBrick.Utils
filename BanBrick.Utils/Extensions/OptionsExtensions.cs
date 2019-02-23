using Microsoft.Extensions.Options;

namespace BanBrick.Utils.Extensions
{
    public static class OptionsExtensions
    {
        /// <summary>
        /// Parse the IOptions to typed value
        /// </summary>
        public static TOptions Parse<TOptions>(this IOptions<object> options) where TOptions : class, new()
        {
            if (options.Value.GetType() == typeof(TOptions))
                return (TOptions)options.Value;
            return options.Value.Parse<TOptions>();
        }
    }
}
