using System.Runtime.Remoting.Messaging;
using Machine.Specifications;

namespace Urlicious.Specifications
{
    [Subject(typeof (Url))]
    public class QuerySpecifications
    {
        private static Url _url;

        Establish context = () =>
        {
            _url = new Url(Constants.BaseUrl);
        };

        Because of = () =>
        {
            _url.AddQuery("foo", true);
        };

        It query_value_should_be_true = () =>
        {
            _url.GetQueryValue<bool>("foo").ShouldBeTrue();
        };

        It query_with_retriever_should_be_true = () =>
        {
            _url.GetQueryValue("foo", o => (bool) o).ShouldBeTrue();
        };
    }
}
