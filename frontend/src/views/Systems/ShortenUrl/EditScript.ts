import type { ShortenURL } from '@/apis/shortenUrlApi';
import {
  getUrlbyKey,
  getUrlsByUserID,
  deleteUrlbyKey,
  addUrlExpirationTime
} from '@/apis/shortenUrlApi';
import { computed } from 'vue';
import { dateFormat } from '@/utils/formatter';
export const urls = ref<ShortenURL[]>([]);
export const editUrl = ref<ShortenURL>({
  userID: '',
  urlKey: '',
  originalUrl: '',
  shortenedUrl: '',
  creationTime: new Date(),
  expirationTime: new Date()
});

export const getAllUrls = async () => {
  const { data } = await getUrlsByUserID().catch((err) => {
    console.log(err);
    throw err;
  });
  console.log(data);
  urls.value = data.data;
};

export const formattedUrl = computed({
  get: () => {
    return {
      userID: editUrl.value.userID,
      urlKey: editUrl.value.urlKey,
      originalUrl: editUrl.value.originalUrl,
      shortenedUrl: editUrl.value.shortenedUrl,
      creationTime: dateFormat(editUrl.value.creationTime),
      expirationTime: dateFormat(editUrl.value.expirationTime)
    };
  },
  set: (val) => {
    editUrl.value.userID = val.userID;
    editUrl.value.urlKey = val.urlKey;
    editUrl.value.originalUrl = val.originalUrl;
    editUrl.value.shortenedUrl = val.shortenedUrl;
    editUrl.value.creationTime = new Date(val.creationTime);
    editUrl.value.expirationTime = new Date(val.expirationTime);
  }
});

export const isEditorVisible = ref(false);
export const showEditor = async (urlKey: string) => {
  isEditorVisible.value = true;
  const res = await getUrlbyKey(urlKey).catch((err) => {
    console.log(err);
    throw err;
  });
  editUrl.value = res.data.data;
};

export const delelteUrl = async (urlKey: string) => {
  const confirm = await ElMessageBox.confirm('Are you sure to delete this url?', 'Warning', {
    confirmButtonText: 'OK',
    cancelButtonText: 'Cancel',
    type: 'warning'
  });
  if (confirm !== 'confirm') {
    ElMessage.info('Delete canceled');
    return;
  }
  const { data } = await deleteUrlbyKey(urlKey).catch((err) => {
    console.log(err);
    throw err;
  });
  if (data.isSuccess) {
    ElMessage.success('Delete success');
  } else {
    ElMessage.error('Delete failed');
  }
};

export const addExpirationTime = (days: number) => {
  addUrlExpirationTime(editUrl.value.urlKey, days).then((res) => {
    if (res.data.isSuccess) {
      ElMessage.success('Add expiration time success');
    } else {
      ElMessage.error('Add expiration time failed');
    }
  });
};
