import twitter4j._

import scala.collection.JavaConversions._

object Main extends App {

  override def main(args: Array[String]) {

    // TODO: Look for ways to filter people without followers, or who don't follow
    //        ie. SPAM.

    val stream = new TwitterStreamFactory(Util.config).getInstance
    stream.addListener(Util.simpleStatusListener)
    // TODO: Configure central location that we want to listen for.
    val melbcbd = Array(Array(144.9476,-37.8217),Array(144.9761,-37.8065))
    stream.filter(new FilterQuery().locations(melbcbd))

    Thread.sleep(10000) // Just waiting.. nothing important.

    // Altenate query method.  Useful for catching up on streams, or looking retrospectively
    // for period of time analysis.
    //var q = new Query().geoCode(new GeoLocation(-37.812080, 144.965202), 1, "km")
    //var tweets = t.search(q).getTweets
    
  }

  object Util {
    val config = new twitter4j.conf.ConfigurationBuilder()
    .setUseSSL(true)
  // Set tokens here, or set in properties file.
  //.setOAuthConsumerKey("...")
  //.setOAuthConsumerSecret("...")
  //.setOAuthAccessToken("...")
  //.setOAuthAccessTokenSecret("...")
    .build

    def simpleStatusListener = new StatusListener() {
      def onStatus(status: Status) {  
        println(status.getGeoLocation)
        println(status.getCreatedAt)
        println(status.getText)
      }
      def onDeletionNotice(statusDeletionNotice: StatusDeletionNotice) {}
      def onTrackLimitationNotice(numberOfLimitedStatuses: Int) {}
      def onException(ex: Exception) { ex.printStackTrace }
      def onScrubGeo(arg0: Long, arg1: Long) {}
      def onStallWarning(warning: StallWarning) {}
    }

  }

}
