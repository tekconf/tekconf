using TinyMessenger;

namespace TekConf.UI.Api
{
    public class HubSubscriptions
    {
        private readonly ITinyMessengerHub _hub;

        public HubSubscriptions(ITinyMessengerHub hub)
        {
            _hub = hub;
            Subscribe();
        }

        private void Subscribe()
        {
            SubscribeToConferencePublished();
            SubscribeToSessionAdded();
        }

        private void SubscribeToSessionAdded()
        {
            _hub.Subscribe<SessionAdded>((m) =>
                                             {

                                             });
        }

        private void SubscribeToConferencePublished()
        {
            _hub.Subscribe<ConferencePublished>((m) =>
                                    {
                                        // Add to community megaphone
                                    });

            _hub.Subscribe<ConferencePublished>((m) =>
                                    {
                                        // Add to rss feed
                                    });

            _hub.Subscribe<ConferencePublished>((m) =>
                                    {
                                        // Tweet
                                    });
        }
    }
}