use atmosphere_api::{apis::device_api::api_device_get_all_get, models::DeviceDto};

use crate::error::Error;

use super::get_config;

pub async fn get_all_devices() -> Result<Vec<DeviceDto>, Error> {
    let config = get_config();

    api_device_get_all_get(&config).await.map_err(|e| e.into())
}