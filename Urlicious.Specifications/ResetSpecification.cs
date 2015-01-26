using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace Urlicious.Specifications
{
    [Subject(typeof(Url))]
    public class ResetSpecification
    {
        private static Url _url;

        Establish context = () =>
        {
            _url = new Url(Constants.BaseUrl);
        };

        Because of = () =>
        {
            _url.AddQuery("hl", "en");
            _url.AddQuery("a", false);

            _url.Reset(true);
        };

        It url_should_be_default = () => _url.ToString().ShouldEqual(Constants.BaseUrl);
    }
}
