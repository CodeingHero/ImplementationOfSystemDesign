<template>
  <div class="login">
    <el-form
      :model="form"
      :rules="rules"
      ref="formRef"
      label-width="120px"
      label-position="top"
      size="large"
    >
      <div class="input">
        <h1>Implementations of Grokking System Design</h1>
        <h2>LOG IN</h2>
        <el-form-item label="Email/User Name/ID" prop="userKey">
          <el-input v-model="form.userKey" type="" placeholder="Email/User Name/ID" />
        </el-form-item>
        <el-form-item label="Password" prop="password">
          <el-input v-model="form.password" type="password" placeholder="Password" />
        </el-form-item>

        <el-form-item>
          <el-button type="primary" @click="onSubmit" :loading="isLoading">LOG IN</el-button>
        </el-form-item>
        <el-link @click="toRegister">Register>></el-link>
      </div>
    </el-form>
  </div>
</template>

<script setup lang="ts">
import type { FormInstance, FormRules } from 'element-plus';
import { login } from '@/apis/userApi';
import { tokenManager } from '@/stores/myToken';
import { useRouter, useRoute } from 'vue-router';

import { ElMessage } from 'element-plus';

//save
const tokenStore = tokenManager();
const router = useRouter();
const route = useRoute();
const form = reactive({
  userKey: 'jyw',
  password: '123456'
});

const isLoading = ref(false);
const onSubmit = async () => {
  //isLoading.value = true;
  //validate form
  await formRef.value?.validate().catch((err) => {
    ElMessage.error('Valadation failed');
    throw err;
  });

  // login request
  const data = await login(form).then((res) => {
    if (!res.data.isSuccess) {
      ElMessage.error(res.data.message);
      throw new Error(res.data.message);
    }
    return res.data;
  });

  console.log(data);

  //save token(access and refresh) to local storage
  tokenStore.saveToken(JSON.stringify(data.data.token));
  //restore login button state
  isLoading.value = false;
  ElMessage.success('Login succeed');

  // //Check if need redirec
  router.push((route.query.redirect as string) || '/');
};

const toRegister = () => {
  router.push('/register');
};

//form validations
const rules = reactive<FormRules>({
  userKey: [
    { required: true, message: 'Email/User Name/ID は必須です', trigger: 'change' },
    { min: 3, message: 'Email/User Name/ID は4桁以上です。', trigger: 'change' }
  ],
  password: [
    { required: true, message: 'パスワードは必須です', trigger: 'blur' },
    { max: 18, min: 6, message: 'パスワードの長さは6~18桁です' }
  ]
});

const formRef = ref<FormInstance>();
</script>

<style scoped>
.login {
  background-color: #ccc;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}

.el-form {
  width: 330px;
  height: 600px;
  background-color: #fff;
  border-radius: 10px;
  padding: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.el-form h1 {
  text-align: center;
  margin-bottom: 50px;
  width: 100%;
}

.el-form el-link {
  float: right;
  text-decoration: none;
  color: blue;
}

/* el-link:hover {
  color: red;
} */

.el-form .input {
  width: 300px;
}

.el-form-item {
  margin-top: 10px;
}

.el-button {
  width: 100%;
  margin-top: 10px;
}
</style>
