import { ref, computed } from 'vue';
import { defineStore } from 'pinia';

export interface Token {
  accessToken: string;
  refreshToken: string;
}

export const tokenManager = defineStore('mytoken', () => {
  //state
  const tokenJson = ref('');
  // getter
  const token = computed<Token>(() => {
    try {
      return JSON.parse(tokenJson.value || window.localStorage.getItem('TokenInfo') || '{}');
    } catch (error) {
      ElMessage.error('Json string parse failed');
      window.localStorage.setItem('TokenInfo', '');
      throw error;
    }
  });
  //function => actions

  function saveToken(data: string) {
    tokenJson.value = data;
    window.localStorage.setItem('TokenInfo', data);
  }

  function clearToken() {
    tokenJson.value = '';
    window.localStorage.setItem('TokenInfo', '');
  }
  //export
  return { token, saveToken, clearToken };
});
