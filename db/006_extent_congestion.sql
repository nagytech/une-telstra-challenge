CREATE OR REPLACE FUNCTION public.extent_congestion(_x1 float8, _y1 float8, _x2 float8, _y2 float8)
  returns table (gid INTEGER, type int, name text, starton timestamp, endon timestamp, wkt text)
  AS
  $$
  declare extent geometry;
  begin
    select st_geomfromtext('polygon(('||_x1||' '||_y1||','||_x2||' '||_y1||','||_x2||' '||_y2||','||_x1||' '||_y2||','||_x1||' '||_y1||'))', 4326) into extent;
    return query SELECT
      c.gid,
      c.congestiontypeid as type,
      c.name,
      c.starton,
      c.endon,
      st_astext(c.geometry) as wkt
    FROM congestion c
    where st_intersects(c.geometry, extent)
    limit 1000;
end;
$$
LANGUAGE plpgsql
