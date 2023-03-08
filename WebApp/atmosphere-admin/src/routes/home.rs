use yew::prelude::*;
use yew_router::prelude::*;

use crate::hooks::use_user_context::use_user_context;
use crate::components::chart::Chart;

use super::AppRoute;

/// Home page
#[function_component(Home)]
pub fn home() -> Html {
    let user = use_user_context();

    if !user.is_logged_in() {
        return html! {
            <div>
                <p>{"You must login to use this site."}</p>
                <Link<AppRoute> to={AppRoute::SignIn}>
                    <button class="btn btn-primary">{"Sign In"}</button>
                </Link<AppRoute>>
            </div>
        };
    }

    html! {
        <Chart canvas_id="home_chart" />
    }
}
