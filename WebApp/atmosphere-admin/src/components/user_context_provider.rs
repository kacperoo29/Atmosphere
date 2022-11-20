use yew::prelude::*;
use yew_hooks::{use_async, use_mount};

use crate::{
    models::user::UserInfo,
    services::user::{self, get_token, set_token},
};

#[derive(Properties, Clone, PartialEq)]
pub struct Props {
    pub children: Children,
}

#[function_component(UserContextProvider)]
pub fn user_context_provider(props: &Props) -> Html {
    let user_ctx = use_state(|| None);
    let current_user = use_async(async move { user::current().await });

    {
        let current_user = current_user.clone();
        use_mount(move || {
            if get_token().is_some() {
                current_user.run();
            }
        });
    }

    {
        let user_ctx = user_ctx.clone();
        use_effect_with_deps(
            move |current_user| {
                if let Some(user) = &current_user.data {
                    user_ctx.set(Some(user.clone()))
                }

                if let Some(err) = &current_user.error {
                    match err {
                        crate::error::Error::Unauthorized(_) => {
                            set_token(None);
                        }
                        _ => {}
                    }
                }

                || ()
            },
            current_user.clone(),
        )
    }

    html! {
        <ContextProvider<UseStateHandle<Option<UserInfo>>> context={user_ctx} children={props.children.clone()} />
    }
}
