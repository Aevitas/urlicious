using System.Net;
using Machine.Specifications;

namespace Urlicious.Specifications
{
    [Subject(typeof(Url))]
    public class AppendPathsSpecifications
    {
        private static Url _url;

        Establish context = () =>
        {
            _url = new Url(Constants.BaseUrl);
        };

        Because of = () =>
        {
            _url.AppendPaths("a", "b", "c");
        };

        It url_should_contain_paths = () =>
        {
            _url.ToString().ShouldEqual(string.Format("{0}{1}/{2}/{3}", Constants.BaseUrl, "a", "b", "c"));
        };
    }
}
