import axios, { type AxiosRequestHeaders } from 'axios';
import { tokenManager } from '@/stores/myToken';
import router from '@/router/index';
const request = axios.create({});
//const router = useRouter();
// Register a request interceptor to add a token to all request headers
request.interceptors.request.use((config) => {
  // check if the request config object has a headers property, if not, create an empty headers object
  if (!config.headers) {
    config.headers = {} as AxiosRequestHeaders;
  }
  // get a store object that stores the access token
  const store = tokenManager();
  // Set the access_token stored in the store as the Authorization request header to verify the user's identity
  config.headers.Authorization = 'Bearer ' + store.token?.accessToken;
  // Return the processed config object to be sent to the server
  return config;
});

//Register a response interceptor to handle the response
request.interceptors.response.use(
  (res) => res,
  async (err) => {
    if (err.response.status === 401) {
      // Push route to login page if the response status is 401
      router.push({ name: 'login' });
      return;
    }
    return Promise.reject(err);
  }
);
// request.interceptors.response.use(
//   (response) => {
//     const router = useRouter();
//     // Push route to login page if the response status is 401
//     if (response.status === 401) {
//       router.push({ name: 'Login' });
//     }
//     return response;
//   },
//   (error) => {
//     // Do something with response error
//     return Promise.reject(error);
//   }
// );

export default request;
