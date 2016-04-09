insert into networktype
(entitytypeid, name, cost)
    VALUES
      (1, 'road corridor', 1),
      (2, 'sidewalk', 0.9)

insert into congestiontype
(congestiontypeid, name, cost)
    VALUES
      (1, 'construction', 1000),
      (2, 'emergency', 50),
      (3, 'major-event', 10)


/* POST IMPORT

insert into network(entityid, entitytype, name, geom, length)
select gid, 1, address, geom, length from roads;

insert into network(entityid, entitytype, name, geom, length)
select gid, 2, address, geom, s.distance from sides s;

select pgr_createtopology('network', 0.00001, 'geom', 'gid')

-- not sure why, some entries are null - perhaps botched geoprocessing
update network as n
  set name = s.address
from sides s
  where n.name is null and n.entityid = s.gid and s.address is not null and n.entitytype = 2

*/
