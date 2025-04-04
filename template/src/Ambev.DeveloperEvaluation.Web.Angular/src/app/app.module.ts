import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

// used to create fake backend
import { HttpMockInterceptor } from './_helpers';

import { AppRoutingModule } from './app-routing.module';
import { JwtInterceptor, ErrorInterceptor } from './_helpers';
import { AppComponent } from './app.component';
import { AlertComponent } from './_components/alert';
import { HomeComponent } from './home';
import { RefsInterceptor } from 'angular-json-refs-interceptor';
// import { PaginationComponent } from './_components/pagination/pagination.component';

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        HttpClientModule,
        AppRoutingModule
    ],
    declarations: [
        AppComponent,
        AlertComponent,
        HomeComponent,
        // PaginationComponent
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: RefsInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: HttpMockInterceptor, multi: true },

        // provider used to create fake backend
        HttpMockInterceptor
    ],
    bootstrap: [AppComponent]
})
export class AppModule { };