import 'leaflet/dist/leaflet.css';
import { Map, TileLayer, Layer } from 'leaflet';
import { computed, ref } from 'vue';
import { defineStore } from 'pinia';

class MyLocationUpdates {
  positionWatchId?: number;
  positionCallback?: (pos: GeolocationPosition) => void;
  position?: GeolocationPosition;

  start() {
    console.log('geo start');
    if (navigator.geolocation || !this.positionWatchId) {
      console.log('geo check');
      this.positionWatchId = navigator.geolocation.watchPosition((pos) => {
        console.log('geo watch');
        this.position = pos;
        if (this.positionCallback) {
          this.positionCallback(pos);
        }
      });
    }
  }

  stop() {
    console.log('geo stop');
    if (this.positionWatchId) {
      navigator.geolocation.clearWatch(this.positionWatchId);
      console.log('geo clear');
    }
  }
}

class MyMap {
  readonly locationUpdate: MyLocationUpdates;

  readonly map: Map;
  readonly layers: {[name: string]: Layer};

  constructor(el: HTMLDivElement) {
    this.map = new Map(el, {
      center: [50.5, 3.1],
      zoom: 12,
    });

    const osmLayer = new TileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
      pane: 'tilePane'
    });

    const arcGisLayer = new TileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
      attribution: '&copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community',
      pane: 'tilePane'
    });

    this.layers = {
      'ArcGIS': arcGisLayer,
      'OpenStreetMap': osmLayer,
    };

    this.currentLayer = 'ArcGIS';

    this.locationUpdate = new MyLocationUpdates();
  }

  destroy() {
    this.map.off();
    this.map.remove();
  }

  _currentLayer: string = '';

  set currentLayer(key: string) {
    if (!Object.keys(this.layers).includes(key)
        || this._currentLayer == 'key') {
      return;
    }

    if (this._currentLayer) {
      const oldLayer = this.layers[this._currentLayer];
      oldLayer.removeFrom(this.map);
    }

    const newLayer = this.layers[key];
    newLayer.addTo(this.map);
  }

  get currentLayer() {
    return this._currentLayer;
  }
}

const useMapStore = defineStore('map', () => {
  const map = ref<MyMap>();

  const initiaze = (el: HTMLDivElement) => {
    map.value = new MyMap(el);
  }

  const destroy = () => {
    map.value?.destroy();
  }

  return { initiaze, destroy,  };
});

export default useMapStore;
