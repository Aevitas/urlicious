using System.Collections.Generic;
using System.Linq;
using System.Net;
using Machine.Specifications;

namespace Urlicious.Specifications
{
    [Subject(typeof(Url))]
    public class PathSpecifications
    {
        private static Url _url;
        private static List<string> _obtainedPaths;

        private static string[] _initialPaths = { "a", "b", "c" };
            
        Establish context = () =>
        {
            _url = new Url(Constants.BaseUrl);
        };

        Because of = () =>
        {
            _url.AppendPaths("a", "b", "c");
            _obtainedPaths = _url.GetPaths().ToList();
        };

        It url_should_contain_paths = () =>
        {
            _url.ToString().ShouldEqual(string.Format("{0}/{1}/{2}/{3}", Constants.BaseUrl, "a", "b", "c"));
        };

        It url_should_contain_three_paths = () => _obtainedPaths.Count.ShouldEqual(3);

        It url_paths_should_equal_initial_paths = () =>
        {
            for (int i = 0; i < _obtainedPaths.Count; i++)
            {
                _obtainedPaths[i].ShouldEqual(_initialPaths[i]);
            }
        };
    }

    [Subject(typeof(Url))]
    public class ErroreousPathSpecifications
    {
        private static Url _url;

        Establish context = () =>
        {
            _url = new Url(Constants.BaseUrl);
        };

        Because we_try_to_mess_stuff_up = () =>
        {
            _url.AppendPath("/////////////");
            _url.AppendPath("//d////");
            _url.AppendPath("/e$//");
        };

        It should_still_produce_valid_output = () =>
        {
            _url.ToString().ShouldEqual(string.Format("{0}/{1}/{2}", Constants.BaseUrl, "d", "e$"));
        };
    }
}
