import axios from "axios";
import { createPortal } from "react-dom";

export interface AccountListItem {
    id: string
    name: string
    currency: string
}

export const dummyAccounts: AccountListItem[] = [
    { id :"1", name: "mock account 1", currency: "EUR" },
    { id : "2", name: "mock account 2", currency: "USD" }
]

export const AccountsList = async (token: string): Promise<AccountListItem[]> => {
    let url = "/api/accounts";

    const config = {
        headers: { Authorization: `Bearer ${token}` },
        mode: "no-cors",
    };

    return (await axios.get(url, config)).data;
}

export const GetSelected = async (): Promise<AccountListItem> => {
    const a = localStorage.getItem("account") || "";
    return JSON.parse(a);
}

export const SetSelected = async (account: AccountListItem) => {
    localStorage.setItem("account", JSON.stringify(account));
}

export const DeleteAccount = async (token: string, id : string) => {
    let url = "/api/accounts/" + id;

    const config = {
        headers: { Authorization: `Bearer ${token}` },
        mode: "no-cors",
    };

    await (await axios.delete(url, config));
}

export interface AccountSummary {
    year: number
    amount: number
}

export const Summary = async (accountId: string, token: string): Promise<AccountSummary[]> => {
    let url = "/api/accounts/deposits/summary/year/"+ accountId;

    const config = {
        headers: { Authorization: `Bearer ${token}` },
        mode: "no-cors",
    };

    return await (await axios.get(url, config)).data    
}

const wait = (ms: number): Promise<void> => {
    return new Promise(resolve => setTimeout(resolve, ms));
}



