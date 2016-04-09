create table networktype
(
  gid serial primary key not null,
  entitytypeid int not null,
  name text not null,
  cost double precision not null
);
CREATE UNIQUE INDEX networktype_entitytypeid_uindex ON public.networktype (entitytypeid);

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
alter table public.network
add constraint network_networktype_entitytypeid_fk
foreign key (entitytype) references networktype (entitytypeid);

create table congestiontype
(
  gid serial primary key not null,
  congestiontypeid integer not null,
  name text not null,
  cost double precision not null
)
create unique index congestiontype_congestiontypeid_uindex ON public.congestiontype (congestiontypeid);
  create unique index congestiontype_name_uindex ON public.congestiontype (name);

create table congestionsource
(
    gid serial primary key not null,
    name text not null
    -- TODO: OAuth tokens etc...
)
create unique index congestionsource_name_uindex ON public.congestionsource (name);

create table congestion
(
  gid serial primary key not null,
  congestiontypeid integer not null,
  name text not null,
  startOn timestamp not null,
  endOn timestamp,
  address text null,
  geometry geometry not null
);
alter table public.congestion
add constraint congestion_congestiontype_congestiontypeid_fk
foreign key (congestiontypeid) references congestiontype (congestiontypeid);

create table networkcongestion
(
  gid serial primary key not null,
  congestionid int not null,
  networkid int not null
);
alter table public.networkcongestion
add constraint networkcongestion_congestion_congestionid
foreign key (congestionid) references congestion (gid);
alter table public.networkcongestion
add constraint networkcongestion_network_networkid
foreign key (networkid) references network (gid);
