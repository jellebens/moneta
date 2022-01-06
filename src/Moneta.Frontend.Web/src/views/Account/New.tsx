import React from "react";
import {
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Table,
    Row,
    Col,
    Button,
    Dropdown, DropdownToggle, DropdownMenu, DropdownItem,
    Form, FormGroup,
    Input,

} from "reactstrap";


export const CreateAccount = () => {
    const [isOpen, setIsOpen] = React.useState<boolean>(false);
    const toggle = () => setIsOpen(prevState => !prevState);

    return (
        <Row>
            <Col md="12">
                <Card>
                    <CardHeader>
                        <CardTitle tag="h4">Create new Account</CardTitle>
                    </CardHeader>
                    <CardBody>
                        <Form>
                            <Row>
                                <Col md="12">
                                    <FormGroup>
                                        <label>Name</label>
                                        <Input
                                            type="text"
                                        />
                                    </FormGroup>
                                </Col>
                            </Row>
                            <Row>
                                <Col md="12">
                                    <FormGroup>
                                        <label>Currency</label>
                                        <Dropdown isOpen={isOpen} toggle={toggle}>
                                            <DropdownToggle caret>Select Currency</DropdownToggle>
                                            <DropdownMenu
                                                modifiers={{
                                                    setMaxHeight: {
                                                        enabled: true,
                                                        order: 890,
                                                        fn: data => {
                                                            return {
                                                                ...data,
                                                                styles: {
                                                                    ...data.styles,
                                                                    overflow: "auto",
                                                                    maxHeight: "100px"
                                                                }
                                                            };
                                                        }
                                                    }
                                                }}
                                            >
                                                <DropdownItem>EUR</DropdownItem>
                                                <DropdownItem>USD</DropdownItem>
                                            </DropdownMenu>
                                        </Dropdown>
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
export default CreateAccount;