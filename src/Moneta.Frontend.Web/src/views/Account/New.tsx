import React, { useState } from "react";
import {
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Row,
    Col,
    Button,
    Form, FormGroup,
    Input,

} from "reactstrap";



export const NewAccountView = () => {

    const [accountName, setAccountName] = useState("");
    const [currency, setCurrency] = useState("EUR");

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
        e.preventDefault();


        if(accountName !== ""){
            await submitForm();
        }
    };

        

     const submitForm = async() : Promise<boolean> => {
        // TODO - submit the form
        
        return true;
}


return (
    <Row>
        <Col md="12">
            <Card>
                <CardHeader>
                    <CardTitle tag="h4">Create new Account</CardTitle>
                </CardHeader>
                <CardBody>
                    <Form onSubmit={handleSubmit} noValidate={true}>
                        <Row>
                            <Col md="12">
                                <FormGroup>
                                    <label>Name</label>
                                    <Input type="text" value={accountName} onChange={(e) => setAccountName(e.target.value)} />
                                </FormGroup>
                            </Col>
                        </Row>
                        <Row>
                            <Col md="12">
                                <FormGroup>
                                    <label>Currency</label>
                                    <Input type={"select"} value={currency} onChange={(e) => setCurrency(e.target.value)}>
                                        <option>EUR</option>
                                        <option>USD</option>
                                    </Input>
                                </FormGroup>
                            </Col>
                        </Row>
                        <Row>
                            <div className="update ml-auto mr-auto">
                                <Button
                                    className="btn-round"
                                    color="danger"
                                >
                                    Cancel
                                </Button>
                                <Button
                                    className="btn-round"
                                    color="primary"
                                    type="submit"
                                >
                                    Save
                                </Button>
                            </div>
                        </Row>
                    </Form>
                </CardBody>
            </Card>
        </Col>
    </Row>

);
}


export default NewAccountView