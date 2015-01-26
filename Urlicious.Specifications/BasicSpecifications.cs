using Machine.Specifications;

namespace Urlicious.Specifications
{
    [Subject(typeof(Url))]
    public class BasicSpecifications
    {
        private static Url _url;

        Establish context = () =>
        {
            _url = new Url(Constants.BaseUrl);
        };

        Because of = () =>
        {
            _url.AppendPath("maps");
            _url.AddQuery("hl", "en");
        };

        It url_should_point_to_maps = () => _url.ToString().ShouldEqual("http://google.com/maps?hl=en");
    }
}
