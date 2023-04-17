<template>
  <div>
    <el-card class="box-card">
      <template #header>
        <div class="card-header">
          <h1>プロジェクトの紹介</h1>
        </div>
      </template>
      <div>
        <p id="introduction-title"></p>
        <p v-text="getIntroInfo.text"></p>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import * as indexApi from '@/apis/indexApi';
//properties
const introInfo = reactive<indexApi.Introduction>({
  title: '',
  text: ''
});

const getIntroInfo = computed(() => {
  return {
    title: introInfo.title,
    text: introInfo.text
  } as indexApi.Introduction;
});

//methods
const requestIntroduction = async () => {
  try {
    const { data } = await indexApi.getIntroduction();
    if (data.isSuccess) {
      introInfo.text = data.data.text;
      introInfo.title = data.data.title;
      console.log(introInfo.text);
    } else {
      throw new Error(data.message);
    }
  } catch (err) {
    ElMessage.error('err');
    console.error(err);
  }
};

//hook methods
onMounted(async () => {
  await requestIntroduction();
});
</script>

<style scoped>
h1 {
  font-size: 2em;
}
.box-card {
  width: 98%;
}
p {
  white-space: pre-wrap;
}
</style>
