<?php

$type=$_POST["type"];
$wkt=$_POST["wkt"];
$start=$_POST["start"];
$end=$_POST["end"];

$db = pg_connect("host=127.0.0.1 dbname=gis user=gis password=gispass")
        or die('no psql found');

$t = 'select * from public.add_congestion($1, $2, $3, $4)';
$q = sprintf($t, $id);

$r = pg_query_params($db, $q, array($type, $start, $end, $wkt)) or die ('bad query: ' . pg_last_error($db));

// TODO: Check result, return 400 on bad request

?>

