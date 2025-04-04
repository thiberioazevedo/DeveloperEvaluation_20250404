import { Guid } from "guid-typescript";

export class Product {
    constructor(
        public id?: Guid,
        public name?: string,
        public unitPrice?: number)
	{
    }
}
