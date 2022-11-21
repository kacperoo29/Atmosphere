use atmosphere_api::models::ReadingDto;
use yew::prelude::*;
use yew_hooks::prelude::*;

use crate::components::reading::reading_list_entry::ReadingListEntry;

use crate::services::reading::get_readings;

#[function_component(ReadingList)]
pub fn reading_list() -> Html {
    let readings: UseStateHandle<Option<Vec<ReadingDto>>> = use_state(|| None);
    let get_readings = use_async(async move { get_readings().await });

    {
        let get_readings = get_readings.clone();
        use_mount(move || {
            get_readings.run();
        });
    }

    {
        let readings = readings.clone();
        use_effect_with_deps(
            move |get_readings| {
                if let Some(res) = &get_readings.data {
                    readings.set(Some((*res).clone()));
                }

                || ()
            },
            get_readings.clone(),
        );
    }

    let readings = (*readings).clone();

    html! {
        <div>
            <h1>{"Reading List"}</h1>
            if readings.is_some() {
                {
                    readings.unwrap().iter().map(|reading|  {
                        html! {<ReadingListEntry reading_dto={reading.clone()} />}
                    }).collect::<Html>()
                }
            }
        </div>
    }
}
