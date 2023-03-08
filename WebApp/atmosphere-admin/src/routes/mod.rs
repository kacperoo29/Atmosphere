use yew::prelude::*;
use yew_router::prelude::*;

pub mod create_user;
pub mod devices;
pub mod home;
pub mod settings;
pub mod signin;
pub mod users;

use home::Home;
use settings::Settings;
use signin::SignIn;
use crate::components::reading::reading_list;
use crate::components::chart::Chart;

/// App routes
#[derive(Routable, Debug, Clone, PartialEq)]
pub enum AppRoute {
    #[at("/readings")]
    Readings,
    #[at("/readings/:id")]
    DeviceReadings { id: String },
    #[at("/settings")]
    Settings,
    #[at("/signin")]
    SignIn,
    #[at("/devices")]
    Devices,
    #[at("/devices_chart/:id")]
    DeviceChart { id: String },
    #[at("/users")]
    Users,
    #[at("/newuser")]
    NewUser,
    #[not_found]
    #[at("/page-not-found")]
    PageNotFound,
    #[at("/")]
    Home,
}

/// Switch app routes
pub fn switch(routes: &AppRoute) -> Html {
    match routes.clone() {
        AppRoute::Home => html! { <Home /> },
        AppRoute::SignIn => html! { <SignIn /> },
        AppRoute::Settings => html! { <Settings /> },
        AppRoute::Devices => html! { <devices::Devices /> },
        AppRoute::Users => html! { <users::Users /> },
        AppRoute::NewUser => html! { <create_user::CreateUser /> },
        AppRoute::Readings => html! { <reading_list::ReadingList /> },
        AppRoute::DeviceReadings { id } => {
            html! { <reading_list::ReadingList device_id={Some(id)} /> }
        }
        AppRoute::DeviceChart { id } => {
            html! { <Chart device_id={uuid::Uuid::parse_str(&id).ok()} canvas_id={"device_chart"} /> }
        }
        AppRoute::PageNotFound => html! { "Page not found" },
    }
}
