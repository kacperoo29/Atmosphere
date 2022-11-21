use atmosphere_api::models::ReadingDto;
use chrono::{DateTime, Local};
use yew::{function_component, html, Properties};

#[derive(Properties, Clone, PartialEq)]
pub struct Props {
    pub reading_dto: ReadingDto,
}

#[function_component(ReadingListEntry)]
pub fn reading_list_entry(props: &Props) -> Html {
    let reading = &props.reading_dto;
    let date = DateTime::parse_from_rfc3339(&reading.timestamp).unwrap();
    let date = date.with_timezone(&Local);

    // convert sensor address from xxxxxxxxxxxx to xx:xx:xx:xx:xx:xx
    let sensor_address = reading
        .sensor_identifier
        .chars()
        .enumerate()
        .map(|(i, c)| {
            if i % 2 == 0 && i != 0 {
                format!(":{}", c)
            } else {
                c.to_string()
            }
        })
        .collect::<String>();

    html! {
        <div class="card">
            <div class="card-body">
                <h5 class="card-title">{format!("Reading id: {}", &reading.id)}</h5>
                <h6 class="card-subtitle mb-2 text-muted">{format!("Device id: {}", &reading.device_id)}</h6>
                <p class="card-text">{format!("Sensor address: {}", &sensor_address)}</p>
                <p class="card-text">{format!("Value: {}", &reading.value)}</p>
                <p class="card-text">{format!("Type: {}", &reading.r#type.to_string())}</p>
                <p class="card-text">{format!("Date taken: {}", date.naive_local())}</p>
            </div>
        </div>
    }
}
