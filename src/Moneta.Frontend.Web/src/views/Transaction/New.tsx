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

import { AccountListItem , GetSelected } from "views/Account/Accounts";





export const NewTransaction = () => {
   
    const [activeAccount, setActiveAccount] = useState<AccountListItem>();

    React.useEffect(() => {
      GetSelected().then(a => setActiveAccount(a));
    },[]);

    return (
        <>
        <AuthenticatedTemplate>
            <Row>
                <Col md="4">
                    <Card body>
                          <CardTitle className="text-center">Transfer funds</CardTitle>
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