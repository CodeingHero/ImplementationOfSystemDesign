export interface CommonResponse<T> {
  isSuccess: boolean;
  isError: boolean;
  message: string;
  data: T;
  errors: [];
}
