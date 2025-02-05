<script setup lang="ts">
import { onMounted, onUnmounted } from 'vue';

const onContextMenu = (ev: MouseEvent) => {
  ev.preventDefault();

  console.log(ev.pageX, ev.pageY, ev.target);
};

onMounted(() => {
  document.addEventListener('contextmenu', onContextMenu);
});
onUnmounted(() => {
  document.removeEventListener('contextmenu', onContextMenu);
});
</script>
<template>
  <v-menu persistent>
    <v-list>
      <v-list-item>
        <v-list-item-title>Test 1</v-list-item-title>
      </v-list-item>
      <v-list-item>
        <v-list-item-title>Test 2</v-list-item-title>
      </v-list-item>
    </v-list>
  </v-menu>
</template>
