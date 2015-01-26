using Machine.Specifications;

namespace Urlicious.Specifications
{
    [Subject(typeof(Url))]
    public class ParseSpecifications
    {
        private static Url _url;

        Establish context = () =>
        {
            _url =
                Url.Parse(
                    "https://msdn.microsoft.com/en-us/library/system.uri.getcomponents(v=vs.110).aspx?hl=en&b=c&foo=bar");
        };

        It root_should_be_msdn_dot_microsoft_dot_com = () =>
            _url.RootUrl.ShouldEqual("https://msdn.microsoft.com");

        It absolute_path_should_no_longer_contain_queries =
            () => _url.AbsolutePath.Contains("?").ShouldBeFalse();

        It argument_count_should_be_three = 
            () => _url.Queries.Count.ShouldEqual(3);
    }
}
