import request from './request';
import type { CommonResponse } from './commonResponse';
import type { Token } from '@/stores/myToken';

export type UserInfo = {
  userID: number;
  userName: string;
  nickName: string;
  email: string;
  portrait: string;
};

type IdentityResponse = {
  email: string;
  userName: string;
  userID: number;
  token: Token;
};

export type UserUpdateInfo = Omit<UserInfo, 'portrait'> & {
  prevPassword: string;
  newPassword: string;
};

// //used for login response
//used for login request
type LoginInfo = {
  userKey: string;
  password: string;
};

export const login = (loginInfo: LoginInfo) => {
  return request<CommonResponse<IdentityResponse>>({
    method: 'POST',
    url: '/user/login',
    data: {
      userKey: loginInfo.userKey,
      password: loginInfo.password
    }
  });
};

//Get user information
export const getUserInfo = () => {
  return request<CommonResponse<UserInfo>>({
    method: 'GET',
    url: '/user/info'
  });
};

//user exit
export const logout = () => {
  return request<CommonResponse<any>>({
    method: 'POST',
    url: '/user/signOut'
  });
};

//register new user

export const register = (register: LoginInfo) => {
  return request({
    method: 'POST',
    url: '/user/register',
    data: {
      userKey: register.userKey,
      password: register.password
    }
  });
};

//update user information
export const updateUserInfo = (userUpdateInfo: UserUpdateInfo) => {
  return request<CommonResponse<UserInfo>>({
    method: 'PUT',
    url: '/user/updateUser',
    data: {
      userID: userUpdateInfo.userID,
      userName: userUpdateInfo.userName,
      email: userUpdateInfo.email,
      nickName: userUpdateInfo.nickName,
      portrait: '',
      prevPassword: userUpdateInfo.prevPassword,
      newPassword: userUpdateInfo.newPassword
    }
  });
};
