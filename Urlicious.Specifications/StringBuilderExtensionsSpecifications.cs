using System.Text;
using Machine.Specifications;

namespace Urlicious.Specifications
{
    [Subject(typeof(StringBuilder))]
    public class StringBuilderExtensionsSpecifications
    {
        private static StringBuilder _sb;

        Establish context = () =>
        {
            _sb = new StringBuilder();
        };

        Because of = () =>
        {
            _sb.Append("/foo/");
        };

        It stringBuilder_should_start_with_slash = () => _sb.StartsWith("/").ShouldBeTrue();
        It stringBuilder_should_end_with_slash = () => _sb.EndsWith("/").ShouldBeTrue();
    }

    public class StringBuilderTrimSpecifications
    {
        private static StringBuilder _sb;

        Establish context = () => _sb = new StringBuilder();

        Because of = () =>
        {
            _sb.Append(";;;;SomeText456/////");
            _sb.TrimStart(';');
            _sb.TrimEnd('/');
        };

        It resulting_string_should_not_contain_semicolon_or_slashes = () => _sb.ToString().ShouldEqual("SomeText456");
    }
}
