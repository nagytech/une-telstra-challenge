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

update network as n
  set name = n2.name
from network n2
where n.name is null and n2.name is not null and n.entitytype = 2 and n2.entitytype = 2 and n2.entityid = n.entityid

insert into network(entityid, entitytype, name, geom, length)
select gid, 1, address, geom, length from roads;

insert into network(entityid, entitytype, name, geom, length)
select gid, 2, address, geom, s.distance from sides s;

select pgr_createtopology('network', 0.00001, 'geom', 'gid')

*/
