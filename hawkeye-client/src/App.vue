<script setup lang="ts">
import AppBar from './components/AppBarComponent.vue';
import MyMap from './components/MapComponent.vue';
import Drawer from './components/DrawerComponent.vue';
</script>

<template>
  <v-app>
    <app-bar />
    <drawer />
    <v-main>
      <my-map />
    </v-main>
  </v-app>
</template>
