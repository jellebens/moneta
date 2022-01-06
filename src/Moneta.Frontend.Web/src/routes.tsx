
import Home from "views/Home/Index"
import Dashboard from "views/Dashboard/Index";
import AccountOverview from "views/Account/Index";
import CreateAccount from "views/Account/New";


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
    icon: "nc-icon nc-chart-pie-36",
    showInSideBar: true,
    component: AccountOverview
  },
  {
    path: "/accounts/new",
    name: "Create Account",
    showInSideBar: false,
    component: CreateAccount
  }

];

export default routes;

