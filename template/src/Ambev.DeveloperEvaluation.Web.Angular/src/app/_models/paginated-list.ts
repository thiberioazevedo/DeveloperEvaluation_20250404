export class PaginatedList<T> {
    public totalPages?: number
    public totalCount?: number
    public hasPrevious?: boolean
    public hasNext?: boolean
    public pageNumber?: number
    public pageSize?: number
    public collection?: T[]
}
