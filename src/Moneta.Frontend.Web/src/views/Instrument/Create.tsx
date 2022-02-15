
import React, { useState, useEffect, Component  } from "react";
import { useLocation } from "react-router-dom";
import { InstrumentDetailResult, InstrumentSearchResult, Detail, NewInstrument } from "views/Instrument/Instruments";
import {  useMsal } from "@azure/msal-react";
import { AccountInfo } from "@azure/msal-browser";
import { loginRequest } from "AuthConfig";
import { useHistory } from "react-router-dom";

import {
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Button,
    Label, Spinner,
    Form, FormGroup, FormFeedback,
    Input,InputGroup

} from "reactstrap";
import { useForm, Controller } from "react-hook-form";
import axios from "axios";
import { v4 as uuid } from 'uuid';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export const CreateInstrumentView = () => {
    const { register, handleSubmit, setValue, reset, watch, control , formState: { errors }, setError } = useForm<NewInstrument>();
    

    const { instance, accounts } = useMsal();
    const location = useLocation();
    const history = useHistory();
    
    const [instrumentDetail, setInstrumentDetail] = useState<InstrumentDetailResult>();
    const [id, setId] = useState(uuid());
    const [isLoading, setIsLoading] = useState(false);
    const [IsSubmitted, setIsSubmitted] = React.useState(false);

    const type = watch('type');

    let host = ""
    if(process.env.REACT_APP_API){
        host = process.env.REACT_APP_API;
    }

    useEffect(() => {
        if(type !== 'Stock'){
            setValue('sector', 0);
        }
    },[type]);

    useEffect(() => {
        setIsLoading(true);
        var searchResult = location.state as InstrumentSearchResult
        if(searchResult){
            const request = {
                scopes: loginRequest.scopes,
                account: accounts[0] as AccountInfo
            };
    
            instance.acquireTokenSilent(request).then(async (response) => {
                const r = await Detail(searchResult.symbol, response.accessToken);
                
                setInstrumentDetail(r);
                
                reset(r)
                setValue("id", id);
                setValue("sector", 0);
                setIsLoading(false);
            }).catch((e) => {
                console.log(e);
            });
        }else{
            setIsLoading(false);
        }
    }, [location]);

    
    const onSubmit = async (data:NewInstrument) => {
        
        const request = {
            scopes: loginRequest.scopes,
            account: accounts[0] as AccountInfo
        };

        instance.acquireTokenSilent(request).then(async (response) => {
            setIsSubmitted(false);

            let url = "/api/instruments";
            

            const config = {
                headers: { Authorization: `Bearer ${response.accessToken}` },
                mode: "no-cors",
            };

            const connection = new HubConnectionBuilder()
            .configureLogging(LogLevel.Warning)
            .withUrl(host + "/hubs/commands", {
                accessTokenFactory: () => {
                    return `${response.accessToken}`
                }
            })
            .withAutomaticReconnect()
            .build();

            connection.on(id, msg => {
                if(msg.status === 'Completed'){
                    history.push("/instruments")
                }
            });

            await connection.start();
            console.log(data);
            await axios.post(url, data ,config);
        }).catch((e) => {
            console.log(e);
        });
    }

    function onCancel(){
        history.push("/instruments/");
    }

    return (<>
            
         <Card>
                    <CardHeader>
                        <CardTitle tag="h4">Search new instrument</CardTitle>
                    </CardHeader>
                    <CardBody>
                        { isLoading ? <div className="text-center"><Spinner color="primary" /></div> :
                        <Form onSubmit={handleSubmit(onSubmit)}>
                            <Input type="hidden" defaultValue={id} {...register("id")} />
                            <FormGroup>
                                <Label for="name">Name</Label>
                                <Input defaultValue={instrumentDetail?.name } {...register("name")} onChange={(e) => setValue('name', e.target.value)} invalid={!!errors.name} />
                            </FormGroup>
                            <FormGroup>
                                <Label for="currency">Currency</Label>
                                <Controller
                                    name="currency"
                                    control={control}
                                    render={({ field }) => <Input type={"select"} {...field}>
                                        <option></option>
                                        <option value="EUR">EUR</option>
                                        <option value="USD">USD</option>
                                    </Input>                                   
                                    }
                                />
                            </FormGroup>
                            <FormGroup>
                                <Label for="symbol">Symbol</Label>
                                <Input defaultValue={instrumentDetail?.symbol} {...register("symbol")} onChange={(e) => setValue('symbol', e.target.value)} />
                            </FormGroup>
                            <FormGroup>
                                <Label for="symbol">Isin</Label>
                                <Input {...register("isin")} onChange={(e) => setValue('isin', e.target.value)} />
                            </FormGroup>
                            <FormGroup>
                                <Label for="sector">Sector</Label>
                                <Controller
                                    name="sector"
                                    control={control}
                                    rules={{
                                        validate: value => {
                                            if(watch("type") === 'Stock' && value ===0 ){
                                                return false;
                                            }
                                            return true;
                                        }

                                    }}
                                    render={({ field }) => <Input type={"select"} {...field} disabled={watch("type") !== 'Stock'} invalid={!!errors.sector } >
                                        <option value="0">None</option>
                                        <option value="1">Energy</option>
                                        <option value="2">Materials</option>
                                        <option value="3">Industrials</option>
                                        <option value="4">Utilities</option>
                                        <option value="5">Healthcare</option>
                                        <option value="6">Financials</option>
                                        <option value="7">Consumer Discretionary</option>
                                        <option value="8">Consumer Staples</option>
                                        <option value="9">Information Technology</option>
                                        <option value="10">Communication Services</option>
                                        <option value="11">Real Estate</option>
                                    </Input>                                   
                                    }
                                />
                                <FormFeedback invalid >Sector is mandatory for stocks</FormFeedback>
                            </FormGroup>
                            <FormGroup>
                                <Label for="type">Type</Label>
                                <Controller
                                    name="type"
                                    control={control}
                                    render={({ field }) => <Input type={"select"} {...field}>
                                        <option></option>
                                        <option value="ETF">ETF</option>
                                        <option value="Stock">Stock</option>
                                        <option value="Mutual Fund">Mutual Fund</option>
                                        <option value="Index">Index</option>
                                    </Input>                                   
                                    }
                                />
                            </FormGroup>
                            <FormGroup>
                                <Label for="exchange">exchange</Label>
                                <Input defaultValue={instrumentDetail?.exchange} {...register("type")} onChange={(e) => setValue('exchange', e.target.value)} />
                            </FormGroup>
                            <FormGroup>
                                <div className="text-center">
                                    <Button className="btn-round" color="danger" onClick={onCancel} >Cancel</Button>
                                    <Button className="btn-round" color="primary" type="submit" disabled={IsSubmitted} >Save</Button>
                                </div>
                            </FormGroup>
                        </Form>
                    }
                    </CardBody>
                </Card>
    
    
    </>);
};


export default CreateInstrumentView