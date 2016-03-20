<?php

$x1=$_GET["x1"];
$y1=$_GET["y1"];
$x2=$_GET["x2"];
$y2=$_GET["y2"];

$form=$_GET["format"];

$db = pg_connect("host=127.0.0.1 dbname=gis user=gis password=gispass")
        or die('no psql found');

$q = 'select id, node, edge, cost, length_m, name, st_astext(the_geom) from public.route_single($1, $2, $3, $4)';

$r = pg_query_params($db, $q, array($x1, $y1, $x2, $y2) or die ('bad query: ' . pg_last_error($db));

if ($form != 'html') {
$a = array();
while ($l = pg_fetch_array($r, null, PGSQL_ASSOC)) {
  $a[] = $l;
}
echo json_encode($a);
} else {
?>
<!doctype HTML>
<style>
table, th, tr, td {
  border: 1px solid #444;
  border-collapse: collapse;
}
</style>
<h1>Route from <?php echo "$x1,$y1 to $x2,$y2"; ?></h1>
<table>
<thead>
<th>seq</th><th>node</th><th>edge</th><th>weight</th><th>dist_m</th><th>name</th><th>geom</th>
</thead>
<?php
while ($l = pg_fetch_array($r, null, PGSQL_ASSOC)) {
  echo '<tr>';
  foreach ($l as $c) {
    echo "<td>$c</td>";
  }
  echo '</tr>';
}
echo '</table>';

}

?>


