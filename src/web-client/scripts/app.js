/* Global */
var map;
var urlBase = 'http://routable.me';

/* Constants - General Utils */
var proj_int = 'EPSG:3857';
var proj_ext = 'EPSG:4326';
var doOrDont = function(x) {
	return x;
}
var render = function(l) {
	l.render();
}
var clear = function(l) {
	l.clear();
}

/* Form management */
var clearForm = function() {
	drawSource.clear();
	['type', 'start', 'end', 'wkt'].forEach(function(n) {
			$('[name=' + n + ']').val('');
			});
}
var hideForm = function() {
	$('#congest').hide();
}
var loadForm = function(feat) {
	var obj = feat.obj;
	$('[name=osm_id]').val(obj.osm_id);
	$('[name=type]').val(obj.name);
	$('[name=length_m]').val(parseFloat(obj.length_m).toFixed());
}
var drawHandle, drawSource;
var startCongestDraw = function() {
	function addInteraction() {
		drawHandle = new ol.interaction.Draw({
source: drawSource,
type: ('Polygon')
});
drawHandle.on('drawend', function(e) {
		var format = new ol.format.WKT();
		var wkt = format.writeFeature(e.feature);
		var nf = format.readFeature(wkt);
		nf.getGeometry().transform(proj_int, proj_ext);
		wkt = format.writeFeature(nf);
		$('#congest input[name=wkt]').val(wkt);
		map.removeInteraction(drawHandle);
		$('#congest').show();
		});
// TODO: Handle cancel.
map.addInteraction(drawHandle);
}
if (drawHandle) map.removeInteraction(drawHandle);
addInteraction();
}

/* Map Mode */
var mapMode = '';
var routeSource, drawSource;
var switchMapMode = function(mode) {
	switch (mode) {
		case 'map-nav':
			$('#console').show();
			hideForm();
			break;
		case 'map-congest':
			$('#console').hide();
			$('#console').children().remove();
			routeSource.clear();
			startCongestDraw();
			break;
		default:
			$('#console').hide();
			hideForm();
			return;
	}
};

/* Event management */
$(document).on('ready', function() {
		$('.header-tab').on('click', function() {
			var currentMode = $('.header-tab.is-active').attr('id');
			$('.header-tab').removeClass('is-active');
			$(this).addClass('is-active');
			mapMode = $(this).attr('id');
			switchMapMode(mapMode);
			});
		$('#congest-cancel').on('click', function() {
			clearForm();
			hideForm();
			});
		$('#congest-submit').on('click', function() {
			// TODO: Could just serialize the form....
			var type = $('[name=type]').val();
			var start = $('[name=start]').val();
			var end = $('[name=end]').val();
			var wkt = $('[name=wkt]').val();
			$.ajax(urlBase + '/api/congest/new', {
type: 'POST',
data: {
'type': type,
'start': start,
'end': end,
'wkt': wkt
}
})
			.done(function() {

				})
			// TODO: Handle error (at the moment, php gives no error)
			.complete(function() {
				clearForm();
				hideForm();
				layers.map(function(l) {
					l.render();
					})
				});
})
});

/* Mapping Components */
var layer = {
create: function(n, u, vs, vl, po) {
		var l = $.extend({}, layer);
		l.name = n;
		l.url = u;
		if (vs) {
			vl.setSource(vs);
			l.vectorSource = vs,
				l.vectorLayer = vl;
			l.parseObject = po;
		}
		return l;
	},
_tryParseObject: function(o) {
			 try {
				 // Check for empty line
				 if (o && o.length == 1 && o[0] === "") return
					 var f = this.parseObject(o);
				 f.obj = o;
				 return f;
			 } catch (e) {
				 this.errors.push({
						 e,
						 o
						 });
			 }
		 },
name: null,
      url: null,
      objects: [],
      vectorSource: null,
      vectorLayer: null,
      errors: [],
      parseObject: function(o) { /* default */ },
      clear: function() {
	      this.vectorSource.clear();
      },
render: function() {
		// TODO: Check current rendering state, indicate in progress
		// TODO: Other per-layer query filters
		if (!this.vectorSource) return;
		var self = this;
		var extent = map.getView().calculateExtent(map.getSize());
		var ll = ol.proj.transform([extent[0], extent[1]], proj_int, proj_ext);
		var ur = ol.proj.transform([extent[2], extent[3]], proj_int, proj_ext);
		var extentQuery = 'x1=' + ll[0] + '&y1=' + ll[1] + '&x2=' + ur[0] + '&y2=' + ur[1];
		$.ajax(self.url + '?' + extentQuery)
			.done(function(csv) {
					self.objects = JSON.parse(csv);
					})
		.error(function(xhr) {
				self.objects = [];
				console.error("Failed to get feature data: %s", self.url, xhr);
				})
		.complete(function() {
				self.vectorSource.clear();
				self.vectorSource.addFeatures(self.objects
					.map(self._tryParseObject, self)
					.filter(doOrDont));
				if (self.errors && self.errors.length > 0)
				console.error("Failed to render %s objects: %s", self.errors.length, self.name)
				console.debug(self.errors);
				self.errors = [];
				});
	}
};

