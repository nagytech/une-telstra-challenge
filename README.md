# une-telstra-challenge

Project code for the UNE Telstra Challenge for 2016.  For this project we chose the Cerebral Palsy Alliance to support. Our application consisted of a PHP/PostGIS webservice which provided network routing for accessible routes in the Sydney / Melbourne CBD.



[![Build Status](http://jenkins.nagytech.com/buildStatus/icon?job=Telstra University Challenge - UNE)](http://jenkins.nagytech.com/job/Telstra%20University%20Challenge%20-%20UNE/)

##Telstra University Challenge 2016
###Cerebral Palsy Alliance

Below is the transcript of the project challenge:

(transcript)

E: Hi, I'm Emily Dash

J: and I'm Jordan Nguyen from the Cerebral Palsy Alliance.

E: Imagine having to always having to go the long way around when you're out with friends in the city.  That's life for people like me who use wheelchairs because there's no realtime information on the fastest most accessible route.

E: Like anyone, I want to go out and have fun in the city.

J: So our challenge to you is, how might you use technology to capture data about wheelchair-accessible routes around footpaths and public spaces in the CBD and then feed that back through to other users through their smartphones.

E: We want the most tallented developers to tackle our challenge, so please, jump on board.

## File Management

    ├── build.sh                             Build file used in deployment
    ├── data    
    │   └── melb.zip                         Melbourne street corridor and pathway data (processed)
    │   └── sydney.osm.bz2                   OpenStreetMap Data
    ├── db                                   DB Functions
    ├── doc                                  Documentation
    │   └── ...
    ├── install                              Installation procedures for new system (Ubuntu 14)
    ├── README.md
    └── src
        ├── mobile-client                    Client app for iOS/Android*
        ├── twitter-agent                    Agent for monitoring Twitter feeds
        ├── web-client                       Admin Web UI
        └── web-server                       Server API

## Notes

- All design is simply work in progress and up for discussion, at this point, just trying to get a demo out there.
- CORS is enabled for * for the purposes of debugging and developing (ot at least it was at some point...).  Eventually, we should turn that off.


* Requires Xamarin Studio, free trial for students: https://www.dreamspark.com/Product/Product.aspx?productid=100
