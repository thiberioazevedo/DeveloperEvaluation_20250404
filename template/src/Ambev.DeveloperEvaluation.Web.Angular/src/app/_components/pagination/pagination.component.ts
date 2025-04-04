import { Component, Input, Output, EventEmitter } from '@angular/core';
import { IApiResponsePaginatedListWithData } from '@app/_models/api-response-with-data';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent {
  @Input() apiResponseWithData!: IApiResponsePaginatedListWithData;
  @Output() pageChange: EventEmitter<number> = new EventEmitter<number>();

  get totalPages(): number {
    // return Math.ceil(this.apiResponseWithData.data.totalCount! / this.apiResponseWithData.data.pageSize!);
    return this.apiResponseWithData?.data?.totalPages || 0;
  }

  previousPage(): void {
    if (this.apiResponseWithData.data.pageNumber! > 1) {
      this.apiResponseWithData.data.pageNumber!--;
      this.pageChange.emit(this.apiResponseWithData.data.pageNumber!);
    }
  }

  nextPage(): void {
    if (this.apiResponseWithData.data.pageNumber! < this.totalPages) {
      this.apiResponseWithData.data.pageNumber!++;
      this.pageChange.emit(this.apiResponseWithData.data.pageNumber!);
    }
  }
}