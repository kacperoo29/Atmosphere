use yew::prelude::*;
use yew_router::prelude::*;

pub mod home;
pub mod settings;
pub mod signin;
pub mod devices;

use home::Home;
use signin::SignIn;
use settings::Settings;

/// App routes
#[derive(Routable, Debug, Clone, PartialEq)]
pub enum AppRoute {
    #[at("/settings")]
    Settings,
    #[at("/signin")]
    SignIn,
    #[at("/devices")]
    Devices,
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
        AppRoute::PageNotFound => html! { "Page not found" },
    }
}
