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
import { useMsal } from "@azure/msal-react";
import { AccountInfo } from "@azure/msal-browser";
import { loginRequest } from "AuthConfig";
import { useHistory } from "react-router-dom";
import { v4 as uuid } from 'uuid';

import { HubConnectionBuilder } from '@microsoft/signalr';

const wait = (ms: number): Promise<void> => {
    return new Promise(resolve => setTimeout(resolve, ms));
}

export const NewAccountView = () => {
    const { instance, accounts } = useMsal();
    const [accountName, setAccountName] = useState("");
    const [currency, setCurrency] = useState("EUR");
    const [IsSubmitted, setIsSubmitted] = React.useState(false);
    const history = useHistory();

    let host = ""
    if(process.env.REACT_APP_API){
        host = process.env.REACT_APP_API;
    }

    const connection = new HubConnectionBuilder()
        .withUrl(host + "/hubs/commands")
        .withAutomaticReconnect()
        .build();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
        e.preventDefault();


        if (accountName !== "") {
            await submitForm();
        }
    };

    const cancelForm = async () => {
        history.push("/accounts");
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
            var id = uuid();
            connection.on(id, msg => {
                console.log('Update received: ' + msg.status)
                if(msg.status === 'Completed'){
                    history.push("/accounts")
                }
            });
            connection.start();

            await axios.post(url,{
                "Id": id,
                "Name": accountName,
                "Currency": currency
            } ,config);
        }).catch((e) => {
            console.log(e);
        });

    }


    return (
        <Row>
            <Col md="12">
                <Card>
                    <CardHeader>
                        <CardTitle tag="h4">Create new account</CardTitle>
                    </CardHeader>
                    <CardBody>
                        <Form onSubmit={handleSubmit} noValidate={true}>
                                    <FormGroup>
                                        <label>Name</label>
                                        <Input type="text" value={accountName} onChange={(e) => setAccountName(e.target.value)} />
                                    </FormGroup>
                                
                            
                                    <FormGroup>
                                        <label>Currency</label>
                                        <Input type={"select"} value={currency} onChange={(e) => setCurrency(e.target.value)}>
                                            <option>EUR</option>
                                            <option>USD</option>
                                        </Input>
                                    </FormGroup>
                                    <FormGroup>
                                        <div className="text-center">
                                            <Button className="btn-round" color="danger" onClick={cancelForm}>Cancel</Button>
                                            <Button className="btn-round" color="primary" type="submit" disabled={IsSubmitted}>Save</Button>
                                        </div>
                                    </FormGroup>
                        </Form>
                    </CardBody>
                </Card>
            </Col>
        </Row>

    );
}


export default NewAccountView