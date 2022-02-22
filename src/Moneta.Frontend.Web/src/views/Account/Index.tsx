import React, { useState } from "react";
import { AccountsList, AccountListItem, DeleteAccount, GetSelected, SetSelected } from "views/Account/Accounts";
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


import { Spinner } from "reactstrap";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { AccountInfo } from "@azure/msal-browser";

export const AccountOverview = () => {

    const { instance, accounts } = useMsal();
    const [accountItems, setAccountItems] = React.useState<AccountListItem[]>([]);
    const [isLoading, setIsLoading] = React.useState(true);
    const [activeAccount, setActiveAccount] = useState<AccountListItem>();

    React.useEffect(() => {
        
        GetSelected().then(a => setActiveAccount(a));
        
        const doListAccounts = async () => {
            const request = {
                scopes: loginRequest.scopes,
                account: accounts[0] as AccountInfo
            };

            instance.acquireTokenSilent(request).then(async (response) => {
                const result = await AccountsList(response.accessToken);
                setAccountItems(result);
                setIsLoading(false);
            }).catch((e) => {
                console.log(e);
                setIsLoading(false);
            });
        }

        doListAccounts();
    }, []);

    function setActive(account: AccountListItem){
        SetSelected(account);
        setActiveAccount(account);
    } 

    function handleRemove(id: string) {
        setIsLoading(true);
        const request = {
            scopes: loginRequest.scopes,
            account: accounts[0] as AccountInfo
        };

        instance.acquireTokenSilent(request).then(async (response) => {
            await DeleteAccount(response.accessToken, id);
            setIsLoading(false);
        }).catch((e) => {
            console.log(e);
            setIsLoading(false);
        });

        const newlist = accountItems.filter((item) => item.id !== id);
        setAccountItems(newlist);
        // remove item
      }
    

    return (
        <>
        <AuthenticatedTemplate>
            <Row>
                <Col md="12">
                    <Card>
                        <CardHeader>
                            <CardTitle tag="h4">Accounts</CardTitle>
                        </CardHeader>
                        <CardBody>
                            <NavLink className="btn btn-icon btn-round btn-primary" to="/accounts/new"><i className="fa fa-plus"></i></NavLink>
                            {
                                isLoading ? <div className="text-center"><Spinner color="primary" /></div> :
                                    <Table striped>
                                        <thead className="text-primary">
                                            <tr>
                                                <th>Active</th>
                                                <th>Name</th>
                                                <th>Currency</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {accountItems.map((account) => (
                                                <tr key={account.id}>
                                                    <td>
                                                        <Button className="btn btn-icon btn-square" color="primary" type="button" onClick={() => setActive(account)} >
                                                            {account.id == activeAccount?.id ? <i className="fa fa-check" aria-hidden="true"></i>: <></>
                                                            }
                                                        </Button></td>
                                                    <td>{account.name}</td>
                                                    <td>{account.currency}</td>
                                                    <td>
                                                        <Button className="btn btn-icon btn-round btn-danger" color="danger" type="button" onClick={() => handleRemove(account.id)}>
                                                            <i className="fa fa-trash"></i>
                                                        </Button>
                                                    </td>
                                                        
                                                </tr>
                                            ))}
                                        </tbody>
                                    </Table>
                            }
                        </CardBody>
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

export default AccountOverview;