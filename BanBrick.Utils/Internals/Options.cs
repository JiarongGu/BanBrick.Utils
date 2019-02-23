using Microsoft.Extensions.Options;

namespace BanBrick.Utils.Internals
{
    internal class Options<TOptions> : IOptions<TOptions> where TOptions : class, new()
    {
        public Options(TOptions value) {
            Value = value;
        }

        public TOptions Value { get; }
    }
}
