import React from "react";
import { AccountsList,AccountListItem } from "views/Account/Accounts";
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
import { useMsal } from "@azure/msal-react";
import { AccountInfo } from "@azure/msal-browser";

export const AccountOverview = () => {
    
    const { instance, accounts } = useMsal();
    const [accountItems, setAccountItems] = React.useState<AccountListItem[]>([]);
    const[isLoading, setIsLoading] = React.useState(true);

    React.useEffect(() =>  {
        const doListAccounts =  async () => {
            const request = {
                scopes: loginRequest.scopes,
                account: accounts[0] as AccountInfo
            };
            
            instance.acquireTokenSilent(request).then(async (response) => {
                const result = await AccountsList(response.accessToken);
                setAccountItems(result);
            }).catch((e) => {
                console.log(e);
            });

            setIsLoading(false);
        }

        doListAccounts();
    }, []);

    return (
            <Row>
                <Col md="12">
                    <Card>
                        <CardHeader>
                            <CardTitle tag="h4">Accounts</CardTitle>
                        </CardHeader>
                        <CardBody>
                        <NavLink className="btn btn-icon btn-round btn-primary" to="/accounts/new"><i className="fa fa-plus"></i></NavLink>
                            {
                            isLoading ? <p className="text-center"><Spinner color="primary" /></p>:
                                                            <Table striped>
                                                                <thead className="text-primary">
                                                                    <tr>
                                                                        <th>Name</th>
                                                                        <th>Currency</th>
                                                                        <th></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    {accountItems.map((account) => (
                                                                        <tr key={account.name + '_' + account.currency}>
                                                                            <td>{account.name}</td>
                                                                            <td>{account.currency}</td>
                                                                            <td></td>
                                                                        </tr>
                                                                    ))}
                                                                </tbody>
                                                            </Table>
                            }
                        </CardBody>
                    </Card>
                </Col>
            </Row>



    );

}

export default AccountOverview;