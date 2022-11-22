use web_sys::HtmlInputElement;
use yew::prelude::*;
use yew_hooks::{use_async, use_mount};
use yew_router::prelude::*;

use super::AppRoute;
use crate::{
    components::modal, hooks::use_user_context::use_user_context,
    models::config::NotificationConfig, services::config,
};

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

    let notification_settings = use_state(|| NotificationConfig::default());
    let save_notification_settings = {
        let notification_settings = notification_settings.clone();
        use_async(async move {
            config::set_notifications_settings((*notification_settings).clone()).await
        })
    };
    let get_notification_settings =
        use_async(async move { config::get_notifications_settings().await });

    {
        let settings = get_notification_settings.clone();
        use_mount(move || settings.run());
    }

    {
        let modal_props = modal_props.clone();
        let modal_onclose = modal_onclose.clone();
        let settings = notification_settings.clone();
        use_effect_with_deps(
            move |get_notification_settings| {
                if let Some(res) = &get_notification_settings.data {
                    settings.set((*res).clone());
                }

                if let Some(err) = &get_notification_settings.error {
                    modal_props.set(modal::Props {
                        hidden: false,
                        title: "Error".to_string(),
                        message: format!("Error: {}", err),
                        onclose: modal_onclose.clone(),
                    });
                }

                || ()
            },
            get_notification_settings.clone(),
        );
    }

    let settings = (*notification_settings).clone();

    {
        let modal_props = modal_props.clone();
        let modal_onclose = modal_onclose.clone();
        use_effect_with_deps(
            move |notification_settings| {
                if let Some(_) = &notification_settings.data {
                    modal_props.set(modal::Props {
                        hidden: false,
                        title: "Success".to_string(),
                        message: "Notification settings saved".to_string(),
                        onclose: modal_onclose.clone(),
                    });
                }

                if let Some(err) = &notification_settings.error {
                    modal_props.set(modal::Props {
                        hidden: false,
                        title: "Error".to_string(),
                        message: format!("Error saving notification settings: {:?}", err),
                        onclose: modal_onclose.clone(),
                    });
                }

                || ()
            },
            save_notification_settings.clone(),
        );
    }

    let on_email_enabled_change = {
        let notification_settings = notification_settings.clone();
        Callback::from(move |e: InputEvent| {
            let checkbox: HtmlInputElement = e.target_unchecked_into();
            let mut settings = (*notification_settings).clone();
            settings.base.email_enabled = checkbox.checked();

            notification_settings.set(settings);
        })
    };

    let on_email_change = {
        let notification_settings = notification_settings.clone();
        Callback::from(move |e: InputEvent| {
            let input: HtmlInputElement = e.target_unchecked_into();
            let mut settings = (*notification_settings).clone();
            settings.base.email = input.value();

            notification_settings.set(settings);
        })
    };

    let on_temperature_min_change = {
        let notification_settings = notification_settings.clone();
        Callback::from(move |e: InputEvent| {
            let input: HtmlInputElement = e.target_unchecked_into();
            let mut settings = (*notification_settings).clone();
            settings.temperature.min = input.value().parse().unwrap_or(0.0);

            notification_settings.set(settings);
        })
    };

    let on_temperature_max_change = {
        let notification_settings = notification_settings.clone();
        Callback::from(move |e: InputEvent| {
            let input: HtmlInputElement = e.target_unchecked_into();
            let mut settings = (*notification_settings).clone();
            settings.temperature.max = input.value().parse().unwrap_or(0.0);

            notification_settings.set(settings);
        })
    };

    let on_notification_submit = {
        Callback::from(move |e: FocusEvent| {
            e.prevent_default();
            save_notification_settings.run();
        })
    };

    html! {
        <div>
            <h1>{ "Settings" }</h1>
            <form onsubmit={on_notification_submit}>
                <h3>{ "Notifications" }</h3>
                <div class="form-check mb-3">
                    <input class="form-check-input" type="checkbox" id="notify-email"
                        checked={settings.base.email_enabled} oninput={on_email_enabled_change} />
                    <label class="form-check-label" for="notify">
                        { "Enable email notifications" }
                    </label>
                </div>
                <div class="mb-3">
                    <label for="notify-email" class="form-label">
                        { "Email address" }
                    </label>
                    <input type="email" class="form-control" id="notify-email"
                        value={settings.base.email.clone()} oninput={on_email_change}
                        disabled={!settings.base.email_enabled} />
                    <div id="notify-email-help" class="form-text">
                        { "Enter email to which the notifications will be sent." }
                    </div>
                </div>
                <h3>{ "Temperature" }</h3>
                <div class="mb-3">
                    <label for="notify-temperature-min" class="form-label">
                        { "Minimum temperature" }
                    </label>
                    <input type="number" class="form-control" id="notify-temperature-min" step="0.1"
                        value={settings.temperature.min.to_string()} oninput={on_temperature_min_change} />
                    <div id="notify-temperature-min-help" class="form-text">
                        { "Minimum temperature to trigger a notification." }
                    </div>
                </div>
                <div class="mb-3">
                    <label for="notify-temperature-max" class="form-label">
                        { "Maximum temperature" }
                    </label>
                    <input type="number" class="form-control" id="notify-temperature-max" step="0.1"
                        value={settings.temperature.max.to_string()} oninput={on_temperature_max_change} />
                    <div id="notify-temperature-max-help" class="form-text">
                        { "Maximum temperature to trigger a notification." }
                    </div>
                </div>
                <div>
                    <button class="btn btn-primary" type="submit">
                        { "Save" }
                    </button>
                </div>
            </form>

            <modal::Modal hidden={modal_props.hidden.clone()}
                title={modal_props.title.clone()}
                message={modal_props.message.clone()}
                onclose={modal_onclose.clone()} />
        </div>
    }
}
