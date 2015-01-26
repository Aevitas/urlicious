using Machine.Specifications;

namespace Urlicious.Specifications
{
    [Subject(typeof(Url))]
    public class OperatorSpecifications
    {
        private static Url _url;

        Establish context = () =>
        {
            // Construct a Url instance by implicitly converting from string.
            _url = Constants.BaseUrl;
        };

        It url_should_be_well_formed = () =>
        {
            _url.IsWellFormed().ShouldBeTrue();
        };
    }
}
