use yew::prelude::*;
use yew_router::prelude::*;

pub mod home;
pub mod signin;

use home::Home;
use signin::SignIn;

/// App routes
#[derive(Routable, Debug, Clone, PartialEq)]
pub enum AppRoute {
    #[at("/signin")]
    SignIn,
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
        AppRoute::PageNotFound => html! { "Page not found" },
    }
}
