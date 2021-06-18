
import Home from "views/Home/Index"
import Dashboard from "views/Dashboard/Index";
import AccountOverview from "views/AccountOverview/Index";


var routes = [
  {
    path: "/",
    name: "Home",
    icon: "nc-icon nc-bank",
    component: Home
  },
  {
    path: "/dashboard",
    name: "Dashboard",
    icon: "nc-icon nc-chart-pie-36",
    component: Dashboard
  },
  {
    path: "/accounts",
    name: "Accounts",
    icon: "nc-icon nc-chart-pie-36",
    component: AccountOverview
  }
];

export default routes;

