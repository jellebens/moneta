import axios from "axios";

export interface InstrumentListItem {
    id: string
    name: string
    symbol: string
    currency: string

}

export interface InstrumentSearchResult {
    exchange: string
    symbol: string
    type : string
    name: string
}

export interface InstrumentDetailResult {
    exchange: string
    symbol: string
    type : string
    name: string
    currency: string
}

export interface NewInstrument {
    id: string
    exchange: string
    symbol: string
    type : string
    name: string
    isin: string
    sector: number
    currency: string
}

export const dummyAccounts: InstrumentListItem[] = [
    { id :"1", name: "***DUMMY*** SPDR MSCI World UCITS ETF", symbol: "SWRD", currency: "EUR" },
    { id :"2", name: "***DUMMY*** iShares Nasdaq 100 UCITS ETF (Acc)", symbol: "CNX1", currency: "EUR" },
]


export const dummySearchResults: InstrumentSearchResult[] = [
    { "exchange": "LSE", "symbol": "SWRD.L" , "type": "EFT", "name": "***DUMMY*** SSGA SPDR ETFS Europe I Public Limited Company - SPDR MSCI World UCITS ETF" },
    { "exchange": "AMS", "symbol": "SWRD.AS", "type": "EFT", "name": "***DUMMY***SSGA SPDR ETFS Europe I Public Limited Company - SPDR MSCI World UCITS ETF"},
    { "exchange": "EBS", "symbol": "SWRD.SW", "type": "EFT", "name": "***DUMMY***SPDR MSCI World UCITS ETF" },
    { "exchange": "MIL", "symbol": "SWRD.MI", "type": "EFT", "name": "***DUMMY***SSGA SPDR ETFS Europe I Public Limited Company - SPDR MSCI World UCITS ETF"}
]

export const dummyDetail: InstrumentDetailResult = 
    { "exchange": "AMS",    "symbol": "SWRD.AS", "type": "ETF", "name": "***DUMMY*** SSGA SPDR ETFS Europe I Public Limited Company - SPDR MSCI World UCITS ETF", "currency": "USD" }

export const InstrumentList = async (token: string): Promise<InstrumentListItem[]> => {
    let url = "/api/instruments";

    const config = {
        headers: { Authorization: `Bearer ${token}` },
        mode: "no-cors",
    };

    return await (await axios.get(url, config)).data;
}

export const DeleteInstrument = async (token: string, id : string) => {
    let url = "/api/instruments/" + id;

    const config = {
        headers: { Authorization: `Bearer ${token}` },
        mode: "no-cors",
    };

    await (await axios.delete(url, config));
}


export const Search = async (q: string, token: string): Promise<InstrumentSearchResult[]> => {
    let url = "/api/instruments/autocomplete/"+ q;

    const config = {
        headers: { Authorization: `Bearer ${token}` },
        mode: "no-cors",
    };
    var results = dummySearchResults;
    await axios.get(url, config)
                    .then(response => {results = response.data})
                    .catch(error => {
                        console.log("Error when retrieving results " + error)
                    });
    return results;
}

export const Detail = async (symbol: string, token: string): Promise<InstrumentDetailResult> => {
    let url = "/api/instruments/"+ symbol;

    const config = {
        headers: { Authorization: `Bearer ${token}` },
        mode: "no-cors",
    };

    var results = dummyDetail;
    await axios.get(url, config)
                    .then(response => {results = response.data})
                    .catch(error => {
                        console.log("Error when retrieving results " + error)
                    });
    return results;
}

const wait = (ms: number): Promise<void> => {
    return new Promise(resolve => setTimeout(resolve, ms));
}



