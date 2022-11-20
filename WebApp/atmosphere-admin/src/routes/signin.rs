use web_sys::HtmlInputElement;
use yew::prelude::*;
use yew_hooks::use_async;

use crate::{
    error::Error, hooks::use_user_context::use_user_context, models::user::LoginInfo,
    services::user::authenticate,
};

#[function_component(SignIn)]
pub fn signin() -> Html {
    let user = use_user_context();
    let login_info = use_state(LoginInfo::default);
    let user_login = {
        let login_info = login_info.clone();
        use_async(async move { authenticate((*login_info).clone()).await })
    };

    let login_error = (&user_login).error.clone().map(|err| match err {
        Error::Unauthorized(msg) => msg,
        _ => "Unknown error".to_string(),
    });

    {
        let user = user.clone();
        use_effect_with_deps(
            move |user_login| {
                if let Some(user_info) = &user_login.data {
                    user.login((*user_info).clone());
                }

                if let Some(err) = &user_login.error {
                    match err {
                        Error::Unauthorized(str) => {
                            log::info!("User not logged in: {}", str);
                        }
                        _ => {
                            log::error!("Error logging in: {:?}", err);
                        }
                    }
                }

                || ()
            },
            user_login.clone(),
        );
    }

    let on_submit = {
        let user_login = user_login.clone();
        Callback::from(move |e: FocusEvent| {
            e.prevent_default();
            user_login.run();
        })
    };

    let on_username_change = {
        let login_info = login_info.clone();
        Callback::from(move |e: InputEvent| {
            let input: HtmlInputElement = e.target_unchecked_into();
            let mut info = (*login_info).clone();
            info.username = input.value();
            login_info.set(info);
        })
    };

    let on_password_change = {
        let login_info = login_info.clone();
        Callback::from(move |e: InputEvent| {
            let input: HtmlInputElement = e.target_unchecked_into();
            let mut info = (*login_info).clone();
            info.password = input.value();
            login_info.set(info);
        })
    };

    if user.clone().is_logged_in() {
        html! {
            <div>
                <h1>{"You are logged in!"}</h1>
            </div>
        }
    } else {
        html! {
            <div class="container">
                <div class="row">
                    <div class="col-md-6 offset-md-3 col-xs-12">
                        <h1 class="text-xs-center">{ "Sign In" }</h1>
                        <form onsubmit={on_submit}>
                            <fieldset class="form-group">
                                <input
                                    class="form-control form-control-lg"
                                    type="text"
                                    placeholder="Username"
                                    value={login_info.username.clone()}
                                    oninput={on_username_change}
                                />
                            </fieldset>
                            <fieldset class="form-group">
                                <input
                                    class="form-control form-control-lg"
                                    type="password"
                                    placeholder="Password"
                                    value={login_info.password.clone()}
                                    oninput={on_password_change}
                                />
                            </fieldset>
                            <button
                                class="btn btn-lg btn-primary pull-xs-right"
                                type="submit"
                                disabled={user_login.loading}
                            >
                                { "Sign in" }
                            </button>
                        </form>

                        if let Some(error) = login_error {
                            <div class="alert alert-danger mt-2" role="alert">
                                { error }
                            </div>
                        }
                    </div>
                </div>
            </div>
        }
    }
}
