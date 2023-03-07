use std::collections::HashMap;

use atmosphere_api::models::ReadingType;
use wasm_bindgen::JsValue;
use yew::prelude::*;
use yew_hooks::prelude::*;
use yew_router::prelude::*;

use crate::bindings::draw_chart;
use crate::components::reading::reading_list::ReadingList;
use crate::hooks::use_user_context::use_user_context;
use crate::services::reading::get_chart_data;

use super::AppRoute;

/// Home page
#[function_component(Home)]
pub fn home() -> Html {
    let user = use_user_context();
    let temp_chart_data = use_state(|| None::<HashMap<String, f64>>);

    let fetch_temp_chart_data = {
        use_async(async move { get_chart_data(None, ReadingType::Temperature, None, None).await })
    };

    {
        let fetch_temp_chart_data = fetch_temp_chart_data.clone();
        use_mount(move || {
            fetch_temp_chart_data.run();
        });
    }

    {
        let chart_data = temp_chart_data.clone();
        let fetch_temp_chart_data = fetch_temp_chart_data.clone();
        use_effect_with_deps(
            move |fetch_temp_chart_data| {
                if let Some(res) = &fetch_temp_chart_data.data {
                    chart_data.set(Some(res.clone()));
                }

                || ()
            },
            fetch_temp_chart_data.clone(),
        );
    }

    {
        let chart_data = temp_chart_data.clone();
        use_effect_with_deps(
            move |chart_data| {
                log::info!("chart_data: {:?}", chart_data);
                if let Some(data) = &**chart_data {
                    let data = serde_wasm_bindgen::to_value(&data).unwrap();
                    draw_chart("tempChart", &data, "temperature");
                }

                || ()
            },
            chart_data.clone(),
        );
    }

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
        <div class="chart-container" width="40vh" height="40vh">
            <canvas id="tempChart" ></canvas>
        </div>
    }
}
