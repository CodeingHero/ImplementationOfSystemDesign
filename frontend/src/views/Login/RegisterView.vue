<template>
  <div class="register">
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
        <h2>REGISTER</h2>
        <el-form-item label="Email/User Name/ID" prop="userKey">
          <el-input
            v-model="form.userKey"
            type=""
            placeholder="Email/User Name/ID"
            autocomplete="off"
          />
        </el-form-item>
        <el-form-item label="Password" prop="password">
          <el-input
            v-model="form.password"
            type="password"
            placeholder="Password"
            autocomplete="off"
          />
        </el-form-item>

        <el-form-item>
          <el-button type="primary" @click="onRegister" :loading="isLoading">Register</el-button>
        </el-form-item>
        <el-link @click="onReturn">&lt;&lt;Return</el-link>
      </div>
    </el-form>
  </div>
</template>

<script setup lang="ts">
import type { FormInstance, FormRules } from 'element-plus';
import { useRouter } from 'vue-router';
import { register } from '@/apis/userApi';
import { tokenManager } from '@/stores/myToken';

const router = useRouter();
const formRef = ref<FormInstance>();
const tokenStore = tokenManager();
const isLoading = ref(false);
const form = reactive({
  userKey: '',
  password: ''
});
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

const onRegister = async () => {
  ElMessage.info('test');
  //validate form
  await formRef.value?.validate().catch((err) => {
    ElMessage.error('Valadation failed');
    throw err;
  });

  // register request
  const data = await register(form).then((res) => {
    if (!res.data.isSuccess) {
      ElMessage.error('Register failed:' + res.data.message);
      throw new Error(res.data.message);
    }
    return res.data.data;
  });
  tokenStore.saveToken(data.token);
  isLoading.value = false;
  ElMessage.success('Register success');
  router.push('/home');
};

const onReturn = () => {
  router.push('/login');
};
</script>
<style scoped>
.register {
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
}

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
