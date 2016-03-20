/* 
 *  Identify a single line
 */
CREATE OR REPLACE FUNCTION public.id_line(_osm_id bigint)
  returns table (gid bigint, osm_id bigint, length_m DOUBLE PRECISION, name text, wkt text)
  AS
  $$
  begin
    return query SELECT
      w.gid,
      w.osm_id,
      w.length_m,
      c.name, -- TODO: Should be type, not name.
      st_astext(w.the_geom) wkt
    FROM ways w
    LEFT JOIN osm_way_classes c on w.class_id = c.class_id
    where w.osm_id = _osm_id;
end;
$$
LANGUAGE plpgsql;


  
