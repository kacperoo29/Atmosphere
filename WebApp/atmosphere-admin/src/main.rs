mod pages;

use yew::prelude::*;
use yew_router::prelude::*;

use crate::pages::*;

#[derive(Routable, Debug, Clone, Copy, PartialEq)]
enum Route {
    #[at("/")]
    Home,
}

fn switch(route: Route) -> Html {
    match route {
        Route::Home => html! { <home::HomePage /> },
        _ => html! { <not_found::NotFoundPage /> },
    }
}

#[function_component(App)]
fn app() -> Html {
    html! {
        <BrowserRouter>
            <Switch<Route> render={switch} />
        </BrowserRouter>
    }
}

fn main() {
    wasm_logger::init(wasm_logger::Config::default());
    yew::start_app::<App>();
}
