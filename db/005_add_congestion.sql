CREATE OR REPLACE FUNCTION public.add_congestion(_type text, _start text, _end text, _wkt text)
  RETURNS void
  AS
  $$
  declare bounds geometry;
  declare typeid int;
  declare lastid int;
  begin
    select st_geomfromtext(_wkt, 4326) into bounds;
    select congestiontypeid into typeid from congestiontype where name = _type;
    insert into congestion
        (congestiontypeid, name, starton, endon, address, geometry)
      values
        (typeid, 'TODO', date(_start), date(_end), null, bounds);
    select currval(pg_get_serial_sequence('congestion','gid')) into lastid;
    insert into networkcongestion
        (congestionid, networkid) (
          select lastid, gid
          from network where st_intersects(geom, bounds)
    );
  end;
  $$
LANGUAGE plpgsql;
