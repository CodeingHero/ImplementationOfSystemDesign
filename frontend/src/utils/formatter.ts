import dayjs from 'dayjs';
import customParseFormat from 'dayjs/plugin/customParseFormat';
export const dateToTableFormat = (date: Date, column: any, value: string) => {
  return dayjs(value).format('YYYY-MM-DD HH:mm:ss');
};
export const dateStringFormat = (value: string) => {
  return dayjs(value).format('YYYY-MM-DD HH:mm:ss');
};

export const dateFormat = (date: Date) => {
  return dayjs(date).format('YYYY-MM-DD HH:mm:ss');
};
