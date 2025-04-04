import { Guid } from "guid-typescript";
import { SaleItem as SaleItem } from "./monthCdb";
import { Branch } from "./branch";
import { Customer } from "./customer";

export class Sale {
    constructor(
        public id?: Guid,
        public number?: number,
        public date?: Date,
        public branchId?: Guid,
        public customerId?: Guid,
        public cancelled?: boolean,
        public discount?: number,
        public percentageDiscount?: number,
        public grossTotal?: number,
        public total?: number,
        public branch?: Branch,
        public customer?: Customer,
        public saleItemCollection?: SaleItem[]) {
    }
}
