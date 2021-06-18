/*!

=========================================================
* Paper Dashboard React - v1.3.0
=========================================================

* Product Page: https://www.creative-tim.com/product/paper-dashboard-react
* Copyright 2021 Creative Tim (https://www.creative-tim.com)

* Licensed under MIT (https://github.com/creativetimofficial/paper-dashboard-react/blob/main/LICENSE.md)

* Coded by Creative Tim

=========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

*/
import React from "react";
// javascript plugin used to create scrollbars on windows
import PerfectScrollbar from "perfect-scrollbar";
import { Route, useLocation } from "react-router-dom";

import Navbar from "components/Navbars/Navbar";
import Footer from "components/Footer/Footer";
import Sidebar from "components/Sidebar/Sidebar";

import routes from "routes";

var ps: PerfectScrollbar;

type DashboardProps = {

}

export const Dashboard = (props: DashboardProps) => {
  const [backgroundColor] = React.useState("black");
  const [activeColor] = React.useState("warning");
  const mainPanel = React.useRef<HTMLDivElement>(null);
  const location = useLocation();
  React.useEffect(() => {
    if (navigator.platform.indexOf("Win") > -1) {
      ps = new PerfectScrollbar(mainPanel.current ?? '');
      document.body.classList.toggle("perfect-scrollbar-on");
    }
    return function cleanup() {
      if (navigator.platform.indexOf("Win") > -1) {
        ps.destroy();
        document.body.classList.toggle("perfect-scrollbar-on");
      }
    };
  });
  React.useEffect(() => {
    mainPanel.current!.scrollTop = 0;
    document.scrollingElement!.scrollTop = 0;
  }, [location]);
  return (
    <div className="wrapper">
      <Sidebar
        {...props}
        routes={routes}
        bgColor={backgroundColor}
        activeColor={activeColor}
      />
      <div className="main-panel" ref={mainPanel}>
        <Navbar {...props} />
        <div className="content">
          {routes.map((prop, key) => {
            return (
              <Route exact path={prop.path} component={prop.component} key={key} />
            );
          })}
        </div>

        <Footer fluid />
      </div>
      
    </div>
  );
}

export default Dashboard;
