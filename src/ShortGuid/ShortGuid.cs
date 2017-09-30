using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShortGuid
{
    /// <inheritdoc />
    /// <summary>
    /// Wrapper that outputs shortened version of a Guid.
    /// Converts to and from underlying Guid
    /// </summary>
    [JsonConverter(typeof(ShortGuidJsonConverter))]
    public struct ShortGuid : IEquatable<ShortGuid>
    {
        private readonly Guid _backingGuid;

        public ShortGuid(Guid guid)
        {
            _backingGuid = guid;
        }

        public static ShortGuid Empty = new ShortGuid(Guid.Empty);

        public static ShortGuid NewGuid()
        {
            return new ShortGuid(Guid.NewGuid());
        }

        public override string ToString()
        {
            return Convert.ToBase64String(_backingGuid.ToByteArray()).Replace("=", "").Replace("/", "_");
        }

        public Guid ToGuid()
        {
            return _backingGuid;
        }

        public static ShortGuid Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            var t = new StringBuilder(text);

            // Put back the base64 padding we removed in ToString
            if (text.Length % 3 != 0)
                t.Append("==");

            var guidBytes = Convert.FromBase64String(t.ToString().Replace("_", "/"));

            var guid = new Guid(guidBytes);

            return new ShortGuid(guid);
        }

        public static bool TryParse(string text, out ShortGuid result)
        {
            try
            {
                result = Parse(text);
                return true;
            }
            catch
            {
                result = Empty;
                return false;
            }
        }

        public bool Equals(ShortGuid other)
        {
            return _backingGuid.Equals(other._backingGuid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ShortGuid && Equals((ShortGuid)obj);
        }

        public override int GetHashCode()
        {
            return _backingGuid.GetHashCode();
        }

        public static implicit operator Guid(ShortGuid shortGuid) => shortGuid._backingGuid;

        public static implicit operator ShortGuid(Guid guid) => new ShortGuid(guid);

    }
}
