import React, { useState } from "react";

import { loginRequest } from "AuthConfig";
import { NavLink } from "react-router-dom";
import {
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Table,
    Row,
    Col,
    Button
} from "reactstrap";

import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { AccountInfo } from "@azure/msal-browser";

import { BarChart, Bar, Cell, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

import { AccountListItem , GetSelected, Summary, AccountSummary } from "views/Account/Accounts";
import NumberFormat from 'react-number-format';


export const NewTransaction = () => {
    const [accountSummary, setAccountSummary] = useState<AccountSummary>()

    const [activeAccount, setActiveAccount] = useState<AccountListItem>();
    const { instance, accounts } = useMsal();

    React.useEffect(() => {
        
         GetSelected().then(a => {
            setActiveAccount(a)
            
            const request = {
                scopes: loginRequest.scopes,
                account: accounts[0] as AccountInfo
            };

            instance.acquireTokenSilent(request).then(async (response) => {
                const result = await Summary(a.id ,response.accessToken);
                setAccountSummary(result);
            }).catch((e) => {
                console.log(e);
                
            });
      });
        
    }, []);
    

    return (
        <>
        <AuthenticatedTemplate>
            
            <Row>
                <Col md="4">
                    <Card body>
                          <CardTitle className="text-center">Transfer funds</CardTitle>
                          <div className="text-center">Total funds deposited: <NumberFormat value={accountSummary?.total} displayType={'text'} thousandSeparator={true} decimalScale={2} /> &nbsp;{accountSummary?.currency}</div>
                          <ResponsiveContainer width="100%" height={300}>
                          <BarChart
                            width={500}
                            height={300}
                            data={accountSummary?.lines}
                            >
                            <CartesianGrid strokeDasharray="3 3" />
                            <XAxis dataKey="year" />
                            <YAxis />
                            <Tooltip />
                            <Legend />
                            <Bar dataKey="amount" label="test" fill="#8884d8" />
                            
                            </BarChart>
                            </ResponsiveContainer>
                          <NavLink className="btn btn-default" to="transfer">Transfer</NavLink>
                    </Card>
                    </Col>
                    <Col md="4">
                    <Card body>
                          <CardTitle className="text-center">New Trade</CardTitle>
                          <Button>Trade</Button>
                    </Card>
                </Col>
                <Col md="4">
                <Card body>
                          <CardTitle className="text-center">New Dividend</CardTitle>
                          <Button>Create</Button>
                    </Card>
                </Col>
            </Row>
        </AuthenticatedTemplate>
        <UnauthenticatedTemplate>
            Please Sign in
        </UnauthenticatedTemplate>
        </>

    );

}

export default NewTransaction;