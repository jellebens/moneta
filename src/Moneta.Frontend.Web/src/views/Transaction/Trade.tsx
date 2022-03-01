import React, { useState } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Table,
    Row,
    Col,
    Button,
    Form, Input, FormGroup, Label, FormFeedback
} from "reactstrap";


import { useForm, Controller } from "react-hook-form";
import { NavLink } from "react-router-dom";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { loginRequest } from "AuthConfig";
import { AccountInfo } from "@azure/msal-browser";
import { v4 as uuid } from 'uuid';
import { format } from 'date-fns'
import axios from "axios";
import AsyncSelect from 'react-select'

import { AccountsList, AccountListItem, GetSelected } from "views/Account/Accounts";

export interface NewSecurityTransaction {
    id: string
    securityId: string
    date: string
    amount: number,
    currency: string,
}

export const CashTransfer = () => {
    const {  handleSubmit, reset,  control , formState: { errors }, setFocus } = useForm<NewCashDeposit>();
    const { instance, accounts } = useMsal();
    const [accountItems, setAccountItems] = React.useState<AccountListItem[]>([]);
    const [activeAccount, setActiveAccount] = useState<AccountListItem>();
    const [startDate, setStartDate] = useState(new Date());
    const [cashDeposit, SetCashDeposit] = useState<NewSecurityTransaction>();
    const onSubmit = async (data:NewSecurityTransaction) => {
        const request = {
            scopes: loginRequest.scopes,
            account: accounts[0] as AccountInfo
        };

        instance.acquireTokenSilent(request).then(async (response) => {
            
            let url = "/api/security/trade";
            

            const config = {
                headers: { Authorization: `Bearer ${response.accessToken}` },
                mode: "no-cors",
            };

            
            axios.post(url, data ,config);
            //SetCashDeposit({id: uuid(), accountId: data.accountId, currency: data.currency , amount:0, date: data.date});
        }).catch((e) => {
            console.log(e);
        });
                
       
    }
    
    React.useEffect(() => {
        // reset form with user data
        reset(cashDeposit);
    }, [cashDeposit]);


    React.useEffect(() => {
        
        GetSelected().then(a => {
            setActiveAccount(a); 
            
            //SetCashDeposit({id: uuid(), accountId: a.id,currency: a.currency , amount:0, date: format(new Date(), 'yyyy-MM-dd') });
        });
        
        const doListAccounts = async () => {
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
        }

        doListAccounts();
    }, []);

    return (
        <>
        <AuthenticatedTemplate>
            <Row>
                <Col md="12">
                    <Card body>
                          <CardTitle className="text-center">Trade Security</CardTitle>
                          <Form onSubmit={handleSubmit(onSubmit)}>
                          <FormGroup>
                                <Label for="account">Account</Label>
                                <Controller
                                    name="accountId"
                                    control={control}
                                    render={({ field }) => <Input type={"select"} {...field}>
                                        {accountItems.map((acc) => (
                                            <option value={acc.id} >{acc?.name}</option>
                                        ))}
                                    </Input>                                   
                                    }
                                />
                            </FormGroup>
                            <FormGroup>
                                <Label for="security">Security</Label>
                                <Controller
                                    name="security"
                                    control={control}
                                    render={({ field }) => <AsyncSelect {...field} />
                                    }
                                />
                            </FormGroup>
                            <FormGroup>
                                <Label for="date">Date</Label>
                                
                                <Controller
                                    name="date"
                                    control={control}
                                    render={({ field }) => <Input type={"date"} {...field} />
                                    }
                                />
                            </FormGroup>
                            <FormGroup>
                                <Label for="amount">Amount</Label>
                                <Controller
                                    name="amount"
                                    control={control}
                                    rules={{
                                        validate: value => {
                                            return value !== 0;
                                        }

                                    }}
                                    render={({ field }) => <Input type={"number"} step="any" {...field} invalid={!!errors.amount } />
                                    }
                                />
                                 <FormFeedback invalid={!errors.amount } >Amount can not be 0</FormFeedback>
                            </FormGroup>
                            <FormGroup>
                                <div className="text-center">
                                    <Button className="btn-round" color="danger"  >Cancel</Button>
                                    <Button className="btn-round" color="primary" type="submit" >Save</Button>
                                </div>
                            </FormGroup>
                        </Form>
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

export default CashTransfer;