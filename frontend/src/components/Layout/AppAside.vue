<template>
  <div>
    <el-aside>
      <el-scrollbar>
        <el-menu router unique-opened :collapse="isCollapse">
          <a href="/" class="logo">
            <img src="@/assets/logo.svg" alt="" />
            <h1>My Project</h1>
          </a>
          <el-sub-menu index="1">
            <template #title>
              <el-icon><IEpStar /></el-icon><span>Introduction</span>
            </template>
            <el-menu-item index="/basic">
              <el-icon><IEpStar /></el-icon><span>Basic Info</span>
            </el-menu-item>
            <el-menu-item index="/about">
              <el-icon><IEpStar /></el-icon><span>About</span>
            </el-menu-item>
          </el-sub-menu>
          <el-sub-menu index="2">
            <template #title>
              <el-icon><IEpStar /></el-icon><span>Projects</span>
            </template>
            <el-menu-item index="/shortenUrl">
              <el-icon><IEpStar /></el-icon><span>ShortenURL</span>
            </el-menu-item>
            <el-menu-item index="/pastebin">
              <el-icon><IEpStar /></el-icon><span>Pastebin</span>
            </el-menu-item>
            <el-menu-item index="/continue">
              <el-icon><IEpStar /></el-icon><span>Continue</span>
            </el-menu-item>
          </el-sub-menu>
        </el-menu></el-scrollbar
      >
    </el-aside>
  </div>
</template>

<script setup lang="ts">
//const isCollapse = ref(false);
import { isCollapse } from './isCollapse';
</script>

<style lang="scss" scoped>
.el-aside {
  background-color: #e9edc9;
  height: 100vh;
  width: auto;
}

//菜单样式
.el-menu {
  background-color: #e9edc9;
  border-right: none;
  width: 200px;
  /*& represents father elements*/
  &.el-menu--collapse {
    width: 60px;
    & h1 {
      display: none;
    }
  }
  & .logo {
    display: flex;
    justify-content: center;
    align-items: center;
    text-decoration: none;
    color: black;
    height: 60px;
    img {
      width: 32px;
      height: 32px;
    }
  }
}
</style>
