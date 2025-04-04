import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { first, map } from 'rxjs/operators';
import { AlertService } from '@app/_services';
import { SaleService } from '@app/_services/sale.service';
import { Sale } from '@app/_models/sale';
import { ProductService } from '@app/_services/product.service';
import { ApiResponseWithData } from '@app/_models/api-response-with-data';
import { Product } from '@app/_models/product';
import { SaleItem } from '@app/_models/monthCdb';
import { Branch } from '@app/_models/branch';
import { Customer } from '@app/_models/customer';
import { BranchService } from '@app/_services/branch.service';
import { CustomerService } from '@app/_services/customer.service';

@Component({ templateUrl: 'add-edit.component.html' })
export class AddEditComponent implements OnInit {
    formSale!: FormGroup;
    title!: string;
    loading = false;
    submitting = false;
    submitted = false;
    isAddAnexo = false;
    changedItemSaleCollection = false;
    sale!: Sale;
    file?: File = undefined;

    productCollection!: Product[]
    branchCollection!: Branch[]
    customerCollection!: Customer[]
    
    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private saleService: SaleService,
        private productService: ProductService,
        private branchService: BranchService,
        private customerService: CustomerService,
        private alertService: AlertService
    ) { }

    ngOnInit(){
        this.DataBind();
    }

    editMode(){
        if (this.sale?.id) {
            return true;
        }

        let id = this.route.snapshot.params['id'];
        if (id) {
            this.sale = new Sale(id);
            return true;
        }

        return false;
    }

    DataBind() {
        this.changedItemSaleCollection = false;
        
        this.LoadProductCollection();
        this.LoadBranchCollection();
        this.LoadCustomerCollection();
        
        if (this.editMode()) 
        {
            this.formSale = this.formBuilder.group({
                id: ['', Validators.required],
                branchId: ['', Validators.required],
                customerId: ['', Validators.required],
                date: ['', Validators.required],
                discount: ['', Validators.required],
                percentageDiscount: ['', Validators.required],
                grossTotal: ['', Validators.required],
                total: ['', Validators.required]                
            });
            
            this.title = 'Sale';   
            this.loading = true;
            this.saleService
                .get(this.sale.id)
                .pipe(first())
                .subscribe({
                    next: (x: any) => {
                        this.sale = x.data;
                        
                        this.title = `Sale number: ${this.sale?.number}`; 

                        this.formSale.patchValue({
                            id: this.sale.id,
                            branchId: this.sale.branchId,
                            customerId: this.sale.customerId,
                            date: new Date(this.sale!.date!).toISOString().split('T')[0],
                            discount: this.sale.discount,
                            percentageDiscount: this.sale.percentageDiscount,   
                            grossTotal: this.sale.grossTotal,
                            total: this.sale.total
                        });
                        this.loading = false;
                    },
                    error: error => {
                        this.alertService.error(error);
                        this.loading = false;
                    }
                });
        }
        else
        {
            this.title = 'Create a new Sale';
            
            this.sale = new Sale();

            this.formSale = this.formBuilder.group({
                id: [''],
                branchId: ['', Validators.required],
                customerId: ['', Validators.required],
                date: ['', Validators.required],
            });            
        }
    }

    LoadProductCollection() {
        this.productService.list()
        .pipe(first())
        .subscribe({
            next: (data) => {
                this.loading = false;
                this.productCollection = data.data;
                return data;
            },
            error: error => {
                this.loading = false;
                this.alertService.error(error);
            }
        });
    }

    LoadBranchCollection() {
        this.branchService.list()
        .pipe(first())
        .subscribe({
            next: (data) => {
                this.loading = false;
                this.branchCollection = data.data;
                return data;
            },
            error: error => {
                this.loading = false;
                this.alertService.error(error);
            }
        });
    }
    
    LoadCustomerCollection() {
        this.customerService.list()
        .pipe(first())
        .subscribe({
            next: (data) => {
                this.loading = false;
                this.customerCollection = data.data;
                return data;
            },
            error: error => {
                this.loading = false;
                this.alertService.error(error);
            }
        });
    }

    get f() { return this.formSale.controls; }

    onSubmit() {
        if (this.formSale.invalid) {
            return;
        }

        this.submitted = true;
    }

    saveSale() {
        if (this.formSale.invalid) {
            return;
        }

        this.submitted = true;
        this.submitting = true;
        this.alertService.clear();

        const saleData = {
            ...this.formSale.value,
            saleItemCollection: this.sale?.saleItemCollection || []
        };        

        let editMode = this.editMode();
        return (editMode
            ? this.saleService.put(saleData)
            : this.saleService.post(saleData))
            .pipe(first())
            .subscribe({
                next: (response: any) => {
                    this.submitting = false;
                    this.sale = response.data
                    this.alertService.success(response.message);
                    // this.DataBind();
                    // this.router.navigate([`../${this.sale.id}`], { relativeTo: this.route });
                    // window.location.reload()
                    if (editMode)
                        this.DataBind();
                    else
                        this.router.navigate([`../${this.sale.id}`], { relativeTo: this.route });
                },
                error: error => {
                    this.alertService.error(error);
                    this.submitting = false;
                }
            })
    }

    setIsAddAnexo() {
        this.isAddAnexo = !this.isAddAnexo;
        this.file = undefined;
    }

    onFileSelected(event: any) {
        if (event.target.files.length == 0){
            this.file = undefined;
        }
        else{
            this.file = event.target.files[0];

            let value = this.formSale.value;

            this.formSale.setValue(value);
        }
    }    

    back() {
        this.router.navigate(['../sales/list'], { relativeTo: this.route });
    }

    monthsArray()
    {
        var array = [];

        for (let index = 1; index <= 60; index++) {
            array.push(index);
        }

        return array;
    }

    getQuantity(product: Product){
        return this.sale?.saleItemCollection?.find(i => i.productId == product.id)?.quantity || 0
    }

    getPrice(product: Product){
        let unitPrice = this.sale?.saleItemCollection?.find(i => i.productId == product.id)?.unitPrice;

        if (unitPrice)
            return unitPrice;

        unitPrice = this.productCollection?.find(i => i.id == product.id)?.unitPrice;

        if (unitPrice)
            return unitPrice;

        return 0
    }

    getTotalQuantities(): number {
        return this.productCollection?.reduce((total, product) => total + this.getQuantity(product), 0);
    }

    getTotal(): number {
        return this.productCollection?.reduce((total, product) => total + this.getQuantity(product) * this.getPrice(product), 0);
    }

    upItem(product: Product){
        this.setQuantityItemSale(product, 1);
    }

    downItem(product: Product){
        this.setQuantityItemSale(product, -1);
    }

    setQuantityItemSale(product: Product, quantity: number){
        this.changedItemSaleCollection = true
        this.sale.saleItemCollection = this.sale.saleItemCollection || [];

        let item = this.sale.saleItemCollection?.find(i => i.productId == product.id)

        if (!item){
            item = new SaleItem(
                product.id,
                this.sale.id,
                0
            );

            this.sale.saleItemCollection?.push(item);
        }

        item.quantity = item.quantity! + quantity;

        this.sale.saleItemCollection = this.sale.saleItemCollection?.filter(i => i.quantity! > 0)!
    }
}