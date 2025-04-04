import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { AccountService, AlertService } from '@app/_services';
import { StorangeEnum } from '@app/_services/base.service';

@Component({ templateUrl: 'login.component.html' })
export class LoginComponent implements OnInit {
    form!: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private alertService: AlertService
    ) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        this.accountService.login(this.f.username.value, this.f.password.value)
            .pipe(first())
            .subscribe({
                
                next: (user) => {
                    localStorage.setItem(StorangeEnum.session.toString(), JSON.stringify(user));
                    // return user;
                    
                    this.loading = false;
                    // const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '**';
                    // this.router.navigateByUrl(returnUrl);
                    window.location.replace('/');
                },
                error: error => {
                    this.loading = false;
                    this.alertService.error(error);
                }   

                // error: error => {
                //     this.alertService.error('Invalid email or password');
                //     this.loading = false;
                // }
            });
    }
}