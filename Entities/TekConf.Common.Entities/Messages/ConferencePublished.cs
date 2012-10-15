using TinyMessenger;

namespace TekConf.UI.Api
{
    public class ConferencePublished : ITinyMessage
    {
        /// <summary>
        /// The sender of the message, or null if not supported by the message  implementation.
        /// </summary>
        public object Sender { get; private set; }
    }
}