import axios from "axios";

export interface AccountListItem {
    name: string
    currency: string
}

export const dummyAccounts: AccountListItem[] = [
    { name: "mock account 1", currency: "EUR" },
    { name: "mock account 2", currency: "USD" }
]

export const AccountsList = async (token: string): Promise<AccountListItem[]> => {
    let url = "/api/accounts";

    const config = {
        headers: { Authorization: `Bearer ${token}` },
        mode: "no-cors",
    };

    return await (await axios.get(url, config)).data;
}

const wait = (ms: number): Promise<void> => {
    return new Promise(resolve => setTimeout(resolve, ms));
}



