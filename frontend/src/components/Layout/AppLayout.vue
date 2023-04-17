<template>
  <div class="common-layout">
    <el-container>
      <AppAside></AppAside>
      <el-container class="header-and-main">
        <AppHeader></AppHeader>
        <el-main>
          <el-scrollbar>
            <!-- RouterView will be replaced by components setted in index.ts -->
            <RouterView />
          </el-scrollbar>
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<script setup lang="ts">
import AppAside from './AppAside.vue';
import AppHeader from './AppHeader.vue';
</script>

<style lang="scss" scoped>
.header-and-main {
  flex-direction: column;
  height: 100vh; /* Set right height to 100 viewpoint */
}
.el-main {
  background-color: #fefae0;
  padding-right: 0;
}
</style>
