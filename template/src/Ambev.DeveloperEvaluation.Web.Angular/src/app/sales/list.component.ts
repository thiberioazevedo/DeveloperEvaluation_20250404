import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { AlertService } from '@app/_services';
import { SaleService as SaleService } from '@app/_services/sale.service';
import { ApiResponsePaginatedListWithData } from '@app/_models/api-response-with-data';
import { Sale } from '@app/_models/sale';
import { ProductService } from '@app/_services/product.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({ templateUrl: 'list.component.html' })
export class ListComponent implements OnInit {
    loading: boolean = false;
    apiResponsePaginatedListWithData!: ApiResponsePaginatedListWithData<Sale>;
    
    constructor(private saleService: SaleService, private alertService: AlertService, private route: ActivatedRoute, private router: Router) {}

    ngOnInit() {
        this.dataBind();        
    }

    dataBind() {
        this.loading = true;

        this.saleService.listPagination(this.apiResponsePaginatedListWithData?.data.pageNumber || 1, this.apiResponsePaginatedListWithData?.data.pageSize || 10, '', '', true)
            .pipe(first())
            .subscribe({
                next: (apiResponseWithData) => {
                    this.loading = false;
                    this.apiResponsePaginatedListWithData = apiResponseWithData;
                    return apiResponseWithData;
                },
                error: error => {
                    this.loading = false;
                    this.alertService.error(error);
                }
            });
    }

    delete(sale: Sale) {
        this.loading = true;
        this.saleService.delete(sale.id)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Operation carried out successfully');
                    this.dataBind();
                    this.loading = false;
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });             
    }

    cancel(sale : Sale) {
        this.loading = true;
        this.saleService.cancel(sale.id)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Operation carried out successfully');
                    this.dataBind();
                    this.loading = false;
                },
                error: error => {
                    this.alertService.error(error);
                    this.loading = false;
                }
            });             
    }
    
    edit(sale: Sale) {
        this.router.navigate([`../${sale.id}`], { relativeTo: this.route })
    }

    onPageChange(event: any){
        this.dataBind();
    }
}