import request from './request';
import type { CommonResponse } from './commonResponse';

export interface Introduction {
  title: string;
  text: string;
}

export const getIntroduction = () => {
  return request<CommonResponse<Introduction>>({
    method: 'GET',
    url: 'sd/index/introduction'
  });
};
