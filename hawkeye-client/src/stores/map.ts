import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useMapStore = defineStore('map', () => {
  const localPositionWatchId = ref(0);
  const localPosition = ref<GeolocationCoordinates>();

  const startLocalPositionUpdates = () => {
    localPositionWatchId.value = navigator.geolocation.watchPosition((pos) => {
      localPosition.value = pos.coords;
    });
  };

  const stopLocalPositionUpdates = () => {
    if (localPositionWatchId.value) {
      navigator.geolocation.clearWatch(localPositionWatchId.value);
    }
  };

  return { localPosition, startLocalPositionUpdates, stopLocalPositionUpdates };
});
