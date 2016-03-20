<?php

$id=floatval($_GET["osm_id"]);
$type=$_POST["type"];

$db = pg_connect("host=127.0.0.1 dbname=gis user=gis password=gispass")
        or die('no psql found');

$t = 'select * from public.edit_line($1, $2)';
$q = sprintf($t, $id);

$r = pg_query_params($db, $q, array($id, $type)) or die ('bad query: ' . pg_last_error($db));

// TODO: Check result, return 400 on bad request

?>

