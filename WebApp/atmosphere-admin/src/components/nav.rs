use atmosphere_api::models::UserRole;
use wasm_bindgen::{prelude::Closure, JsCast};
use web_sys::MessageEvent;
use yew::prelude::*;
use yew_router::prelude::*;

use crate::{
    bindings::{self, set_on_close, set_send_ping_interval, show_error},
    hooks::use_user_context::use_user_context,
    models::notification_payload::NotificationPayload,
    routes::AppRoute,
    services::websocket::open_notification_socket,
};

#[function_component(Nav)]
pub fn nav() -> Html {
    let user = use_user_context();
    let notification_ws = use_state(|| None::<web_sys::WebSocket>);

    let signout_callback = {
        let user = user.clone();
        let notification_ws = notification_ws.clone();

        Callback::from(move |e: MouseEvent| {
            e.prevent_default();
            user.logout();
            if let Some(ws) = &*notification_ws.clone() {
                if ws.close().is_err() {
                    show_error("Failed to close notification socket");
                }
            }
        })
    };

    {
        let notification_ws = notification_ws.clone();
        use_effect_with_deps(
            move |token| {
                if let Some(token) = token {
                    let ws = open_notification_socket(token.clone());
                    log::info!("ws = {:?}", ws);
                    notification_ws.set(ws);
                }

                || ()
            },
            user.token(),
        );
    }

    use_effect_with_deps(
        move |notification_ws| {
            if let Some(notification_ws) = &*notification_ws.clone() {
                log::info!("Setting onmessage");
                let on_message = Closure::<dyn FnMut(_)>::new(move |e: MessageEvent| {
                    let data = e.data().as_string().unwrap();

                    if let Ok(notification) = serde_json::from_str::<NotificationPayload>(&data) {
                        if notification.r#type.to_lowercase() == "notification" {
                            bindings::notify(
                                "New notification",
                                &notification
                                    .payload
                                    .iter()
                                    .map(|n| n.message.clone())
                                    .collect::<String>(),
                            );
                        }
                    }
                });

                notification_ws.set_onmessage(Some(on_message.as_ref().unchecked_ref()));
                on_message.forget();

                set_send_ping_interval(notification_ws, 20000);
                set_on_close(notification_ws);
            }

            || ()
        },
        notification_ws.clone(),
    );

    let nav_items = match user.info() {
        Some(user) => match user.role {
            UserRole::Admin => {
                html! {
                <>
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
                    <li class="nav-item">
                        <Link<AppRoute> to={AppRoute::Devices} classes="nav-link">
                            <i class="fs-4 bi bi-laptop"></i>
                            <span class="ms-1 d-none d-sm-inline">{"Devices"}</span>
                        </Link<AppRoute>>
                    </li>
                    <li class="nav-item">
                        <Link<AppRoute> to={AppRoute::Users} classes="nav-link">
                            <i class="fs-4 bi bi-people"></i>
                            <span class="ms-1 d-none d-sm-inline">{"Users"}</span>
                        </Link<AppRoute>>
                    </li>
                </>
                }
            }
            _ => {
                html! {
                <li class="nav-item">
                    <Link<AppRoute> to={AppRoute::Home} classes="nav-link">
                        <i class="fs-4 bi bi-house"></i>
                        <span class="ms-1 d-none d-sm-inline">{"Home"}</span>
                    </Link<AppRoute>>
                </li>
                }
            }
        },
        None => {
            html! {
            <li class="nav-item">
                <Link<AppRoute> to={AppRoute::SignIn} classes="nav-link">
                    <i class="fs-4 bi bi-box-arrow-in-right"></i>
                    <span class="ms-1 d-none d-sm-inline">{"Sign In"}</span>
                </Link<AppRoute>>
            </li>
            }
        }
    };

    html! {
        <div class="d-flex flex-column flex-shrink-0 p-3 text-bg-dark vh-100 sticky-top p-3">
            <a href="/" class="d-flex align-items-center mb-3 mb-md-0 me-md-auto text-white text-decoration-none">
                <span class="fs-4">{"Atmosphere Admin"}</span>
            </a>
            <hr />
            <ul class="nav nav-pills flex-column mb-auto" id="menu">
                {nav_items}
            </ul>
            <hr />
            if user.is_logged_in() {
                <div onclick={signout_callback}>
                    <i class="fs-4 bi bi-box-arrow-left"></i>
                    <span class="ms-1 d-none d-sm-inline">{"Sign Out"}</span>
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
