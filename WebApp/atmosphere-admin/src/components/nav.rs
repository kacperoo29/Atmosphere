use yew::prelude::*;
use yew_router::prelude::*;

use crate::{hooks::use_user_context::use_user_context, routes::AppRoute};

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

    html! {
        <div class="col-auto col-md-3 col-xl-2 px-sm-2 px-0 bg-dark">
            <div class="d-flex flex-column align-items-center align-items-sm-start px-3 pt-2 text-white min-vh-100">
                <a href="/" class="d-flex align-items-center pb-3 mb-md-0 me-md-auto text-white text-decoration-none">
                    <span class="fs-5 d-none d-sm-inline">{"Atmosphere Admin"}</span>
                </a>
                <hr />
                <ul class="nav nav-pills flex-column mb-sm-auto mb-0 align-items-center align-items-sm-start" id="menu">
                    <li class="nav-item">
                        <Link<AppRoute> to={AppRoute::Home} classes="nav-link align-middle px-0">
                            <i class="fs-4 bi bi-house"></i>
                            <span class="ms-1 d-none d-sm-inline">{"Home"}</span>
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
                    <Link<AppRoute> to={AppRoute::SignIn} classes="nav-link align-middle px-0">
                        <i class="fs-4 bi bi-box-arrow-in-right"></i>
                        <span class="ms-1 d-none d-sm-inline">{"Sign In"}</span>
                    </Link<AppRoute>>
                    <hr />
                }
            </div>
        </div>
    }
}
