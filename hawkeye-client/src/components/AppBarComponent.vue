<template>
  <v-app-bar>
    <v-app-bar-title>Hawkeye Mission Planner</v-app-bar-title>
    <template #append>
      <v-menu>
        <template v-slot:activator="{ props }">
          <v-avatar
            class="mr-2"
            v-bind="props"
            :image="`https://via.assets.so/album.png?id=${Math.round(Math.random() * 100)}&q=95&w=360&h=360&fit=fill`"
          ></v-avatar>
        </template>
        <v-list>
          <v-list-item>
            <v-list-item-title>Thibault Deboutte</v-list-item-title>
            <v-list-item-subtitle>thibault.deboutte@gmail.com</v-list-item-subtitle>
          </v-list-item>
          <v-divider />
          <v-list-item>
            <v-list-item-title>TODO</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </template>
  </v-app-bar>
</template>
