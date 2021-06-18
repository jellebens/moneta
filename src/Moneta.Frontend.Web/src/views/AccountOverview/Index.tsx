import React from "react";
import { dummyAccounts } from "views/AccountOverview/Accounts";
import {
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Table,
    Row,
    Col,
} from "reactstrap";




export const AccountOverview = () => {
    const accounts = dummyAccounts
    return (
       
            <Row>
                <Col md="12">
                    <Card>
                        <CardHeader>
                            <CardTitle tag="h4">Accounts</CardTitle>
                        </CardHeader>
                        <CardBody>
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
                        </CardBody>
                    </Card>
                </Col>
            </Row>



    );

}

export default AccountOverview;