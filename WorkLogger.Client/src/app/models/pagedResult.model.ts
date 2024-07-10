export interface PagedResultModel<T>
{
  data: T[];
  pageNumber: number;
  totalRecords: number;
}
