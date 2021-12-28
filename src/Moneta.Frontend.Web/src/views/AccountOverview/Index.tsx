import React from "react";
import { AccountsList,AccountListItem } from "views/AccountOverview/Accounts";
import { loginRequest } from "AuthConfig";
import {
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Table,
    Row,
    Col,
} from "reactstrap";

import { Spinner } from "reactstrap";
import { useMsal } from "@azure/msal-react";
import { AccountInfo } from "@azure/msal-browser";
import axios from "axios";

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
                let url = "/api/accounts";

                if (process.env.REACT_APP_API !== undefined ) {
                    url = process.env.REACT_APP_API + "/api/accounts";
                }
            
                const config = {
                    headers: { Authorization: `Bearer ${response.accessToken}` },
                    mode: "no-cors",
                };
                
                axios.get(url, config).then(r => setAccountItems(r.data));
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