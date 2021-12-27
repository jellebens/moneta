import React from "react";
import { AccountsList,AccountListItem } from "views/AccountOverview/Accounts";
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


export const AccountOverview = () => {
    

    const [accounts, setAccounts] = React.useState<AccountListItem[]>([]);

    const[isLoading, setIsLoading] = React.useState(true);

    React.useEffect(() =>  {
        // const accounts = await AccountsList;
        const doListAccounts =  async () => {
            const result = await AccountsList();

            setAccounts(result);
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
                                                                    {accounts.map((account) => (
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