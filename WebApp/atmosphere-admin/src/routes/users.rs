use yew::prelude::*;
use yew_hooks::prelude::*;
use yew_router::prelude::Link;

use crate::services::user::{get_users_async, remove_user_async};

use super::AppRoute;

#[function_component(Users)]
pub fn users() -> Html {
    let users = use_state(|| vec![]);

    {
        let users = users.clone();
        let get_users = use_async(async move { get_users_async().await });
        {
            let get_users = get_users.clone();
            use_mount(move || {
                get_users.run();
            });
        }

        use_effect_with_deps(
            move |get_users| {
                if let Some(res) = &get_users.data {
                    users.set((*res).clone());
                }

                if let Some(err) = &get_users.error {
                    log::error!("Error getting users: {}", err);
                }
                || ()
            },
            get_users.clone(),
        );
    }
   


    let id_to_remove = use_state(|| "".to_string());            
    let remove_user_async = {
        let id = (*id_to_remove).clone();
        use_async(async move { remove_user_async(id).await })
    };

    let remove_user = {
        let remove_user_async = remove_user_async.clone();
        let id_to_remove = id_to_remove.clone();
        Callback::from(move |id| {
            id_to_remove.set(id);
            remove_user_async.run();
        })
    };

    {
        let users = users.clone();
        let id_to_remove = id_to_remove.clone();
        use_effect_with_deps(
            move |remove_user_async| {
                if let Some(_) = &remove_user_async.data {
                    let mut users_vec = (*users).clone();
                    users_vec.retain(|user| user.id.to_string() != *id_to_remove);
                    users.set(users_vec);
                }

                if let Some(err) = &remove_user_async.error {
                    log::error!("Error removing user: {}", err);
                }
                || ()
            },
            remove_user_async.clone(),
        );
    }

    html! {
        <div>
            <h1>{"Users"}</h1>
            {if users.is_empty() {
                html! {
                    <div>{"No users found"}</div>
                }
            } else {
                html! {
                    <table class="table">
                        <thead>
                            <tr>
                                <th>{"Email"}</th>
                                <th>{"Role"}</th>
                                <th>{"Actions"}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {for (*users).iter().map(|user| {
                                html! {
                                    <tr>
                                        <td>{&user.email}</td>
                                        <td>{&user.role.to_string()}</td>
                                        <td><button onclick={let user = user.clone(); remove_user.reform(move |e: MouseEvent| {
                                            e.prevent_default();
                                            user.id.to_string().clone()
                                        } )} class="btn btn-danger">{"Remove"}</button></td>
                                    </tr>
                                }
                            })}
                        </tbody>
                    </table>
                }
            }
        }
        <Link<AppRoute> to={AppRoute::NewUser} classes="btn btn-primary">{"Add User"}</Link<AppRoute>>
        </div>
    }
}
