import Home from "views/Home/Index"
import Dashboard from "views/Dashboard/Index";

var routes = [
  {
    path: "/home",
    name: "Home",
    icon: "nc-icon nc-bank",
    component: Home
  },
  {
    path: "/dashboard",
    name: "Dashboard",
    icon: "nc-icon nc-chart-pie-36",
    component: Dashboard
  }
];

export default routes;

