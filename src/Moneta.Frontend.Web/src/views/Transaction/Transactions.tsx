import axios from "axios";

export interface InstrumentListItem {
    id: string
    name: string
    currency: string
    symbol: string
}

export interface AccountSummary {
    year: number
    amount: number
}

export const dummyInstruments: InstrumentListItem[] = [
    { id :"1", name: "***DUMMY*** SPDR MSCI World UCITS ETF", symbol: "SWRD.AS", currency: "EUR" },
    { id :"2", name: "***DUMMY*** SPDR MSCI World UCITS ETF II",symbol: "SWRD.BR", currency: "EUR" },
    { id :"3", name: "***DUMMY*** iShares Nasdaq 100 UCITS ETF (Acc)",symbol: "CNX1", currency: "EUR" },
]




export const Summary = async (accountId: string, token: string): Promise<AccountSummary[]> => {
    let url = "/api/accounts/deposits/summary/year/{id}"+ accountId;

    const config = {
        headers: { Authorization: `Bearer ${token}` },
        mode: "no-cors",
    };

    return await (await axios.get(url, config)).data    
}