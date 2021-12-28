import axios from "axios";

export interface AccountListItem{
    name: string
    currency: string
}

export const dummyAccounts: AccountListItem[] = [
    {name: "mock account 1", currency: "EUR"},
    {name: "mock account 2", currency: "USD"}
]

export const AccountsList = async () : Promise<AccountListItem[]> => {
    
    

    await wait(500);
    return dummyAccounts;
}

const wait = (ms: number):Promise<void> => {
    return new Promise(resolve => setTimeout(resolve, ms));
}



