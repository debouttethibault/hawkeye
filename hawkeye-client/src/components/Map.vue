<script setup lang="ts">
import 'leaflet/dist/leaflet.css';
import { LMap, LTileLayer, LPolyline } from '@vue-leaflet/vue-leaflet';
import { onMounted, onUnmounted } from 'vue';
import { useMapStore } from '@/stores/map';

const mapStore = useMapStore();

onMounted(() => {
  mapStore.startLocalPositionUpdates();
});

onUnmounted(() => {
  mapStore.stopLocalPositionUpdates();
});
</script>

<template>
  <div style="width: 100%; height: 100%">
    <l-map id="map" ref="map" :zoom="12" :center="[51, 3]" :use-global-leaflet="false">
      <!-- <l-tile-layer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        layer-type="base"
        name="OSM"
        attribution="OpenStreetMap"
      ></l-tile-layer> -->
      <l-tile-layer
        url="https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}"
        layer-type="base"
        name="ArcGIS"
        attribution="Tiles &copy; Esri &mdash; Source: Esri, i-cubed, USDA, USGS, AEX, GeoEye, Getmapping, Aerogrid, IGN, IGP, UPR-EGP, and the GIS User Community"
      ></l-tile-layer>
      <l-polyline :lat-lngs="[]" color="green" />
    </l-map>
  </div>
</template>
