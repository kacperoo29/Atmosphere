use atmosphere_api::models::ReadingDtoPagedList;
use web_sys::HtmlInputElement;
use yew::prelude::*;
use yew_hooks::prelude::*;

use crate::bindings::show_error;
use crate::components::pagination::Pagination;
use crate::components::reading::reading_list_entry::ReadingListEntry;

use crate::services::reading::{get_readings_by_date, get_readings_by_device};

#[derive(Clone, PartialEq, Properties)]
pub struct ReadingListProps {
    pub device_id: Option<String>,
}

#[function_component(ReadingList)]
pub fn reading_list(props: &ReadingListProps) -> Html {
    let readings: UseStateHandle<Option<ReadingDtoPagedList>> = use_state(|| None);
    let start_date = use_state(|| None);
    let end_date = use_state(|| None);
    let current_page = use_state(|| 1);
    let page_size = use_state(|| 50);

    let get_readings = {
        let start_date = start_date.clone();
        let end_date = end_date.clone();
        let current_page = current_page.clone();
        let page_size = page_size.clone();
        let props = props.clone();
        use_async(async move {
            if let Some(device_id) = &props.device_id {
                return get_readings_by_device(
                    *current_page,
                    *page_size,
                    device_id,
                    (*start_date).clone(),
                    (*end_date).clone(),
                )
                .await;
            } else {
                get_readings_by_date(
                    *current_page,
                    *page_size,
                    (*start_date).clone(),
                    (*end_date).clone(),
                )
                .await
            }
        })
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

    let change_page = {
        let current_page = current_page.clone();
        let get_readings = get_readings.clone();
        Callback::from(move |page: i32| {
            current_page.set(page);
            get_readings.run();
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

    let has_next = readings.is_some() && readings.as_ref().unwrap().has_next;
    let has_previous = readings.is_some() && readings.as_ref().unwrap().has_previous;
    let total_pages = readings.as_ref().map(|r| r.total_pages).unwrap_or(0);

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
                        readings.unwrap().items.iter().map(|reading|  {
                            html! {<ReadingListEntry reading_dto={reading.clone()} />}
                        }).collect::<Html>()
                    }
                }
                </tbody>
            </table>
            <Pagination
                current_page={*current_page}
                page_size={*page_size}
                total_pages={total_pages}
                has_next_page={has_next}
                has_previous_page={has_previous}
                on_page_change={change_page} />
        </div>
    }
}
