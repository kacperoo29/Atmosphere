use web_sys::HtmlInputElement;
use yew::prelude::*;
use yew_hooks::prelude::*;

use crate::services::user::create_user_async;

#[function_component(CreateUser)]
pub fn create_user() -> Html {
    let email = use_state(|| "".to_string());
    let password = use_state(|| "".to_string());

    let create = {
        let email = email.clone();
        let password = password.clone();

        use_async(async move {
            return create_user_async((*email).clone(), (*password).clone()).await;
        })
    };

    let change_username = {
        let email = email.clone();
        Callback::from(move |e: InputEvent| {
            email.set(e.target_unchecked_into::<HtmlInputElement>().value());
        })
    };

    let change_password = {
        let password = password.clone();
        Callback::from(move |e: InputEvent| {
            password.set(e.target_unchecked_into::<HtmlInputElement>().value());
        })
    };

    let submit = {
        Callback::from(move |_| {
            create.run();
        })
    };

    html! {
        <div class="container">
            <h1>{"Create User"}</h1>
            <div class="form col-md-6">

                <div class="form-group">
                    <label for="email">{"Email"}</label>
                    <input class="form-control" type="text" id="email" name="email" oninput={change_username} />
                </div>

                <div class="form-group">
                    <label for="password">{"Password"}</label>
                    <input class="form-control" type="password" id="password" name="password" oninput={change_password} />
                </div>

                <button class="btn btn-primary" onclick={submit}>{"Create"}</button>
            </div>
        </div>
    }
}
