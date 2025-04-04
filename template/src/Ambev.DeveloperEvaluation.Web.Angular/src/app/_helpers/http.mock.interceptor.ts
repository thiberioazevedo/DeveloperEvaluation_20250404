import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import * as authResponse from './responsesFake/auth.response.json'
import * as createSaleResponse from './responsesFake/create-sale.response.json'
import * as deleteSaleResponse from './responsesFake/delete-sale.response.json'
import * as getSaleResponse from './responsesFake/get-sale.response.json'
import * as listBranchsResponse from './responsesFake/list-branchs.response.json'
import * as listCustomersResponse from './responsesFake/list-customers.response.json'
import * as listProductsResponse from './responsesFake/list-products.response.json'
import * as listSalesResponse from './responsesFake/list-sales.response.json'

const httpMockInterceptorEnabled = false;
const urls = [
    {
        url: 'api/auth',
        verb: 'post',
        json: authResponse
    },
    {
        url: 'api/branchs',
        verb: 'get',
        json: listBranchsResponse
    },
    {
        url: 'api/customers',
        verb: 'get',
        json: listCustomersResponse
    },
    {
        url: 'api/products',
        verb: 'get',
        json: listProductsResponse
    }, 
    {
        url: 'api/sales',
        verb: 'put',
        json: createSaleResponse
    },
    {
        url: 'api/sales/',
        verb: 'get',
        json: getSaleResponse
    },
    {
        url: 'api/sales',
        verb: 'get',
        json: listSalesResponse
    },
    {
        url: 'api/sales?',
        verb: 'get',
        json: listSalesResponse
    },    
    {
        url: 'api/sales/',
        verb: 'delete',
        json: deleteSaleResponse
    },
    {
        url: 'api/sales',
        verb: 'post',
        json: createSaleResponse
    }
];

@Injectable()
export class HttpMockInterceptor implements HttpInterceptor {
    constructor(private injector: Injector) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        for (const element of urls) {
            if (httpMockInterceptorEnabled &&request.method.toLowerCase() == element.verb.toLowerCase() && this.compare(this.getPathRelative(request.url), element.url))
            {
                console.log('Json loaded from HttpMockInterceptor : ' + request.url);
                return of(new HttpResponse({ status: 200, body: ((element.json) as any).default }));
            }
        }

        console.log('Loaded from http call :' + request.url);

        return next.handle(request);
    }

    getPathRelative(url: string){
        return url.split('/').map(i => i).filter((_, index) => index > 2).join('/').toLowerCase();
    }

    getPathBase(url: string){
        return url.split('/').map(i => i).filter((_, index) => index <= 2).join('/').toLowerCase();
    }

    compare(url: string, url2: string)  {
        if (url2.endsWith('?') || url2.endsWith('/')){
            return url.toLowerCase().startsWith(url2.toLowerCase());
        } else{
            return url.toLowerCase() == url2.toLowerCase();
        }
    }
}
