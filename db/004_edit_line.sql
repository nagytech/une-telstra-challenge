/* 
 *  Identify a single line
 */
CREATE OR REPLACE FUNCTION public.edit_line(_osm_id bigint, class_name text)
  RETURNS void  
  AS
  $$
  begin
    update ways as w
    set	class_id = c.class_id
    from osm_way_classes as c
    where w.osm_id = _osm_id AND c.name = class_name;
end;
$$
LANGUAGE plpgsql;


  
