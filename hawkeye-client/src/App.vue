<script setup lang="ts">
import { ref } from 'vue';
import Map from './components/Map.vue';
import ActionMenu from './components/ActionMenu.vue';

const drawerOpen = ref(true);
</script>

<template>
  <v-app>
    <ActionMenu />
    <v-navigation-drawer color="black-lighten-3" v-model="drawerOpen"> </v-navigation-drawer>
    <v-main>
      <Map />
    </v-main>
  </v-app>
</template>
