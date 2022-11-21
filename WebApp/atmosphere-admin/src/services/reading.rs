use atmosphere_api::{apis::reading_api::api_reading_get_all_readings_get, models::ReadingDto};

use crate::error::Error;

use super::get_config;

pub async fn get_readings() -> Result<Vec<ReadingDto>, Error> {
    let config = get_config();
    match api_reading_get_all_readings_get(&config).await {
        Ok(response) => Ok(response),
        Err(err) => Err(err.into()),
    }
}
