CREATE OR REPLACE FUNCTION public.route_single(in _x1 float8, in _y1 float8, in _x2 float8, in _y2 float8)
  RETURNS table (id int, node int, edge int, name text, cost double PRECISION, length_m DOUBLE PRECISION, the_geom GEOMETRY)
AS
$BODY$
    declare v1 bigint;
    declare v2 bigint;
    BEGIN
      select v.id into v1 from network_vertices_pgr v
      order by st_distance(v.the_geom,
         (select w.geom from network w
          order by ST_Distance(w.geom, st_geomfromtext('point(' || _x1 || ' ' || _y1 || ')', 4326)) asc
          limit 1)
      ) asc, st_distance(v.the_geom, st_geomfromtext('point(' || _x1 || ' ' || _y1 || ')', 4326)) asc
      limit 1;
      select v.id into v2 from network_vertices_pgr v
      order by st_distance(v.the_geom,
         (select w.geom from network w
          order by ST_Distance(w.geom, st_geomfromtext('point(' || _x2 || ' ' || _y2 || ')', 4326)) asc
          limit 1)
      ) asc, st_distance(v.the_geom, st_geomfromtext('point(' || _x2 || ' ' || _y2 || ')', 4326)) asc
      limit 1;
      --raise notice '%,%', way1, way2;
      return query
        SELECT seq, id1 AS node, id2 AS edge, w.name, d.cost, w.length as length_m, w.geom as the_geom FROM pgr_dijkstra('
          SELECT n.gid::integer AS id,
           n.source,
           n.target,
           -- TODO: need to group / product here where multiple congestions affect the same entity....
           n.length * coalesce(ct.cost, 1) as cost -- big is heavy
          FROM network n
          LEFT OUTER JOIN networkcongestion nc on n.gid = nc.networkid
          left join congestion c on c.gid = nc.congestionid
          left join congestiontype ct on c.congestiontypeid = ct.congestiontypeid
          where c.gid is null or now() between c.starton and c.endon',
          v1, v2, false, false) as d
        left join network w on d.id2 = w.gid;
    END;
$BODY$
LANGUAGE plpgsql VOLATILE;

-- select * from public.route_single(144.96183693408963, -37.81481447789581, 144.96620357036588,-37.81511960401554)
