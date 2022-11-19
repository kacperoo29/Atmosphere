use yew::prelude::*;
use yew_hooks::{use_async, use_mount};

use crate::{
    models::user::UserInfo,
    services::user,
};

#[derive(Properties, Clone, PartialEq)]
pub struct Props {
    pub children: Children,
}

#[function_component(UserContextProvider)]
pub fn user_context_provider(props: &Props) -> Html {
    let user_ctx = use_state(|| UserInfo::default());
    let current_user = use_async(async move { user::current().await });

    {
        let current_user = current_user.clone();
        use_mount(move || {
            if current_user.data.is_some() {
                current_user.run();
            }
        });
    }

    {
        let user_ctx = user_ctx.clone();
        use_effect_with_deps(
            move |current_user| {
                if let Some(user) = &current_user.data {
                    user_ctx.set(user.clone())
                }

                || ()
            },
            current_user.clone(),
        )
    }

    html! {
        <ContextProvider<UseStateHandle<UserInfo>> context={user_ctx} children={props.children.clone()} />
    }
}
