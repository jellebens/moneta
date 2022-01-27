
import Home from "views/Home/Index"
import Dashboard from "views/Dashboard/Index";
import AccountOverview from "views/Account/Index";
import CreateAccount from "views/Account/New";
import InstrumentOverview from "views/Instrument/Index";
import SearchInstrument from "views/Instrument/Search";
import CreateInstrument from "views/Instrument/Create";


var routes = [
  {
    path: "/",
    name: "Home",
    icon: "nc-icon nc-bank",
    showInSideBar: true,
    component: Home
  },
  {
    path: "/dashboard",
    name: "Dashboard",
    icon: "nc-icon nc-chart-pie-36",
    showInSideBar: true,
    component: Dashboard
  },
  {
    path: "/accounts",
    name: "Accounts",
    icon: "fa fa-wallet",
    showInSideBar: true,
    component: AccountOverview
  },
  {
    path: "/accounts/new",
    name: "Create Account",
    showInSideBar: false,
    component: CreateAccount
  },
  {
    path: "/instruments",
    name: "Instruments",
    icon: "fa fa-coins",
    showInSideBar: true,
    component: InstrumentOverview
  },
  {
    path: "/instruments/search",
    name: "Create Instrument",
    showInSideBar: false,
    component: SearchInstrument
  },
  {
    path: "/instruments/new",
    name: "Create Instrument",
    showInSideBar: false,
    component: CreateInstrument
  }

];

export default routes;

