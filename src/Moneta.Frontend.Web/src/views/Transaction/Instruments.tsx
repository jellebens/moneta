export interface InstrumentListItem {
    id: string
    name: string
    currency: string
    symbol: string
}

export const dummyInstruments: InstrumentListItem[] = [
    { id :"1", name: "***DUMMY*** SPDR MSCI World UCITS ETF", symbol: "SWRD.AS", currency: "EUR" },
    { id :"2", name: "***DUMMY*** SPDR MSCI World UCITS ETF II",symbol: "SWRD.BR", currency: "EUR" },
    { id :"3", name: "***DUMMY*** iShares Nasdaq 100 UCITS ETF (Acc)",symbol: "CNX1", currency: "EUR" },
]