import type { CommonResponse } from './commonResponse';
import request from './request';

export type ShortenURL = {
  userID: string;
  urlKey: string;
  originalUrl: string;
  shortenedUrl: string;
  creationTime: Date;
  expirationTime: Date;
};

export const createUrl = (url: string) => {
  return request<CommonResponse<ShortenURL>>({
    method: 'POST',
    url: '/sd/ShortenUrl/CreateUrl',
    params: {
      url: url
    }
  });
};

export const getUrlbyKey = (urlKey: string) => {
  return request<CommonResponse<ShortenURL>>({
    method: 'GET',
    url: '/sd/ShortenUrl/GetUrlByKey',
    params: {
      urlKey: urlKey
    }
  });
};

export const getUrlsByUserID = () => {
  return request<CommonResponse<ShortenURL[]>>({
    method: 'GET',
    url: '/sd/ShortenUrl/GetUrlsByUserID'
  });
};

export const deleteUrlbyKey = (urlKey: string) => {
  return request<CommonResponse<boolean>>({
    method: 'DELETE',
    url: '/sd/ShortenUrl/DeleteUrlByKey',
    params: {
      urlKey: urlKey
    }
  });
};

export const addUrlExpirationTime = (urlKey: string, days: number) => {
  return request<CommonResponse<boolean>>({
    method: 'PUT',
    url: '/sd/ShortenUrl/AddUrlExpirationTime',
    params: {
      urlKey: urlKey,
      days: days
    }
  });
};
