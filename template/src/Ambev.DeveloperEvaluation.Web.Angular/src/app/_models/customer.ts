import { Guid } from "guid-typescript";

export class Customer {
    constructor(
        public id?: Guid,
        public name?: string) 
	{
    }
}
