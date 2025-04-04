import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { Branch } from '@app/_models/branch';
import { BaseService, StorangeEnum } from './base.service';

@Injectable({ providedIn: 'root' })
export class BranchService extends BaseService<Branch> {
    private branchSubject: BehaviorSubject<Branch | null>;
    public branch: Observable<Branch | null>;

    constructor(
        private router: Router,
        httpClient: HttpClient,
    ) {
        super(httpClient, 'branchs');
        this.branchSubject = new BehaviorSubject(JSON.parse(localStorage.getItem(StorangeEnum.session.toString())!));
        this.branch = this.branchSubject.asObservable();
    }

    public get branchValue() {
        return this.branchSubject.value;
    }
}