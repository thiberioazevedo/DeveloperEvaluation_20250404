import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { AccountService, AlertService } from '@app/_services';
import { User, UserRole, UserStatus } from '@app/_models';

@Component({ templateUrl: 'register.component.html' })
export class RegisterComponent implements OnInit {
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
            email: ['', Validators.required],
            phone: ['', Validators.required],
            password: ['', [Validators.required, Validators.minLength(6)]],
            confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
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

        
        const value = {
            ...this.form.value,
            status: UserStatus.Active,//UserRole ,
            role: UserRole.Customer      
        };


        this.accountService.register(value)
            .pipe(first())
            .subscribe({
                next: () => {
                    this.alertService.success('Registration successful');
                    this.router.navigate(['../login'], { relativeTo: this.route });
                },
                error: error => {
                    this.loading = false;
                    this.alertService.error(error);
                }
            });
    }

    enableSubmit(){
        if (this.loading)
            return false;

        if (!this.form.controls['username'].value)
            return false;

        if (!this.form.controls['email'].value)
            return false;

        if (!this.form.controls['phone'].value)
            return false;

        if (!this.form.controls['password'].value)
            return false;

        if (!this.form.controls['confirmPassword'].value)
            return false; 
        
        return this.form.controls['password'].value == this.form.controls['confirmPassword'].value
    }
}
