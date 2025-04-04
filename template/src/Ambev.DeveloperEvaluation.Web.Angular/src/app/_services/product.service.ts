import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { Product } from '@app/_models/product';
import { BaseService, StorangeEnum } from './base.service';

@Injectable({ providedIn: 'root' })
export class ProductService extends BaseService<Product> {
    private productSubject: BehaviorSubject<Product | null>;
    public product: Observable<Product | null>;

    constructor(
        private router: Router,
        httpClient: HttpClient,
    ) {
        super(httpClient, 'products');
        this.productSubject = new BehaviorSubject(JSON.parse(localStorage.getItem(StorangeEnum.session.toString())!));
        this.product = this.productSubject.asObservable();
    }

    public get productValue() {
        return this.productSubject.value;
    }
}