<script setup lang="ts">
import useMapStore from '@/map';
import { onMounted, onUnmounted, ref } from 'vue';

const mapEl = ref<HTMLDivElement>();
const mapStore = useMapStore();

onMounted(() => {
  mapStore.initiaze(mapEl.value!);
});

onUnmounted(() => {
  mapStore.destroy();
});
</script>

<template>
  <div ref="mapEl" style="width: 100%; height: 100%"></div>
</template>
