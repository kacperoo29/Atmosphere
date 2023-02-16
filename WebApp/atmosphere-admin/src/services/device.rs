use atmosphere_api::{
    apis::{auth_api::api_auth_activate_user_id_put, device_api::api_device_get_all_get},
    models::DeviceDto,
};

use crate::error::Error;

use super::get_config;

pub async fn get_all_devices() -> Result<Vec<DeviceDto>, Error> {
    let config = get_config();

    api_device_get_all_get(&config).await.map_err(|e| e.into())
}

pub async fn activate_device(id: String) -> Result<(), Error> {
    let config = get_config();

    api_auth_activate_user_id_put(&config, &id)
        .await
        .map_err(|e| e.into())
}
