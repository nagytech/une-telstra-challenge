# UNE Telstra Comp - 2016: Design Document

REV 0.1 - Initial Commit, 05-Apr-16, Jonathan Nagy

## 1.0    INTRODUCTION

### 1.1   Purpose

The team has accepted the Telstra challenge put forth by the Cerebral Palsy
Alliance of Australia.  The challenge posed is to use technology in a way that
captures real time data about accessible locations for those in a wheelchair.
The primary focus is locations within a specific CBD with an outlook of
expanding to other areas as well as other types of user.

The purpose of this document is to illustrate the major components of the
application as they are envisioned by the team.  The major components will
highlight milestones that are required in order to produce a working 'proof of
concept'.

### 1.2   Scope

The scope of the document is limited to high level functions with respect of
data flow, system architecture and use cases.  Trivial details may be overlooked
or ignored where they have been intuitively evaluated to be, in fact, trivial.

It should also be noted that, due to time constraints, this design document
does not represent any best practices that would otherwise lead to a more
robust, stable, or efficient system.  My opinion is that there will be time
to streamline processes and improve efficiency once the product is 'out there'.

## 2.0    OVERVIEW

### 2.1   System Components

- Operating system: Unix based,
- Additional components: LAPP stack, PostGIS, OSGEO, QGIS, PGRouting
- Minimum requirements: TBD, based on performance

### 2.2   Constraints

TBD

## 3.0    DESIGN

### 3.1   Architecture

Unless we are willing to manage the metadata and transactional data from the
back end, it is safe to say that we require multiple user interfaces.  It will
be most efficient, therefore, to utilize a n-tier approach.  As such, the key
components to be discussed can classified as follows:

*Presentation*
- Administration Portal
- User Portal
- Mobile Client

*Database*
- Data Access Layer

*Business Logic*
- OAuth Server (or session state server TBD)
- API
- Batch Loader
- Actors

### 3.2   Presentation Layer

The Admin Portal and User Portal should be written in either MVC or some form
of JavaScript app framework like Angular or React.

JN: Again, depends on who is doing it - best foot forward..

The Mobile Client may be best done in Xamarin (or similar) in order to
facilitate a cross-platform development environment.  Although the CPA has a
strong preference for iOS, the app would be better used across multiple user
bases.  Having a single code base for the client app would be a significant
saving in development.

#### 3.2.1  Administration Portal

The administration portal will allow admin users and power users to configure,
update, and manage the higher level functions of the application.  It is
envisioned that these higher level functions are comprised of:

- Add, remove, delete users
- Review system logs, event logs
- Initialize batch loader processes
- Configure routing metadata
- Load, or configure interactions with third-party data sources

#### 3.2.2  User Portal

_JN: The user portal may not be completely necessary at this stage.  I cannot
think of any valid reasons to have one other than for a user to create an
account outside of the mobile device. Perhaps it may be useful for carers,
family members, friends to help coordinate events and provide feedback on
navigation issues encountered._

#### 3.2.3   Mobile Client

The primary component of the application that allows users to perform the
following functions:

- Sign up
- Log in
- Create events / waypoints for navigation
- Search for and join friends who also have the app
- Navigate through any area that has an established network of footpaths.

### 3.3   Data Access Layer

For the routing portion of the application, the pgrouting functions are
provided through direct raw SQL queries and stored procedures.  The data access
layer, therefore, needs to support raw SQL.  ORM may be of some use, but it is
arguably easier to map directly.

For the administrative, and 'social' aspect of the application, the data access
layer may be best represented through some type of ORM.  However, some ORMs are
very touchy and difficult to set up.

JN: For discussion?  Depends on if, and who is doing the Admin / Social aspect
of the App

#### 3.3.1  Database Design and Modelling

The database is separated into the following components:

- Core routable data
- Congestion model
- TODO: Users, OAuth, Third-Party integration

##### 3.3.1.1   Core Routable Data

The core routable data consists of spatial information about traversable network
infrastructure within a designated urban area.  Each network component is broken
down into single segments and represented in the following format:

    create table network
    (
        gid serial primary key not null,
        entitytype integer not null,
        entityid integer not null,
        name text,
        geom geometry not null,
        length double precision,
        source int,
        target int
    );

    create table networktype
    (
      gid serial primary key not null,
      entitytypeid int not null,
      name text not null,
      cost double precision not null
    );

Navigating the network, without consideration of congestion is a function of
network.length * networktype.cost where cost is a representation of how
difficult the network entity is to traverse.  A larger cost value indicates a
more difficult network segment.

##### 3.3.1.2   Congestion Model

The congestion model is a series of relationships which represent network
entities that are under some level of 'temporary stress'.  These stressors
make the traversal more difficult, and thus add to the overall cost.  The
congestion model is currently represented as follows:

    create unique index congestiontype_congestiontypeid_uindex ON public.congestiontype (congestiontypeid);
    create unique index congestiontype_name_uindex ON public.congestiontype (name);

    create table congestion
    (
      gid serial primary key not null,
      congestiontypeid integer not null,
      name text not null,
      startOn timestamp not null,
      endOn timestamp,
      address text null,
      geometry geometry not null
    )
    create table congestiontype
    (
      gid serial primary key not null,
      congestiontypeid integer not null,
      name text not null,
      cost double precision not null
    )

The relationship between network and congestion is represented then as follows:

    create table networkcongestion
    (
      gid serial primary key not null,
      congestionid int not null,
      networkid int not null
    );

Note: The database will establish and maintain the linkage between network and
congestion through the create_congestion function.

### 3.4   Business Logic

#### 3.4.1  OAuth Server

Token based authentication would be a great selling point for 'interoperability'
with other third-party apps that would like to use our API.  Not much effort to
set up, but would need HTTPS in the foreseeable future.

#### 3.4.2  API

The API should provide the following functions:

    /**
     Client:
     Used by the mobile client for the purposes of routing or map display
     **/
    GET     /route          req:{from:{x,y},to:{x,y}}, res:{route[]}

    GET     /lines          req:{x1,y1,x2,y2}, res:{network[]}


    /**
      Congestion:
      Usable by third-party, or actor based batch loader.
      **/
    GET     /congest/new    req:{type,start,end,wkt},res:{}

    GET     /congest/edit   req:{id, congestion:{type,start,end,wkt}}, res:{}  

    GET     /congest/list   req:{x1,y1,x2,y2}, res:{congestion[]}


    // TODO: other stuff

    POST    /login          req:{user, password, deviceid}, res:{token,profile}

    GET     /places         req:{filter:{type,x,y,radius}}, res:{place[]}

    POST    /group/new      req:{name,config}, res:{groupid}

    GET     /group/:id      req:{}, res:{user[], location[], waypoint[]}

    POST    /group/:id/join req:{}, res:{}

    WS      /group/:id/msg  req:{message}, res:{message}

Note: not all APIs are relevant for the mobile client, some are for third-party
integration or administration.

#### 3.4.3  Batch Loader

Integration with third-party data that is static, or valid for long periods of
time.

JN: Probably something we can avoid building, but it would be good to at least
have a solid plan on how to manage the data.  Perhaps this is one of the areas
where we need to solve the problem via non-technical means.  Sure, Melbourne is
good for data - but what about other cities?  What do we do if no one has
sidewalk data?  OSM will work to an extent, but other than that - what is our
answer?

#### 3.4.4  Actors

Integration with third-party data that is live (ES/API) based, or refreshed ~1-2
times a day.

JN: Probably something to be said here about how integration with third party
data sources (API) can be achieved.
