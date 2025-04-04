export class User {
    id?: string;
    username?: string;
    password?: string;
    confirmPassword?: string;
    phone?: string;
    email?: string;
    token?: string;;
    status?: UserStatus;
    role?: UserRole;
}

export enum UserRole{
    None,
    Customer,    
    Manager,
    Admin,
}

export enum UserStatus
{
    Unknown = 0,
    Active,
    Inactive,
    Suspended
}

