use std::collections::HashMap;

use atmosphere_api::{
    apis::reading_api::{
        api_reading_get_chart_data_post, api_reading_get_paged_readings_by_date_get,
        api_reading_get_paged_readings_by_device_get,
    },
    models::{reading_type, GetChartData, ReadingDtoPagedList},
};

use crate::error::Error;

use super::get_config;

pub async fn get_readings_by_date(
    current_page: i32,
    page_size: i32,
    start_date: Option<String>,
    end_date: Option<String>,
) -> Result<ReadingDtoPagedList, Error> {
    let config = get_config();

    let response = api_reading_get_paged_readings_by_date_get(
        &config,
        start_date,
        end_date,
        Some(current_page),
        Some(page_size),
    )
    .await
    .map_err(|e| e.into());

    response
}

pub async fn get_readings_by_device(
    current_page: i32,
    page_size: i32,
    device_id: &str,
    start_date: Option<String>,
    end_date: Option<String>,
) -> Result<ReadingDtoPagedList, Error> {
    let config = get_config();

    let response = api_reading_get_paged_readings_by_device_get(
        &config,
        Some(device_id),
        Some(current_page),
        Some(page_size),
        start_date,
        end_date,
    )
    .await
    .map_err(|e| e.into());

    response
}

pub async fn get_chart_data(
    device_id: Option<uuid::Uuid>,
    reading_type: reading_type::ReadingType,
    start_date: Option<String>,
    end_date: Option<String>,
) -> Result<HashMap<String, f64>, Error> {
    let config = get_config();

    let body = GetChartData {
        device_id: Some(device_id),
        reading_type: reading_type,
        start_date: Some(start_date),
        end_date: Some(end_date),
    };

    let response = api_reading_get_chart_data_post(&config, body)
        .await
        .map_err(|e| e.into());

    response
}
