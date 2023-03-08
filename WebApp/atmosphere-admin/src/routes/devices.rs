use yew::prelude::*;
use yew_hooks::prelude::*;
use yew_router::prelude::*;

use crate::routes::AppRoute;

use crate::{
    bindings::show_error,
    services::device::{activate_device, get_all_devices},
};

#[function_component(Devices)]
pub fn devices() -> Html {
    let devices = use_state(|| vec![]);
    let get_devices = { use_async(async move { get_all_devices().await }) };

    {
        let devices = devices.clone();
        use_effect_with_deps(
            move |get_devices| {
                if let Some(res) = &get_devices.data {
                    devices.set((*res).clone());
                }

                if let Some(err) = &get_devices.error {
                    show_error(&err.to_string());
                }

                || ()
            },
            get_devices.clone(),
        );
    }

    {
        let get_devices = get_devices.clone();
        use_mount(move || {
            get_devices.run();
        });
    }

    let devid = use_state(|| None::<String>);
    let activate_device_fn = {
        let devid = devid.clone();
        use_async(async move {
            if let Some(id) = &*devid {
                {
                    activate_device(id.clone()).await
                }
            } else {
                {
                    Ok(())
                }
            }
        })
    };

    {
        let activate_device_fn = activate_device_fn.clone();
        let get_devices = get_devices.clone();
        use_effect_with_deps(
            move |activate_device_fn| {
                if let Some(_) = &activate_device_fn.data {
                    get_devices.run();
                }

                if let Some(err) = &activate_device_fn.error {
                    show_error(&err.to_string());
                }

                || ()
            },
            activate_device_fn.clone(),
        );
    }

    let activate_device_cb = {
        Callback::from(move |id: String| {
            devid.set(Some(id));
            activate_device_fn.run();
        })
    };

    html! {
        <div>
            <h1>{"Devices"}</h1>
            <table class="table">
                <thead>
                    <tr>
                        <th>{"Identifier"}</th>
                        <th>{"Active"}</th>
                        <th>{"Connected"}</th>
                        <th>{"Actions"}</th>
                    </tr>
                </thead>
                <tbody>
                    {for (*devices.clone()).iter().map(|device| {
                        let device = device.clone();
                        html! {
                        <tr>
                            <td>{&device.identifier}</td>
                            <td><input type="checkbox" checked={device.is_active}
                                onchange={let device = device.clone(); activate_device_cb.reform(move |_| device.clone().id.to_string())} /></td>
                            <td><input type="checkbox" checked={device.is_connected} disabled=true /></td>
                            <td>
                                <Link<AppRoute> classes="btn btn-primary mr-2" to={AppRoute::DeviceReadings { id: device.id.to_string().clone() }}>
                                    {"View readings"}
                                </Link<AppRoute>>
                                <Link<AppRoute> classes="btn btn-primary mr-2" to={AppRoute::DeviceChart { id: device.id.to_string().clone() }}>
                                    {"View charts"}
                                </Link<AppRoute>>
                                <Link<AppRoute> classes="btn btn-primary mr-2" to={AppRoute::DeviceSettings { id: device.id.to_string().clone() }}>
                                    {"Settings"}
                                </Link<AppRoute>>
                            </td>
                        </tr>
                    }})}
                </tbody>
            </table>
        </div>
    }
}
