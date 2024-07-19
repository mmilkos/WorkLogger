export interface PagedResultModel<T>
{
  dataList: T[];
  pageNumber: number;
  totalRecords: number;
}
