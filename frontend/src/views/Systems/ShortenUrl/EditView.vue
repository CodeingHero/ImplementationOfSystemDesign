<template>
  <div class="edit-dlg">
    <el-dialog v-model="isEditorVisible" title="Add expiration days">
      <el-form :label-position="'top'" label-width="100px" :model="formattedUrl" ref="frmEditDlg">
        <el-form-item label="Original Url">
          <el-input v-model="formattedUrl.originalUrl" readonly />
        </el-form-item>
        <el-form-item label="Short Url">
          <el-input v-model="formattedUrl.shortenedUrl" readonly />
        </el-form-item>
        <el-form-item label="CreationTime">
          <el-input v-model="formattedUrl.creationTime" readonly />
        </el-form-item>
        <el-form-item label="ExpirationTime">
          <el-input v-model="formattedUrl.expirationTime" readonly />
          <el-input-number
            v-model="addDays"
            :min="0"
            :max="365"
            @change="onAddDaysChange"
            style="margin-left: 15px; margin-right: 15px"
          />
          Days
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="isEditorVisible = false">Cancel</el-button>
          <el-button type="primary" @click="onSubmit"> Confirm </el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import {
  isEditorVisible,
  editUrl,
  showEditor,
  formattedUrl,
  addExpirationTime,
  getAllUrls
} from './EditScript';
import { dateToTableFormat, dateStringFormat } from '@/utils/formatter';
import { toNestedArr } from 'element-plus/es/components/calendar/src/date-table';
import type { FormInstance } from 'element-plus';
const addDays = ref(0);
const onSubmit = () => {
  addExpirationTime(addDays.value);
  isEditorVisible.value = false;
  getAllUrls();
};

const onAddDaysChange = (cur: number | undefined, prev: any | undefined) => {
  console.log(cur);
  const newDate = new Date(formattedUrl.value.expirationTime);
  newDate.setTime(newDate.getTime() + cur! * 24 * 60 * 60 * 1000);
  formattedUrl.value = {
    ...formattedUrl.value,
    expirationTime: newDate.toString()
  };
  return cur;
};
const frmEditDlg = ref<FormInstance>();

const showEditor_ = (urlKey = '') => {
  //frmEditDlg.value?.resetFields();
  addDays.value = 0;
  showEditor(urlKey);
};
defineExpose({
  showEditor_
});
</script>

<style scoped>
.edit-dlg {
  display: flex;
  justify-content: center;
  align-items: center;
}
.el-button--text {
  margin-right: 15px;
}
.el-select {
  width: 300px;
}
.el-input {
  width: 300px;
}
.dialog-footer button:first-child {
  margin-right: 10px;
}
</style>
