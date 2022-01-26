import React, { useState, useEffect } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Row,
    Col,
    Button,
    Label, Spinner,
    Form, FormGroup,
    Input, InputGroup, InputGroupAddon,
    Table, Fade

} from "reactstrap";
import axios from "axios";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { AccountInfo, NavigationClient } from "@azure/msal-browser";
import { loginRequest } from "AuthConfig";
import { useHistory } from "react-router-dom";
import { InstrumentSearchResult, Search } from "views/Instrument/Instruments";


const wait = (ms: number): Promise<void> => {
    return new Promise(resolve => setTimeout(resolve, ms));
}



export const NewInstrumentView = () => {
    const { instance, accounts } = useMsal();
    const history = useHistory();

    const [searchValue, setSearchValue] = useState("");
    const [searchResults, setSearchResults] = useState<InstrumentSearchResult[]>([]);
    const [hasSearchResults, setHasSearchResults] = useState(false)
    const [isLoading, setIsLoading] = React.useState(true);

    useEffect(() => {
        setIsLoading(true);
        const timeoutId = setTimeout(() => {
            if(searchValue.length > 1){
                const request = {
                    scopes: loginRequest.scopes,
                    account: accounts[0] as AccountInfo
                };
        
                instance.acquireTokenSilent(request).then(async (response) => {
                    const r = await Search(searchValue, response.accessToken);
                    setHasSearchResults(r.length > 0)
                    setSearchResults(r);
                    setIsLoading(false);
                }).catch((e) => {
                    console.log(e);
                });
            }

        }, 150);
        
        return () => clearTimeout(timeoutId);
    }, [searchValue]);

    const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSearchValue(e.target.value);
    };

    return (
                <>

                <Card>
                    <CardHeader>
                        <CardTitle tag="h4">Create new instrument</CardTitle>
                    </CardHeader>
                    <CardBody>
                        <Form>
                            <FormGroup>
                                <Label for="exampsearchleDate">Search by ticker</Label>
                                <InputGroup>
                                    <Input type="search" name="search" id="search" placeholder="ticker" onChange={onChange} />
                                    <InputGroupAddon addonType="append"><span className="input-group-text"><i className="fa fa-search" /></span></InputGroupAddon>
                                </InputGroup>
                            </FormGroup>
                        </Form>
                    </CardBody>
                </Card>
                <Fade timeout={150} in={hasSearchResults}>
                <Card>
                        <CardBody>
                        {
                                isLoading ? <p className="text-center"><Spinner color="primary" /></p> :
                        <Table striped>
                                        <thead className="text-primary">
                                            <tr>
                                                <th>Name</th>
                                                <th>Ticker</th>
                                                <th>Exchange</th>
                                                <th>Type</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {searchResults.map((instrument) => (
                                                <tr key={instrument.symbol}>
                                                    <td>{instrument.name}</td>
                                                    <td>{instrument.symbol}</td>
                                                    <td>{instrument.exchange}</td>
                                                    <td>{instrument.type}</td>
                                                    <td>
                                                        <Button className="btn btn-icon btn-round" color="primary" type="button" onClick={() => {}}>
                                                        <i className="fa fa-arrow-right"></i>
                                                        </Button>
                                                    </td>
                                                        
                                                </tr>
                                            ))}
                                        </tbody>
                                    </Table>
                            }
                        </CardBody>
                    </Card>
                    </Fade>
                    </>
    );
}


export default NewInstrumentView