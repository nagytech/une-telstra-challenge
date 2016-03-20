<?php

$id=floatval($_GET["osm_id"]);

$form=$_GET["format"];

$db = pg_connect("host=127.0.0.1 dbname=gis user=gis password=gispass")
        or die('no psql found');

$t = 'select * from public.id_line($1)';
$q = sprintf($t, $id);

$r = pg_query_params($db, $q, array($id)) or die ('bad query: ' . pg_last_error($db));

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
<h1>Attributes for line osm_id <?php echo "$id"; ?></h1>
<table>
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



