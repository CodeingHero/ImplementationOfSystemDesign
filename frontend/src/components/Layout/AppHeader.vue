<template>
  <div>
    <el-header>
      <!-- Icons -->
      <el-icon @click="isCollapse = !isCollapse">
        <IEpExpand v-show="isCollapse" />
        <IEpFold v-show="!isCollapse" />
      </el-icon>

      <!-- breadcumb -->
      <el-breadcrumb separator="/">
        <el-breadcrumb-item :to="{ name: 'home' }">Home</el-breadcrumb-item>
        <el-breadcrumb-item v-if="$route.meta.breadcrumb">{{
          $route.meta.breadcrumb
        }}</el-breadcrumb-item>
      </el-breadcrumb>

      <!-- pull down list -->

      <el-dropdown>
        <span class="el-dropdown-link">
          <el-avatar :size="32" :src="userInfo.portrait" />
          <el-icon class="el-icon--right">
            <IEparrow-down />
          </el-icon>
        </span>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item
              @click="
                router.push({
                  name: 'userInfo',
                  params: { id: userInfo.userID }
                })
              "
              >{{ userInfo.userName }}</el-dropdown-item
            >
            <el-dropdown-item divided @click="handleLogout">Log out</el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
    </el-header>
  </div>
</template>

<script setup lang="ts">
//const isCollapse = ref(true);
import { isCollapse } from './isCollapse';
import { getUserInfo, logout } from '@/apis/userApi';
import { useRouter } from 'vue-router';
import { tokenManager } from '@/stores/myToken';

const router = useRouter();

const userInfo = ref({ portrait: '', userName: '', userID: 0 });

getUserInfo().then((res) => {
  console.log(res);
  userInfo.value = res.data.data;
});
// const breadCrumbItem = computed(() => {
//   const items = [
//     {
//       path: '/',
//       name: 'homepage'
//     },
//     {
//       path: '/promotion',
//       name: 'promotion management'
//     },
//     {
//       path: '/promotion/list',
//       name: 'promotion list'
//     },
//     {
//       path: '/promotion/detail',
//       name: 'promotion detail'
//     }
//   ];
//   return items;
// });

const handleLogout = async () => {
  //1. confirm question
  await ElMessageBox.confirm('本当にログアウトしますか？', '閉じる', {
    confirmButtonText: 'はい',
    cancelButtonText: 'いいえ',
    type: 'warning',
    appendTo: 'body'
  }).catch(() => {
    ElMessage.info('logout has been cenceled');
    return new Promise(() => {});
  });

  //2.logout
  await logout().catch(() => {});
  //3 clear token
  tokenManager().clearToken();
  ElMessage.success('Logout suceed');

  //4 jump tp login
  router.push({ name: 'login' });
};
</script>

<style lang="scss" scoped>
/* to horizontal */
.el-header {
  display: flex;
  align-items: center;
  background-color: #ccd5ae;
  & .el-breadcrumb {
    margin-left: 20px;
  }
}
.el-dropdown {
  /* Move user info to right */
  margin-left: auto;
  .el-dropdown-link {
    display: flex;
    justify-content: center;
    align-items: center;
  }
}
.el-message-box__wrapper {
  z-index: 9999;
}
</style>
