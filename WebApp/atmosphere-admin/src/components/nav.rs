use wasm_bindgen::{prelude::Closure, JsCast};
use web_sys::MessageEvent;
use yew::prelude::*;
use yew_router::prelude::*;

use crate::{hooks::use_user_context::use_user_context, routes::AppRoute, services::websocket::open_notification_socket, bindings, models::notification_payload::NotificationPayload};

/// Nav component
#[function_component(Nav)]
pub fn nav() -> Html {
    let user = use_user_context();

    let signout_callback = {
        let user = user.clone();

        Callback::from(move |e: MouseEvent| {
            e.prevent_default();
            user.logout();
        })
    };

    use_effect_with_deps(
        |_user| {
            if (*_user).is_some() {
                let notification_ws = open_notification_socket().unwrap();
                let on_message = Closure::<dyn FnMut(_)>::new(move |e: MessageEvent| {
                    let data = e.data().as_string().unwrap();

                    // deserialize data
                    let notification: NotificationPayload = serde_json::from_str(&data).unwrap();
                    if notification.r#type.to_lowercase() == "notification" {
                        bindings::notify("New notification", &notification.data.join(" "));
                    }
                });

                notification_ws.set_onmessage(Some(on_message.as_ref().unchecked_ref()));
                on_message.forget();
            }

            || ()
        },
        (*user).clone(),
    );

    html! {
        <div class="d-flex flex-column flex-shrink-0 p-3 text-bg-dark vh-100 sticky-top p-3">
            <a href="/" class="d-flex align-items-center mb-3 mb-md-0 me-md-auto text-white text-decoration-none">
                <span class="fs-4">{"Atmosphere Admin"}</span>
            </a>
            <hr />
            <ul class="nav nav-pills flex-column mb-auto" id="menu">
                <li class="nav-item">
                    <Link<AppRoute> to={AppRoute::Home} classes="nav-link">
                        <i class="fs-4 bi bi-house"></i>
                        <span class="ms-1 d-none d-sm-inline">{"Home"}</span>
                    </Link<AppRoute>>
                </li>
                <li class="nav-item">
                    <Link<AppRoute> to={AppRoute::Settings} classes="nav-link">
                        <i class="fs-4 bi bi-gear"></i>
                        <span class="ms-1 d-none d-sm-inline">{"Settings"}</span>
                    </Link<AppRoute>>
                </li>
            </ul>
            <hr />
            if user.is_logged_in() {
                <div class="dropdown pb-4">
                    <a href="#" class="d-flex align-items-center text-white text-decoration-none dropdown-toggle" id="dropdownUser1" data-bs-toggle="dropdown" aria-expanded="false">
                        <span>{user.as_ref().unwrap().username.clone()}</span>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-dark text-small shadow" aria-labelledby="dropdownUser1">
                        <li><a class="dropdown-item">{"Settings"}</a></li>
                        <li><hr class="dropdown-divider" /></li>
                        <li><a class="dropdown-item" onclick={signout_callback}>{"Sign out"}</a></li>
                    </ul>
                </div>
            } else {
                <Link<AppRoute> to={AppRoute::SignIn} classes="nav-link">
                    <i class="fs-4 bi bi-box-arrow-in-right"></i>
                    <span class="ms-1 d-none d-sm-inline">{"Sign In"}</span>
                </Link<AppRoute>>
            }
        </div>
    }
}
