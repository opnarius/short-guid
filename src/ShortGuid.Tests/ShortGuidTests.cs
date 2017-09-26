using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ShortGuid.Tests
{
    [TestFixture]
    public class ShortGuidTests
    {
        [Test]
        public void Should_Parse_WithPaddingIntact()
        {
            var guid = Guid.NewGuid();
            var parsedShortGuid = ShortGuid.Parse(Convert.ToBase64String(guid.ToByteArray()));

            guid.Should().Be(parsedShortGuid.ToGuid());
        }

        [Test]
        public void Should_Parse_WithPaddingRemoved()
        {
            var shortGuid = ShortGuid.NewGuid();
            var parsedShortGuid = ShortGuid.Parse(shortGuid.ToString());

            shortGuid.Should().Be(parsedShortGuid);
        }

        [Test]
        public void Parse_ShouldThrow_UsingBadInput()
        {
            var input = "obviously not a guid";

            Action parseAction = () => ShortGuid.Parse(input);
            parseAction.ShouldThrow<Exception>();
        }

        [Test]
        public void Parse_ShouldThrow_UsingEmptyInput()
        {
            var input = "";

            Action parseAction = () => ShortGuid.Parse(input);
            parseAction.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Should_TryParse_WithProperInput()
        {
            var shortGuid = ShortGuid.NewGuid();

            ShortGuid parsedShortGuid;
            var parseResult = ShortGuid.TryParse(shortGuid.ToString(), out parsedShortGuid);

            parseResult.Should().BeTrue();
            parsedShortGuid.Should().Be(shortGuid);
        }

        [Test]
        public void TryParse_ShouldBeEmpty_WithBadInput()
        {
            var input = "some value, not a guid";

            ShortGuid parsedShortGuid;
            var parseResult = ShortGuid.TryParse(input, out parsedShortGuid);

            parseResult.Should().BeFalse();
            parsedShortGuid.Should().Be(ShortGuid.Empty);
        }

        [Test]
        public void Should_Cast_ToGuid()
        {
            var guid = Guid.NewGuid();
            var shortGuid = new ShortGuid(guid);

            ((Guid)shortGuid).Should().Be(guid);
        }

        [Test]
        public void Guid_Should_Cast_To_ShortGuid()
        {
            var guid = Guid.NewGuid();
            var shortGuid = new ShortGuid(guid);

            ((ShortGuid)guid).Should().Be(shortGuid);
        }

        [Test]
        public void NullableShort_Should_Cast_To_NullableGuid()
        {
            var guid = (Guid?)null;
            var shortGuid = (ShortGuid?)null;

            ((ShortGuid?)guid).Should().Be(shortGuid);
        }

        [Test]
        public void NullableGuid_Should_Cast_To_NullableShortGuid()
        {
            var guid = (Guid?)null;
            var shortGuid = (ShortGuid?)null;

            ((Guid?)shortGuid).Should().Be(guid);
        }

        [Test]
        public void Should_Serialize_Single()
        {
            var shortGuid = ShortGuid.NewGuid();

            var jsonString = JsonConvert.SerializeObject(shortGuid);
            ShortGuid shortGuidFromJson = JsonConvert.DeserializeObject<ShortGuid>(jsonString);

            jsonString.Should().NotBeEmpty();
            shortGuidFromJson.Should().NotBeNull();
            shortGuid.Should().Be(shortGuidFromJson);
        }

        [Test]
        public void Should_Serialize_Array()
        {
            var shortGuidArray = new[]
            {

                ShortGuid.NewGuid(),
                ShortGuid.NewGuid()
            };

            var jsonString = JsonConvert.SerializeObject(shortGuidArray);
            ShortGuid[] shortGuidFromJson = JsonConvert.DeserializeObject<ShortGuid[]>(jsonString);

            jsonString.Should().NotBeEmpty();
            shortGuidFromJson.Should().NotBeEmpty();
            shortGuidFromJson.Length.Should().Be(2);

            for (int i = 0; i < shortGuidArray.Length; i++)
            {
                shortGuidArray[i].Should().Be(shortGuidFromJson[i]);
            }
        }
    }
}
