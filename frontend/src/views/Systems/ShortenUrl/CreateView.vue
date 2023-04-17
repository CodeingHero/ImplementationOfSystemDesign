<template>
  <div>
    <el-card class="box-card">
      <template #header>
        <div class="card-header">
          <span>Welcome to ShortenUrl</span>
        </div>
      </template>
      <div class="card-body">
        <el-form :label-position="labelPosition" label-width="100px" :model="urlSet">
          <el-form-item label="Enter a long URL to make a Short URL">
            <el-input v-model="urlSet.oriUrl" />
          </el-form-item>
          <el-form-item label="Your shortened URL:">
            <el-input v-model="urlSet.shortenUrl" readonly />
          </el-form-item>
          <el-button class="button" type="primary" @click="onSubmit">Get your short URL!</el-button>
        </el-form>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { createUrl } from '@/apis/shortenUrlApi';
const urlSet = reactive({
  oriUrl: '',
  shortenUrl: ''
});
const labelPosition = ref('top' as 'top' | 'left' | 'right');
const onSubmit = async () => {
  const data = await createUrl(urlSet.oriUrl)
    .then((res) => {
      console.log(res.data);
      return res.data;
    })
    .catch((err) => {
      throw err;
    });
  urlSet.shortenUrl = data.data.shortenedUrl;
};
</script>

<style scoped>
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

.text {
  font-size: 14px;
}

.item {
  margin-bottom: 18px;
}

.box-card {
  width: fit-content;
}

.el-form {
  text-align: center;
  justify-content: center;
}
</style>
