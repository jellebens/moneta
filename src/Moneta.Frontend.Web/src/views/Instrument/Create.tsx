
import React, { useState, useEffect, Component  } from "react";
import { useLocation } from "react-router-dom";
import { InstrumentDetailResult, InstrumentSearchResult, Detail } from "views/Instrument/Instruments";
import {  useMsal } from "@azure/msal-react";
import { AccountInfo } from "@azure/msal-browser";
import { loginRequest } from "AuthConfig";


import {
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Row,
    Col,
    Button,
    Label, Spinner,
    Form, FormGroup,
    Input, InputGroup, InputGroupAddon,
    Table, Fade

} from "reactstrap";
import { useForm } from "react-hook-form";

type FormValues = {
    exchange: string
    symbol: string
    type : string
    name: string
    currency: string
  };

export const CreateInstrumentView = () => {
    const { register,handleSubmit,setValue,formState: { errors } } = useForm();
    

    const { instance, accounts } = useMsal();
    const location = useLocation();

    //const [instrument, setInstrument] = useState<InstrumentSearchResult>();
    const [instrumentDetail, setInstrumentDetail] = useState<InstrumentDetailResult>();
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        register('name')
        register('currency')
        register('symbol')
        register('type')
        register('exchange')
      }, [register])

    // useEffect(() => {
    //     var searchResult = location.state as InstrumentSearchResult
        
    //     const request = {
    //         scopes: loginRequest.scopes,
    //         account: accounts[0] as AccountInfo
    //     };

    //     instance.acquireTokenSilent(request).then(async (response) => {
    //         const r = await Detail(searchResult.symbol, response.accessToken);
            
    //         setInstrumentDetail(r);
            
    //         setIsLoading(false);
    //     }).catch((e) => {
    //         console.log(e);
    //     });
    // }, [location]);

    const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        var detail = instrumentDetail;
        
        if(detail){
            detail.name = e.target.value;
            console.log(detail.name);
            setInstrumentDetail(detail);
        }   
        
        console.log(instrumentDetail?.name);
    };

    const onSubmit = async (data:FormValues) => console.log(data);

    return (<>
            
         <Card>
                    <CardHeader>
                        <CardTitle tag="h4">Search new instrument</CardTitle>
                    </CardHeader>
                    <CardBody>
                        { isLoading ? <p className="text-center"><Spinner color="primary" /></p> :
                        <Form onSubmit={handleSubmit(onSubmit)}>
                            <FormGroup>
                                <Label for="name">Name</Label>
                                <Input name="name" defaultValue={instrumentDetail?.name} onChange={(e) => setValue('name', e.target.value)} />
                            </FormGroup>
                            <FormGroup>
                            <fieldset disabled>
                                <Label for="currency">Currency</Label>
                                <Input defaultValue={instrumentDetail?.currency} onChange={(e) => setValue('currency', e.target.value)} />
                            </fieldset>
                            </FormGroup>
                            <FormGroup>
                                <Label for="symbol">Symbol</Label>
                                <Input name="symbol" defaultValue={instrumentDetail?.name} onChange={(e) => setValue('symbol', e.target.value)} />
                            </FormGroup>
                            <FormGroup>
                                <Label for="type">Type</Label>
                                <Input name="type" defaultValue={instrumentDetail?.name} onChange={(e) => setValue('type', e.target.value)} />
                            </FormGroup>
                            <FormGroup>
                                <Label for="exchange">exchange</Label>
                                <Input name="exchange" defaultValue={instrumentDetail?.name} onChange={(e) => setValue('exchange', e.target.value)} />
                            </FormGroup>
                            <FormGroup>
                                <div className="text-center">
                                    <Button className="btn-round" color="danger" >Cancel</Button>
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