/* 
 * Finds network lines within the given extent
 *
 * NOTE: x1, y1 needs to be lower left, where x2, y2 is upper right.
 *
 */
CREATE OR REPLACE FUNCTION public.extent_lines(in _x1 float8, in _y1 float8, in _x2 float8, in _y2 float8)
  returns table (gid bigint, osm_id bigint, length_m DOUBLE PRECISION, name text, wkt text)
  AS
  $$
  declare extent geometry;
  begin
    select st_geomfromtext('polygon(('||_x1||' '||_y1||','||_x2||' '||_y1||','||_x2||' '||_y2||','||_x1||' '||_y2||','||_x1||' '||_y1||'))', 4326) into extent;
    return query SELECT
      w.gid,
      w.osm_id,
      w.length_m,
      c.name, -- TODO: This should be 'type', since it's class type not name
      st_astext(w.the_geom) wkt
    FROM ways w
    LEFT JOIN osm_way_classes c on w.class_id = c.class_id
    where st_within(w.the_geom, extent)
    limit 1000;
end;
$$
LANGUAGE plpgsql
