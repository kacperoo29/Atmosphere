use std::collections::HashMap;

use atmosphere_api::models::ReadingType;
use yew::prelude::*;
use yew_hooks::prelude::*;

use crate::{bindings::draw_chart, services::reading::get_chart_data, to_title_case};

static CHART_TYPES: &[ReadingType] = &[ReadingType::Temperature, ReadingType::Humidity];

#[derive(Clone, PartialEq, Properties)]
pub struct ChartProps {
    pub device_id: Option<uuid::Uuid>,
    pub canvas_id: String,
}

#[function_component(Chart)]
pub fn chart(props: &ChartProps) -> Html {
    let chart_data = use_state(|| None::<HashMap<String, HashMap<String, f64>>>);
    let complete: UseStateHandle<Vec<ReadingType>> = use_state(|| Vec::new());
    let real_complete: UseStateHandle<Vec<ReadingType>> = use_state(|| Vec::new());

    let mut fetch_chart_data = HashMap::new();
    for reading_type in CHART_TYPES {
        let props = props.clone();
        fetch_chart_data.insert(
            reading_type,
            use_async(
                async move { get_chart_data(props.device_id, *reading_type, None, None).await },
            ),
        );
    }

    {
        let fetch_chart_data = fetch_chart_data.clone();
        use_mount(move || {
            for fetch_data in fetch_chart_data.values() {
                fetch_data.run();
            }
        });
    }

    {
        let complete = complete.clone();
        let chart_data = chart_data.clone();
        let fetch_chart_data = fetch_chart_data.clone();
        use_effect_with_deps(
            move |fetch_chart_data| {
                for (reading_type, fetch_data) in fetch_chart_data {
                    if complete.contains(reading_type) {
                        continue;
                    }

                    if let Some(res) = &fetch_data.data {
                        if let Some(data) = &*chart_data {
                            let mut data = data.clone();
                            data.insert(reading_type.to_string(), res.clone());
                            chart_data.set(Some(data));
                        } else {
                            let mut data = HashMap::new();
                            data.insert(reading_type.to_string(), res.clone());
                            chart_data.set(Some(data));
                        }

                        let mut complete_data = (*complete).clone();
                        complete_data.push(**reading_type);
                        complete.set(complete_data);
                    }
                }

                || ()
            },
            fetch_chart_data.clone(),
        );
    }

    {
        let chart_data = chart_data.clone();
        let props = props.clone();
        let complete = complete.clone();
        use_effect_with_deps(
            move |complete| {
                for reading_type in CHART_TYPES {
                    if complete.contains(reading_type) && !real_complete.contains(reading_type) {
                        if let Some(data) = &*chart_data {
                            let canvas_id =
                                format!("{}-{}", props.canvas_id, reading_type.to_string());
                            let value =
                                serde_wasm_bindgen::to_value(&data.get(&reading_type.to_string()))
                                    .unwrap();
                            draw_chart(&canvas_id, &value, &reading_type.to_string());

                            let mut real_complete_data = (*real_complete).clone();
                            real_complete_data.push(*reading_type);
                            real_complete.set(real_complete_data);
                        }
                    }
                }

                || ()
            },
            complete.clone(),
        );
    }

    html! {
        <>
            <h1>{"Chart"}</h1>
            <div class="d-flex flex-wrap justify-content-center">
            {
                for CHART_TYPES.iter().map(|reading_type| {
                    html! {
                        <div class="chart-container d-flex flex-wrap" style="width: 40vw">
                            <h3>{to_title_case(&reading_type.to_string())}</h3>
                            <canvas id={format!("{}-{}", props.canvas_id, reading_type.to_string())}></canvas>
                        </div>
                    }
                })
            }
            </div>
        </>
    }
}
