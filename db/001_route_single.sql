CREATE OR REPLACE FUNCTION public.route_single(in _x1 float8, in _y1 float8, in _x2 float8, in _y2 float8)
  RETURNS table (id int, node int, edge int, name text, cost double PRECISION, length_m DOUBLE PRECISION, the_geom GEOMETRY)
AS
$BODY$
    declare v1 bigint;
    declare v2 bigint;
    BEGIN
      select v.id into v1 from ways_vertices_pgr v
      order by st_distance(v.the_geom,
                           (select w.the_geom from ways w
                             left join osm_way_classes c on w.class_id = c.class_id
                           where c.name not in ('motorway','steps')
                            order by ST_Distance(w.the_geom, st_geomfromtext('point(' || _x1 || ' ' || _y1 || ')', 4326)) asc
                            limit 1)
      ) asc, st_distance(v.the_geom, st_geomfromtext('point(' || _x1 || ' ' || _y1 || ')', 4326)) asc
      limit 1;
      select v.id into v2 from ways_vertices_pgr v
      order by st_distance(v.the_geom,
         (select w.the_geom from ways w
           left join osm_way_classes c on w.class_id = c.class_id
         where c.name not in ('motorway','steps')
          order by ST_Distance(w.the_geom, st_geomfromtext('point(' || _x2 || ' ' || _y2 || ')', 4326)) asc
          limit 1)
      ) asc, st_distance(v.the_geom, st_geomfromtext('point(' || _x2 || ' ' || _y2 || ')', 4326)) asc
      limit 1;
      --raise notice '%,%', way1, way2;
      return query
        SELECT seq, id1 AS node, id2 AS edge, w.name,  d.cost, w.length_m, w.the_geom FROM pgr_dijkstra('
          SELECT
             gid::integer AS id,
             source::integer,
             target::integer,
             -- COSTING
             length_m::double precision * c.priority AS cost
          FROM ways w
          left join osm_way_classes c on c.class_id = w.class_id
          -- LIMITATIONS
          where c.name not in (''motorway'',''steps'')',
          v1, v2, false, false) as d
        left join ways w on d.id2 = w.gid;
    END;
$BODY$
LANGUAGE plpgsql VOLATILE


