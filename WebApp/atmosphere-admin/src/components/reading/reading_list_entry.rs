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

    // // convert sensor address from xxxxxxxxxxxx to xx:xx:xx:xx:xx:xx
    // let sensor_address = reading
    //     .sensor_identifier
    //     .chars()
    //     .enumerate()
    //     .map(|(i, c)| {
    //         if i % 2 == 0 && i != 0 {
    //             format!(":{}", c)
    //         } else {
    //             c.to_string()
    //         }
    //     })
    //    .collect::<String>();

    html! {
        <tr>
            <td>{date.format("%Y-%m-%d %H:%M:%S").to_string()}</td>
            <td>{&reading.r#type.to_string()}</td>
            <td>{&reading.value}</td>
            <td>{&reading.unit}</td>
            <td>{&reading.device.identifier}</td>
        </tr>
    }
}
