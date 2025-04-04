import { PaginatedList } from "./paginated-list";

export class ApiResponseWithData<T>  {
    public data!: T[];
    public success!: boolean;
    public message!: string;
    public errors!: []
}

export class ApiResponsePaginatedListWithData<T> implements IApiResponsePaginatedListWithData {
    public data!: PaginatedList<T>;
    public success!: boolean;
    public message!: string;
    public errors!: []
}

export interface IApiResponsePaginatedListWithData{
    data: IPaginatedList;
}

export interface IPaginatedList{
    totalPages?: number
    totalCount?: number
    hasPrevious?: boolean
    hasNext?: boolean
    pageNumber?: number
    pageSize?: number
    collection?: any[]
}
