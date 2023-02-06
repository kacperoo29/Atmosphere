use crate::components::config::{email_config::EmailConfigForm, validation_rule::ValidationRule};
use atmosphere_api::models::ReadingType;
use yew::prelude::*;
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

    html! {
        <div>
            <ValidationRule reading_type={ReadingType::Temperature} />
            <ValidationRule reading_type={ReadingType::Humidity} />
            <ValidationRule reading_type={ReadingType::Pressure} />
            <EmailConfigForm />

            <modal::Modal hidden={modal_props.hidden.clone()}
                title={modal_props.title.clone()}
                message={modal_props.message.clone()}
                onclose={modal_onclose.clone()} />
        </div>
    }
}
