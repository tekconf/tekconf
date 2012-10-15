using TinyMessenger;

namespace TekConf.UI.Api
{
    public class SessionAdded : ITinyMessage
    {
        public object Sender { get; private set; }
    }
}