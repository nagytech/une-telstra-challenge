/* Selects a route between two points (x1y1, x2y2)
 */

CREATE OR REPLACE FUNCTION public.route_single(in _x1 float8, in _y1 float8, in _x2 float8, in _y2 float8)
  RETURNS table (id int, node int, edge int, name text, cost double PRECISION, length_m DOUBLE PRECISION, the_geom GEOMETRY)
AS
$BODY$
    declare way1 bigint;
    declare way2 bigint;
    BEGIN
      -- TODO: This needs work.  It ends up giving us the wrong legs.
      select w.source into way1 from ways w
        order by ST_Distance(w.the_geom, st_geomfromtext('point(' || _x1 || ' ' || _y1 || ')', 4326))
      limit 1;
      select w.source into way2 from ways w
        order by ST_Distance(w.the_geom, st_geomfromtext('point(' || _x2 || ' ' || _y2 || ')', 4326))
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
          way1, way2, false, false) as d
        left join ways w on d.id2 = w.gid;
    END;
$BODY$
LANGUAGE plpgsql VOLATILE
