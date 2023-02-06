use crate::services::config;
use atmosphere_api::models::EmailConfiguration;
use web_sys::{HtmlInputElement, InputEvent};
use yew::{function_component, html, use_effect_with_deps, use_state, Callback, TargetCast};
use yew_hooks::{use_async, use_mount};

#[function_component(EmailConfigForm)]
pub fn email_configuration() -> Html {
    let config = use_state(|| EmailConfiguration::default());

    {
        let config = config.clone();
        let get_config = use_async(async move { config::get_email_config().await });

        {
            let get_config = get_config.clone();
            use_mount(move || get_config.run());
        }

        let get_config = get_config.clone();
        let config = config.clone();
        use_effect_with_deps(
            move |get_config| {
                if let Some(res) = &get_config.data {
                    config.set((*res).clone());
                }

                if let Some(err) = &get_config.error {
                    log::error!("Error getting email config: {}", err);
                }
                || ()
            },
            get_config.clone(),
        );
    }

    let update_config = {
        let config = config.clone();
        use_async(async move {
            return config::update_email_config((*config).clone()).await;
        })
    };

    let submit_config = {
        let update_config = update_config.clone();
        Callback::from(move |_| {
            update_config.run();
        })
    };

    let on_server_change = {
        let config = config.clone();
        Callback::from(move |e: InputEvent| {
            let value = e.target_unchecked_into::<HtmlInputElement>().value();
            if value.is_empty() {
                config.set(EmailConfiguration {
                    smtp_server: None,
                    ..(*config).clone()
                })
            } else {
                config.set(EmailConfiguration {
                    smtp_server: Some(Some(value)),
                    ..(*config).clone()
                })
            }
        })
    };

    let on_port_change = {
        let config = config.clone();
        Callback::from(move |e: InputEvent| {
            let value = e.target_unchecked_into::<HtmlInputElement>().value();
            if value.is_empty() {
                config.set(EmailConfiguration {
                    smtp_port: None,
                    ..(*config).clone()
                })
            } else {
                config.set(EmailConfiguration {
                    smtp_port: Some(Some(value.parse::<i32>().unwrap())),
                    ..(*config).clone()
                })
            }
        })
    };

    let on_username_change = {
        let config = config.clone();
        Callback::from(move |e: InputEvent| {
            let value = e.target_unchecked_into::<HtmlInputElement>().value();
            if value.is_empty() {
                config.set(EmailConfiguration {
                    smtp_username: None,
                    ..(*config).clone()
                })
            } else {
                config.set(EmailConfiguration {
                    smtp_username: Some(Some(value)),
                    ..(*config).clone()
                })
            }
        })
    };

    let on_password_change = {
        let config = config.clone();
        Callback::from(move |e: InputEvent| {
            let value = e.target_unchecked_into::<HtmlInputElement>().value();
            if value.is_empty() {
                config.set(EmailConfiguration {
                    smtp_password: None,
                    ..(*config).clone()
                })
            } else {
                config.set(EmailConfiguration {
                    smtp_password: Some(Some(value)),
                    ..(*config).clone()
                })
            }
        })
    };

    let on_email_change = {
        let config = config.clone();
        Callback::from(move |e: InputEvent| {
            let value = e.target_unchecked_into::<HtmlInputElement>().value();
            if value.is_empty() {
                config.set(EmailConfiguration {
                    email_address: None,
                    ..(*config).clone()
                })
            } else {
                config.set(EmailConfiguration {
                    email_address: Some(Some(value)),
                    ..(*config).clone()
                })
            }
        })
    };

    let on_server_email_change = {
        let config = config.clone();
        Callback::from(move |e: InputEvent| {
            let value = e.target_unchecked_into::<HtmlInputElement>().value();
            if value.is_empty() {
                config.set(EmailConfiguration {
                    server_email_address: None,
                    ..(*config).clone()
                })
            } else {
                config.set(EmailConfiguration {
                    server_email_address: Some(Some(value)),
                    ..(*config).clone()
                })
            }
        })
    };

    html! {
        <div class="mt-4 mb-4">
            <h3>{ "Email Configuration" }</h3>
            <div class="form">
                <div class="form-group">
                    <label for="server">{ "SMTP Server" }</label>
                    <input type="text" class="form-control" id="server" placeholder="smtp.gmail.com" value={(*config).smtp_server.clone().unwrap_or(None).unwrap_or("".to_string())} oninput={on_server_change.clone()} />
                </div>
                <div class="form-group">
                    <label for="port">{ "SMTP Port" }</label>
                    <input type="number" class="form-control" id="port" placeholder="587" value={(*config).smtp_port.clone().unwrap_or(None).unwrap_or(0).to_string()} oninput={on_port_change.clone()} />
                </div>
                <div class="form-group">
                    <label for="username">{ "SMTP Username" }</label>
                    <input type="text" class="form-control" id="username" placeholder="username" value={(*config).smtp_username.clone().unwrap_or(None).unwrap_or("".to_string())} oninput={on_username_change.clone()} />
                </div>
                <div class="form-group">
                    <label for="password">{ "SMTP Password" }</label>
                    <input type="password" class="form-control" id="password" placeholder="password" value={(*config).smtp_password.clone().unwrap_or(None).unwrap_or("".to_string())} oninput={on_password_change.clone()} />
                </div>
                <div class="form-group">
                    <label for="email">{ "Destination Email Address" }</label>
                    <input type="email" class="form-control" id="email" placeholder=""
                        value={(*config).email_address.clone().unwrap_or(None).unwrap_or("".to_string())} oninput={on_email_change.clone()} />
                </div>
                <div class="form-group">
                    <label for="server_email">{ "Server Email Address" }</label>
                    <input type="email" class="form-control" id="server_email" placeholder=""
                        value={(*config).server_email_address.clone().unwrap_or(None).unwrap_or("".to_string())} oninput={on_server_email_change.clone()} />
                </div>
                <button type="submit" class="btn btn-primary" onclick={submit_config.clone()}>{ "Submit" }</button>
            </div>
        </div>
    }
}
