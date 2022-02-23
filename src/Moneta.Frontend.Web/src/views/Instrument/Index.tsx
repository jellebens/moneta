import React from "react";
import { InstrumentList, DeleteInstrument, InstrumentListItem } from "views/Instrument/Instruments";
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

export const InstrumentOverview = () => {

    const { instance, accounts } = useMsal();
    const [instruments, setInstruments] = React.useState<InstrumentListItem[]>([]);
    const [isLoading, setIsLoading] = React.useState(true);

    React.useEffect(() => {
        const doListAccounts = async () => {
            const request = {
                scopes: loginRequest.scopes,
                account: accounts[0] as AccountInfo
            };

            instance.acquireTokenSilent(request).then(async (response) => {
                const result = await InstrumentList(response.accessToken);
                setInstruments(result);
                setIsLoading(false);
            }).catch((e) => {
                console.log(e);
                setIsLoading(false);
            });
        }

        doListAccounts();
    }, []);

    function handleRemove(id: string) {
        setIsLoading(true);
        const request = {
            scopes: loginRequest.scopes,
            account: accounts[0] as AccountInfo
        };

        instance.acquireTokenSilent(request).then(async (response) => {
            await DeleteInstrument(response.accessToken, id);
            setIsLoading(false);
        }).catch((e) => {
            console.log(e);
            setIsLoading(false);
        });

        const newlist = instruments.filter((item) => item.id !== id);
        setInstruments(newlist);
        // remove item
      }
    

    return (
        <>
        <AuthenticatedTemplate>
            <Row>
                <Col md="12">
                    <Card>
                        <CardHeader>
                            <CardTitle tag="h4">Instruments</CardTitle>
                        </CardHeader>
                        <CardBody>
                            <NavLink className="btn btn-icon btn-round btn-primary" to="/instruments/search"><i className="fa fa-plus"></i></NavLink>
                            {
                                isLoading ? <div className="text-center"><Spinner color="primary" /></div> :
                                    <Table striped>
                                        <thead className="text-primary">
                                            <tr>
                                                <th>Name</th>
                                                <th>Symbol</th>
                                                <th>Currency</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {instruments.map((instrument) => (
                                                <tr key={instrument.id}>
                                                    <td>{instrument.name}</td>
                                                    <td>{instrument.symbol}</td>
                                                    <td>{instrument.currency}</td>
                                                    <td>
                                                        <Button className="btn btn-icon btn-round btn-danger" color="danger" type="button" onClick={() => handleRemove(instrument.id)}>
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

export default InstrumentOverview;