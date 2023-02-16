use atmosphere_api::models::ReadingDto;
use web_sys::HtmlInputElement;
use yew::prelude::*;
use yew_hooks::prelude::*;

use crate::bindings::show_error;
use crate::components::reading::reading_list_entry::ReadingListEntry;

use crate::services::reading::get_readings_by_date;

#[function_component(ReadingList)]
pub fn reading_list() -> Html {
    let readings: UseStateHandle<Option<Vec<ReadingDto>>> = use_state(|| None);
    let start_date = use_state(|| None);
    let end_date = use_state(|| None);
    let get_readings = {
        let start_date = start_date.clone();
        let end_date = end_date.clone();
        use_async(
            async move { get_readings_by_date((*start_date).clone(), (*end_date).clone()).await },
        )
    };

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

                if let Some(err) = &get_readings.error {
                    show_error(&err.to_string());
                }

                || ()
            },
            get_readings.clone(),
        );
    }

    let readings = (*readings).clone();

    let change_start_date = {
        let start_date = start_date.clone();
        Callback::from(move |e: InputEvent| {
            start_date.set(Some(e.target_unchecked_into::<HtmlInputElement>().value()));
        })
    };

    let change_end_date = {
        let end_date = end_date.clone();
        Callback::from(move |e: InputEvent| {
            end_date.set(Some(e.target_unchecked_into::<HtmlInputElement>().value()));
        })
    };

    let get_readings = {
        let get_readings = get_readings.clone();
        Callback::from(move |_| {
            get_readings.run();
        })
    };

    let reset_filters = {
        let start_date = start_date.clone();
        let end_date = end_date.clone();
        Callback::from(move |_| {
            start_date.set(None);
            end_date.set(None);
        })
    };

    html! {
        <div>
            <div>
                <label>{"Start date"}</label>
                <input type="datetime-local"
                    class="form-control"
                    value={((*start_date).clone()).unwrap_or("".to_string())}
                    oninput={change_start_date} />
                <label>{"End date"}</label>
                <input type="datetime-local"
                    class="form-control"
                    value={((*end_date).clone()).unwrap_or("".to_string())}
                    oninput={change_end_date} />
                <button class="btn btn-primary" onclick={get_readings}>{"Apply filters"}</button>
                <button class="btn btn-primary" onclick={reset_filters}>{"Reset filters"}</button>
            </div>
            <h1>{"Reading List"}</h1>
            <table class="table">
                <thead>
                    <tr>
                        <th>{"Date"}</th>
                        <th>{"Type"}</th>
                        <th>{"Value"}</th>
                        <th>{"Unit"}</th>
                        <th>{"Device name"}</th>
                    </tr>
                </thead>
                <tbody>
                if readings.is_some() {
                    {
                        readings.unwrap().iter().map(|reading|  {
                            html! {<ReadingListEntry reading_dto={reading.clone()} />}
                        }).collect::<Html>()
                    }
                }
                </tbody>
            </table>
        </div>
    }
}
