<template>
  <div>
    <el-card class="box-card">
      <template #header>
        <div class="card-header">
          <span>Manage your personal profile</span>
        </div>
        <div>
          <el-form
            :label-position="labelPosition"
            :model="userInfoForm"
            class="userInfo-form"
            label-width="100px"
          >
            <el-form-item label="User ID">
              <el-input v-model.number="userInfoForm.userID" disabled />
            </el-form-item>
            <el-form-item label="User Name">
              <el-input v-model="userInfoForm.userName" disabled />
            </el-form-item>
            <el-form-item label="Nick Name">
              <el-input v-model="userInfoForm.nickName" />
            </el-form-item>
            <el-form-item label="Email">
              <el-input v-model="userInfoForm.email" />
            </el-form-item>
            <el-checkbox v-model="isChangePassword" label="Change Password?" size="large" />
            <el-form-item label="Prev Password">
              <el-input
                v-model="userInfoForm.prevPassword"
                type="password"
                autocomplete="off"
                :disabled="isDisablePasswordChange"
              />
            </el-form-item>
            <el-form-item label="New Password">
              <el-input
                v-model="userInfoForm.newPassword"
                type="password"
                autocomplete="off"
                :disabled="isDisablePasswordChange"
              />
            </el-form-item>
            <el-form-item label="Confirm New">
              <el-input
                v-model="userInfoForm.confirmPassword"
                type="password"
                autocomplete="off"
                :disabled="isDisablePasswordChange"
              />
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="submitForm">Update</el-button>
            </el-form-item>
          </el-form>
        </div>
      </template>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import type { UserInfo, UserUpdateInfo } from '@/apis/userApi';
import { getUserInfo, updateUserInfo } from '@/apis/userApi';
import { useRouter } from 'vue-router';
//setup

const router = useRouter();
//define type
type UserInfoForm = Omit<UserInfo, 'portrait'> & {
  prevPassword: string;
  newPassword: string;
  confirmPassword: string;
};
//define props
const userInfoForm = reactive<UserInfoForm>({
  userID: -1,
  userName: '',
  email: '',
  nickName: '',
  prevPassword: '',
  newPassword: '',
  confirmPassword: ''
});

const updateData = computed(() => {
  return {
    userID: userInfoForm.userID,
    nickName: userInfoForm.nickName,
    userName: userInfoForm.userName,
    email: userInfoForm.email,
    prevPassword: isChangePassword ? userInfoForm.prevPassword : '',
    newPassword: isChangePassword ? userInfoForm.newPassword : '',
    confirmPassword: isChangePassword ? userInfoForm.confirmPassword : ''
  } as UserUpdateInfo;
});

const isChangePassword = ref(false);
console.log('test' + isChangePassword.value);
const isDisablePasswordChange = computed<boolean>({
  get() {
    return !isChangePassword.value;
  },
  set(value: boolean) {
    isChangePassword.value = !value;
  }
});
const labelPosition = ref('top' as 'top' | 'left' | 'right');

//define methods

// get user info from backend
const getUserInfoFromBackend = async () => {
  const data = await getUserInfo()
    .then((res) => {
      console.log(res.data);
      ElMessage.success('Get User Info Success');
      return res.data;
    })
    .catch((err) => {
      ElMessage.error('Get User Info Failed \n' + err);
      router.push('/');
      throw err;
    });
  userInfoForm.userID = data.data.userID;
  userInfoForm.userName = data.data.userName;
  userInfoForm.email = data.data.email;
  userInfoForm.nickName = data.data.nickName;
};

//submit to update user info
const submitForm = async () => {
  const data = await updateUserInfo(updateData.value)
    .then((res) => {
      console.log(res.data);
      return res.data;
    })
    .catch((err) => {
      ElMessage.error('Update Failed \n' + err);
      throw err;
    });
  if (data.isSuccess) {
    ElMessage.success('Update Success');
  } else {
    ElMessage.error('Update Failed');
    throw new Error('Update Failed');
  }
};
getUserInfoFromBackend();
//script setup
onMounted(() => {
  isChangePassword.value = false;
});
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
  width: 30vw;
  margin: 0 auto;
}

.el-form {
  text-align: center;
  justify-content: center;
}
</style>
