use atmosphere_api::{apis::reading_api::{api_reading_get_all_readings_get, api_reading_get_readings_by_date_get}, models::ReadingDto};

use crate::error::Error;

use super::get_config;

pub async fn get_readings() -> Result<Vec<ReadingDto>, Error> {
    let config = get_config();
    match api_reading_get_all_readings_get(&config).await {
        Ok(response) => Ok(response),
        Err(err) => Err(err.into()),
    }
}

pub async fn get_readings_by_date(start_date: Option<String>, end_date: Option<String>) -> Result<Vec<ReadingDto>, Error> {
    let config = get_config();
    match api_reading_get_readings_by_date_get(&config, start_date, end_date).await {
        Ok(response) => Ok(response),
        Err(err) => Err(err.into()),
    }
}
