/*
 * Finds network lines within the given extent
 *
 * NOTE: x1, y1 needs to be lower left, where x2, y2 is upper right.
 *
 */
CREATE OR REPLACE FUNCTION public.extent_lines(_x1 float8, _y1 float8, _x2 float8, _y2 float8)
  returns table (gid INTEGER, length DOUBLE PRECISION, name text, wkt text)
  AS
  $$
  declare extent geometry;
  begin
    select st_geomfromtext('polygon(('||_x1||' '||_y1||','||_x2||' '||_y1||','||_x2||' '||_y2||','||_x1||' '||_y2||','||_x1||' '||_y1||'))', 4326) into extent;
    return query SELECT
      n.gid,
      n.length,
      n.name,
      st_astext(geom) wkt
    FROM network n
    where st_intersects(geom, extent)
    limit 1000;
end;
$$
LANGUAGE plpgsql
