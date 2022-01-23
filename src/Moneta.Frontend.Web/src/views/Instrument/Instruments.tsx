import axios from "axios";

export interface InstrumentListItem {
    id: string
    name: string
    isin : string
    ticker: string
    currency: string

}

export const dummyAccounts: InstrumentListItem[] = [
    { id :"1", name: "SPDR MSCI World UCITS ETF",isin: "IE00BFY0GT14", ticker: "SWRD", currency: "EUR" },
    { id :"2", name: "iShares Nasdaq 100 UCITS ETF (Acc)",isin: "IE00B53SZB19", ticker: "CNX1", currency: "EUR" },
]

export const InstrumentList = async (token: string): Promise<InstrumentListItem[]> => {
    // let url = "/api/instruments";

    // const config = {
    //     headers: { Authorization: `Bearer ${token}` },
    //     mode: "no-cors",
    // };

    // return await (await axios.get(url, config)).data;
    await wait(150)
    return dummyAccounts;
}

export const DeleteInstrument = async (token: string, id : string) => {
    let url = "/api/instruments/" + id;

    const config = {
        headers: { Authorization: `Bearer ${token}` },
        mode: "no-cors",
    };

    await (await axios.delete(url, config));
}

const wait = (ms: number): Promise<void> => {
    return new Promise(resolve => setTimeout(resolve, ms));
}



