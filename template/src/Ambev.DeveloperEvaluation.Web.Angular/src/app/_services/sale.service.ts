import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { Sale } from '@app/_models/sale';
import { BaseService, StorangeEnum } from './base.service';

@Injectable({ providedIn: 'root' })
export class SaleService extends BaseService<Sale> {
    private saleSubject: BehaviorSubject<Sale | null>;
    public sale: Observable<Sale | null>;

    constructor(
        private router: Router,
        httpClient: HttpClient,
    ) {
        super(httpClient, 'sales');
        this.saleSubject = new BehaviorSubject(JSON.parse(localStorage.getItem(StorangeEnum.session.toString())!));
        this.sale = this.saleSubject.asObservable();
    }

    public get saleValue() {
        return this.saleSubject.value;
    }

    public override post(sale: Sale, route?: string){
        return super.post(
            {
                branchId: sale.branchId,
                customerId: sale.customerId,
                date: sale.date
            }, route);
    }

    public override put(sale: Sale, route?: string): Observable<Object> {
        return super.put(
            {
                id: sale.id,
                branchId: sale.branchId!,
                customerId: sale.customerId!,
                date: sale.date,
                saleItemCollection: sale.saleItemCollection?.map(x => {
                    return {
                        productId: x.productId,
                        quantity: x.quantity
                    };
                })
            },  route).pipe(map(x => { return x; })
        );
    }

    // add(sale: Sale) {
    //     return this.post(sale)
    // }

    // getAll() {
    //     return this.list()
    // }

    // getById(id: any) {
    //     return this.get(id)
    // }

    // update(id: number, boby: any) {
    //     return this.post(boby)
    //         .pipe(map(x => {
    //             return x;
    //         }));
    // }

    // override delete(id: any){
    //     return super.delete(id);
    // }

}