import { Guid } from "guid-typescript";

export class SaleItem {
    constructor(
        public productId?: Guid,
        public saleId?: Guid,
        public quantity?: number,
        public unitPrice?: number) {
    }
}
