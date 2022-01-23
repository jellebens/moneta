import React, { useState } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Row,
    Col,
    Button,
    Form, FormGroup,
    Input,

} from "reactstrap";
import axios from "axios";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { AccountInfo, NavigationClient } from "@azure/msal-browser";
import { loginRequest } from "AuthConfig";
import { useHistory } from "react-router-dom";

const wait = (ms: number): Promise<void> => {
    return new Promise(resolve => setTimeout(resolve, ms));
}

export const NewInstrumentView = () => {
    const { instance, accounts } = useMsal();
    const [instrumentName, setInstrumentName] = useState("");
    const [type, setType] = useState("");
    const [isin, setIsin] = useState("");
    const [ticker, setTicker] = useState("");
    const [currency, setCurrency] = useState("EUR");
    const [url, setUrl] = useState("EUR");
    const [IsSubmitted, setIsSubmitted] = React.useState(false);
    const history = useHistory();


    const validateForm = () : boolean => {
        return instrumentName !== "" && isin !== "" && ticker !== "" && type !== "";
    }

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
        e.preventDefault();

        if (validateForm()) {
            await submitForm();
        }
    };

    const cancelForm = async () => {
        history.push("/instruments");
    }

    const submitForm = async () => {
        

        const request = {
            scopes: loginRequest.scopes,
            account: accounts[0] as AccountInfo
        };

        instance.acquireTokenSilent(request).then(async (response) => {
            
            setIsSubmitted(true);

            let url = "/api/accounts";

            const config = {
                headers: { Authorization: `Bearer ${response.accessToken}` },
                mode: "no-cors",
            };

            await (await axios.post(url,{
                "Name": instrumentName,
                "Type": type,
                "Isin": isin,
                "Ticker": ticker,
                "Currency": currency,
                "Url": url
            } ,config));
            
            await wait(300);

            history.push("/instruments")
        }).catch((e) => {
            console.log(e);
        });

    }


    return (
        <Row>
            <Col md="12">
                <Card>
                    <CardHeader>
                        <CardTitle tag="h4">Create new instrument</CardTitle>
                    </CardHeader>
                    <CardBody>
                        <Form onSubmit={handleSubmit} noValidate={true}>
                            <Row>
                                <Col md="12">
                                    <FormGroup>
                                        <label>Name</label>
                                        <Input type="text" value={instrumentName} onChange={(e) => setInstrumentName(e.target.value)} />
                                    </FormGroup>
                                </Col>
                            </Row
                            <Row>
                                <Col md="12">
                                    <FormGroup>
                                        <label>Currency</label>
                                        <Input type={"select"} value={type} onChange={(e) => setType(e.target.value)}>
                                            <option>ETF</option>
                                            <option>Fund</option>
                                            <option>Stock</option>
                                        </Input>
                                    </FormGroup>
                                </Col>
                            </Row>>
                            <Row>
                                <Col md="12">
                                    <FormGroup>
                                        <label>Isin</label>
                                        <Input type="text" value={isin} onChange={(e) => setIsin(e.target.value)} />
                                    </FormGroup>
                                </Col>
                            </Row>
                            <Row>
                                <Col md="12">
                                    <FormGroup>
                                        <label>Ticker</label>
                                        <Input type="text" value={ticker} onChange={(e) => setTicker(e.target.value)} />
                                    </FormGroup>
                                </Col>
                            </Row>
                            <Row>
                                <Col md="12">
                                    <FormGroup>
                                        <label>Currency</label>
                                        <Input type={"select"} value={currency} onChange={(e) => setCurrency(e.target.value)}>
                                            <option>EUR</option>
                                            <option>USD</option>
                                        </Input>
                                    </FormGroup>
                                </Col>
                            </Row>
                            <Row>
                                <Col md="12">
                                    <FormGroup>
                                        <label>Url</label>
                                        <Input type="text" value={url} onChange={(e) => setUrl(e.target.value)} />
                                    </FormGroup>
                                </Col>
                            </Row>
                            <Row>
                                <div className="update ml-auto mr-auto">
                                    <Button
                                        className="btn-round"
                                        color="danger"
                                        onClick={cancelForm}
                                    >
                                        Cancel
                                    </Button>
                                    <Button
                                        className="btn-round"
                                        color="primary"
                                        type="submit"
                                        disabled={IsSubmitted}
                                    >
                                        Save
                                    </Button>
                                </div>
                            </Row>
                        </Form>
                    </CardBody>
                </Card>
            </Col>
        </Row>

    );
}


export default NewInstrumentView