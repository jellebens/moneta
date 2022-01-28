
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
    Form, FormGroup,
    Input,

} from "reactstrap";
import { useForm, Controller } from "react-hook-form";



export const CreateInstrumentView = () => {
    const { register,handleSubmit,setValue, reset,control ,formState: { errors } } = useForm<NewInstrument>();
    

    const { instance, accounts } = useMsal();
    const location = useLocation();
    const history = useHistory();
    
    const [instrumentDetail, setInstrumentDetail] = useState<InstrumentDetailResult>();
    const [isLoading, setIsLoading] = useState(false);


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
                setIsLoading(false);
            }).catch((e) => {
                console.log(e);
            });
        }else{
            setIsLoading(false);
        }
    }, [location]);

    
    const onSubmit = async (data:NewInstrument) => console.log(data);

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
                            <FormGroup>
                                <Label for="name">Name</Label>
                                <Input defaultValue={instrumentDetail?.name } {...register("name")} onChange={(e) => setValue('name', e.target.value)} />
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
                                    <Button className="btn-round" color="primary" type="submit" >Save</Button>
                                </div>
                            </FormGroup>
                        </Form>
                    }
                    </CardBody>
                </Card>
    
    
    </>);
};


export default CreateInstrumentView