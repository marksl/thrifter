using System.Collections.Generic;

namespace HttpTest
{
    public class TwitterImplementation : Twitter.Iface
    {
        public void ping()
        {
        }

        public bool postTweet(Tweet tweet)
        {
            if (tweet != null && tweet.Text == "foo")
                throw new TwitterUnavailable();

            return true;
        }

        public TweetSearchResult searchTweets(string query)
        {
            

            return new TweetSearchResult
                       {
                           Tweets = new List<Tweet>
                                        {
                                            new Tweet
                                                {
                                                    Language = "foo",
                                                    Loc = new Location
                                                              {
                                                                  Latitude = 54.0,
                                                                  Longitude = 55.0
                                                              },
                                                    Text = "foo",
                                                    TweetType = TweetType.TWEET,
                                                    UserId = 3,
                                                    UserName = "fooname"
                                                },
                                                new Tweet
                                                {
                                                    Language = "foo2",
                                                    Loc = new Location
                                                              {
                                                                  Latitude = 1.0,
                                                                  Longitude = 2.0
                                                              },
                                                    Text = "foo2",
                                                    TweetType = TweetType.DM,
                                                    UserId = 4,
                                                    UserName = "fooname2"
                                                }
                                        }
                       };
        }

        public void zip()
        {
        }
    }
}