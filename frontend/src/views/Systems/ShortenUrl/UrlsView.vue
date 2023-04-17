<template>
  <div class="myUrls">
    <el-card class="box-card">
      <template #header>
        <div class="card-header">
          <span>Manage your Urls</span>
        </div>
      </template>
      <div class="card-body">
        <el-table :data="urls" stripe style="width: 100%">
          <el-table-column prop="originalUrl" label="Original Url" />
          <el-table-column prop="shortenedUrl" label="Short Url" />
          <el-table-column
            prop="creationTime"
            label="Creation Time"
            :formatter="dateToTableFormat"
          />
          <el-table-column
            prop="expirationTime"
            label="Expiration Time"
            :formatter="dateToTableFormat"
          />
          <el-table-column prop="address" label="Edit" align="center" v-slot="{ row }">
            <el-button type="primary" @click="editView?.showEditor_(row.urlKey)">Edit</el-button>
            <el-button type="danger" @click="onDelete(row.urlKey)">Remove</el-button>
          </el-table-column>
        </el-table>
      </div>
    </el-card>
    <EditView ref="editView" />
  </div>
</template>

<script setup lang="ts">
import { urls, delelteUrl, getAllUrls } from './EditScript';
import EditView from './EditView.vue';
import { dateToTableFormat } from '@/utils/formatter';
const editView = ref<InstanceType<typeof EditView>>();
getAllUrls();
const onDelete = (urlKey: string) => {
  delelteUrl(urlKey);
  getAllUrls();
};
</script>

<style scoped>
.myUrls {
  display: flex;
  justify-content: center;
  align-items: center;
}
.card-header {
  display: flex;
  justify-content: center;
  align-items: center;
}
.card-body {
  display: flex;
  justify-content: center;
  align-items: center;
}
.box-card {
  width: 80%;
}
</style>
