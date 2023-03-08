use crate::{
    components::config::{email_config::EmailConfigForm, validation_rule::ValidationRule},
    services::config::change_polling_rate,
};
use atmosphere_api::models::ReadingType;
use web_sys::HtmlInputElement;
use yew::prelude::*;
use yew_hooks::use_async;
use yew_router::prelude::*;

use super::AppRoute;
use crate::{components::modal, hooks::use_user_context::use_user_context};

#[function_component(Settings)]
pub fn settings() -> Html {
    let user = use_user_context();
    if !user.is_logged_in() {
        return html! {
            <Redirect<AppRoute> to={AppRoute::SignIn} />
        };
    }

    let modal_props = use_state(|| modal::Props {
        hidden: true,
        title: "".to_string(),
        message: "".to_string(),
        onclose: Callback::noop(),
    });
    let modal_props_clone = modal_props.clone();
    let modal_onclose = Callback::from(move |_| {
        modal_props_clone.set(modal::Props {
            hidden: true,
            title: "".to_string(),
            message: "".to_string(),
            onclose: Callback::noop(),
        });
    });

    let polling_rate = use_state(|| 0);
    let change_polling_rate = {
        let polling_rate = polling_rate.clone();
        use_async(async move { change_polling_rate(*polling_rate, None).await })
    };

    let on_polling_rate_change = {
        let polling_rate = polling_rate.clone();
        Callback::from(move |event: InputEvent| {
            let value = event
                .target_unchecked_into::<HtmlInputElement>()
                .value_as_number() as i32;
            polling_rate.set(value);
        })
    };

    let submit_polling_rate = {
        let change_polling_rate = change_polling_rate.clone();
        Callback::from(move |_| {
            change_polling_rate.run();
        })
    };

    html! {
        <div>
            <ValidationRule reading_type={ReadingType::Temperature} />
            <ValidationRule reading_type={ReadingType::Humidity} />
            //<ValidationRule reading_type={ReadingType::Pressure} />
            <EmailConfigForm />
            <div class="form-group">
                <label for="polling-rate">{"Polling rate (ms)"}</label>
                <input type="number" step="1" class="form-control" id="polling-rate" value={(*polling_rate).to_string()} oninput={on_polling_rate_change} />
            </div>
            <button class="btn btn-primary" onclick={submit_polling_rate}>{"Change polling rate"}</button>

            <modal::Modal hidden={modal_props.hidden.clone()}
                title={modal_props.title.clone()}
                message={modal_props.message.clone()}
                onclose={modal_onclose.clone()} />
        </div>
    }
}
