export interface AccountListItem{
    name: string
    currency: string
}

export const dummyAccounts: AccountListItem[] = [
    {name: "mock account 1", currency: "EUR"},
    {name: "mock account 2", currency: "USD"}
]

export const AccountsList = async () : Promise<AccountListItem[]> => {
    //https://www.josephguadagno.net/2020/10/24/working-with-microsoft-identity-react-native-client
    //https://thedutchlab.com/blog/using-axios-interceptors-for-refreshing-your-api-token
    await wait(500);
    return dummyAccounts;
}

const wait = (ms: number):Promise<void> => {
    return new Promise(resolve => setTimeout(resolve, ms));
}



