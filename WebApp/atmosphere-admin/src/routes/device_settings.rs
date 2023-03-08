use atmosphere_api::models::ReadingType;
use web_sys::HtmlInputElement;
use yew::prelude::*;
use yew_hooks::prelude::*;

use crate::components::config::validation_rule::ValidationRule;
use crate::services::config::change_polling_rate;

#[derive(Clone, Debug, PartialEq, Properties)]
pub struct DeviceSettingsProps {
    pub device_id: uuid::Uuid,
}

#[function_component(DeviceSettings)]
pub fn device_settings(props: &DeviceSettingsProps) -> Html {
    let polling_rate = use_state(|| 0);
    let change_polling_rate = {
        let polling_rate = polling_rate.clone();
        let props = props.clone();
        use_async(async move { change_polling_rate(*polling_rate, Some(props.device_id)).await })
    };

    let on_polling_rate_change = {
        let polling_rate = polling_rate.clone();
        Callback::from(move |event: InputEvent| {
            let value = event
                .target_unchecked_into::<HtmlInputElement>()
                .value_as_number() as i32;
            polling_rate.set(value);
        })
    };

    let submit_polling_rate = {
        let change_polling_rate = change_polling_rate.clone();
        Callback::from(move |_| {
            change_polling_rate.run();
        })
    };

    html! {
        <div>
            <h1>{"Device settings"}</h1>
            <ValidationRule reading_type={ReadingType::Temperature} device_id={props.device_id} />
            <ValidationRule reading_type={ReadingType::Humidity} device_id={props.device_id} />
            <div class="form-group">
                <label for="polling-rate">{"Polling rate (ms)"}</label>
                <input type="number" step="1" class="form-control" id="polling-rate" value={(*polling_rate).to_string()} oninput={on_polling_rate_change} />
            </div>
            <button class="btn btn-primary" onclick={submit_polling_rate}>{"Change polling rate"}</button>
        </div>
    }
}
