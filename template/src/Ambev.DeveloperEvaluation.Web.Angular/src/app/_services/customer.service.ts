import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { Customer } from '@app/_models/customer';
import { BaseService, StorangeEnum } from './base.service';

@Injectable({ providedIn: 'root' })
export class CustomerService extends BaseService<Customer> {
    private customerSubject: BehaviorSubject<Customer | null>;
    public customer: Observable<Customer | null>;

    constructor(
        private router: Router,
        httpClient: HttpClient,
    ) {
        super(httpClient, 'customers');
        this.customerSubject = new BehaviorSubject(JSON.parse(localStorage.getItem(StorangeEnum.session.toString())!));
        this.customer = this.customerSubject.asObservable();
    }

    public get customerValue() {
        return this.customerSubject.value;
    }
}