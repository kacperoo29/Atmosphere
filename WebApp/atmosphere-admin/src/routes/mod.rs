use yew::prelude::*;
use yew_router::prelude::*;

pub mod devices;
pub mod home;
pub mod settings;
pub mod signin;
pub mod users;
pub mod create_user;

use home::Home;
use settings::Settings;
use signin::SignIn;

/// App routes
#[derive(Routable, Debug, Clone, PartialEq)]
pub enum AppRoute {
    #[at("/settings")]
    Settings,
    #[at("/signin")]
    SignIn,
    #[at("/devices")]
    Devices,
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
        AppRoute::PageNotFound => html! { "Page not found" },
    }
}
