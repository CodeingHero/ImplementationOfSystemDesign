<template>
  <div>
    <el-tabs v-model="activeName" class="demo-tabs" @tab-click="handleClick">
      <el-tab-pane label="Shorten your URL" name="first">
        <CreateView></CreateView>
      </el-tab-pane>
      <el-tab-pane label="MyUrls" name="second" id="myUrls"><UrlsView></UrlsView></el-tab-pane>
    </el-tabs>
  </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue';
import type { TabsPaneContext } from 'element-plus';
import CreateView from './CreateView.vue';
import UrlsView from './UrlsView.vue';

const activeName = ref('first');

const handleClick = (tab: TabsPaneContext, event: Event) => {
  console.log(tab, event);
};
</script>
<style>
.demo-tabs > .el-tabs__content {
  padding: 15px;
  color: #6b778c;
  font-size: 28px;
  font-weight: 600;
}

el-tab-pane {
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
