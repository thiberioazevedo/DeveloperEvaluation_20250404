import { Guid } from "guid-typescript";

export class Branch {
    constructor(
        public id?: Guid,
        public name?: string) 
	{
    }
}