/* Layers */
var layers = [
/* Lines */
layer.create(
		'lines', urlBase + '/api/lines.php',
		new ol.source.Vector({
projection: proj_int
}),
		new ol.layer.Vector({
maxResolution: 4.6,
style: function(feature, resolution) {
var navableState = function(f) {
var objType = f.obj.name;
var navableTypes = ['footway', 'cycleway', 'path', 'pedestrian'];
var unnavableTypes = ['steps'];
return navableTypes.indexOf(objType) > -1 ? 1 : unnavableTypes.indexOf(objType) > -1 ? -1 : 0;
}
var navableColor = function(state) {
switch (state) {
case 1:
return '#00DD99';
case -1:
return '#FF3300';
default:
return '#0088DD';
}
}
return [new ol.style.Style({
stroke: new ol.style.Stroke({
color: navableColor(navableState(feature)),
width: 2
})
})];
}
}),
	function(d) {
		if (!d) return;
		var format = new ol.format.WKT();
		var feature = format.readFeature(d.wkt);
		feature.getGeometry().transform(proj_ext, proj_int);
		return feature;
	}),
	/* Congestion */
	layer.create(
			'congestion', urlBase + '/api/congest/list.php',
			new ol.source.Vector({
projection: proj_int
}),
			new ol.layer.Vector({
maxResolution: 1.5,
style: function(feature, resolution) {
return [new ol.style.Style({
stroke: new ol.style.Stroke({
color: '#FFA333',
width: 1
}),
fill: new ol.style.Fill({
color: 'rgba(255,100,50,0.5)'
})
})];
}
}),
			function(d) {
			if (!d) return;
			var format = new ol.format.WKT();
			var feature = format.readFeature(d.wkt);
			feature.getGeometry().transform(proj_ext, proj_int);
			return feature;
			}
)
];
layers.getLayer = function(name) {
	for (var i = 0; i < layers.length; i++) {
		if (layers[i].name == name) return layers[i];
    }
};

var init = function() {
    drawSource = new ol.source.Vector({wrapX: false});

    var drawLayer = new ol.layer.Vector({
      source: drawSource,
      style: new ol.style.Style({
        fill: new ol.style.Fill({
          color: 'rgba(255, 255, 255, 0.2)'
        }),
        stroke: new ol.style.Stroke({
          color: '#ffcc33',
          width: 2
        }),
        image: new ol.style.Circle({
          radius: 7,
          fill: new ol.style.Fill({
            color: '#ffcc33'
          })
        })
      })
    });

    routeSource = new ol.source.Vector({
        projection: proj_int
    });
    var routeLayer = new ol.layer.Vector({
        source: routeSource,
        style: function(f, r) {
            return [new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: 'rgba(255,255,0,1)',
                    width: 2
                }),
                image: new ol.style.Circle({
                    radius: 5,
                    fill: new ol.style.Fill({
                        color: 'rgba(255,255,0,1)'
                    }),
                    stroke: new ol.style.Stroke({
                        color: '#000000',
                        width: 2
                    }),
                })
            })];
        }
    });
    map = new ol.Map({
        interactions: ol.interaction.defaults({
            mouseWheelZoom: false
        }),
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM(),
                opacity: '0.3'
            })
        ],
        target: 'map',
        view: new ol.View({
            center: [16137272.090214774, -4553212.478335404],
            zoom: 17,
            minZoom: 17,
            maxZoom: 22,
            projection: proj_int
        })
    });

    var refreshing = 0;
    map.on('moveend', function() {
        var func = map.getView().getResolution() > 4.5 ? clear : render;
        layers.forEach(func);
    });
    var firstClick;
    map.on('singleclick', function(e) {
        var nav = function() {
            if (!firstClick) {
                routeSource.clear();
            }
            var point = new ol.geom.Point(e.coordinate).transform(proj_int, proj_ext);
            routeSource.addFeature(new ol.Feature(new ol.geom.Point(e.coordinate)));
            var cx = $('#console');
            if (!firstClick) {
                firstClick = point;
                cx.children().remove();
                cx.append("<pre>Point 1:          " + firstClick.getCoordinates() + "</pre>");
            } else {
                cx.append("<pre>Point 2:          " + point.getCoordinates() + "</pre>");
                var x1 = firstClick.getCoordinates()[0];
                var y1 = firstClick.getCoordinates()[1];
                var x2 = point.getCoordinates()[0];
                var y2 = point.getCoordinates()[1];
                $.ajax(urlBase + '/api/route?x1=' + x1 + '&y1=' + y1 + '&x2=' + x2 + '&y2=' + y2)
                    .done(function(json) {

                        var format = new ol.format.WKT();
                        var data = JSON.parse(json);

                        var total = 0;
                        var name = '';
                        var type = -1;
                        var directions = [];
                        for (var i = 0; i < data.length; i++) {
                          var d = data[i];
                          if (d.name != name || d.type != type) {
                            if (total > 0) {
                              directions.push({name: name, type: type, total: total });
                            }
                            total = parseFloat(d.length_m);
                            name = d.name;
                            type = d.type;
                          } else {
                            total += parseFloat(d.length_m);
                          }
                        }
                        directions.push({name: name, type: type, total: total });
			cx.append("<pre></pre>");
			directions.map(function(d) {
				if (!d.name) return;
  				if (d.name.startsWith("Intersection of"))
				cx.append("<pre>Cross the " + d.name + " ("+d.total.toFixed(0)+")</pre>");
 				else
				cx.append("<pre>Travel along " + d.name + " for " + d.total.toFixed(0) + "m</pre>");
			});

                        var features = data.map(function(d) {
                            try {
                                var format = new ol.format.WKT();
                                var feature = format.readFeature(d.wkt);
                                feature.getGeometry().transform(proj_ext, proj_int);
                                feature.obj = d;
                                return feature;
                            } catch (e) {
                                return;
                            }
                        });

                        routeSource.addFeatures(features.filter(doOrDont));
                        firstClick = null;
                    });
            }
        };

        if (mapMode == 'map-nav') nav();
        else if (mapMode == 'map-congest-draw') congest();

    })

    layers.forEach(function(l) {
        if (l.vectorSource) map.addLayer(l.vectorLayer);
        l.render();
    });
    map.addLayer(routeLayer);
    map.addLayer(drawLayer)

};
