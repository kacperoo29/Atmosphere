use std::collections::HashMap;

use atmosphere_api::{
    apis::configuration_api::{
        api_configuration_get_configuration_entries_get,
        api_configuration_get_notification_settings_get,
        api_configuration_update_configuration_put,
        api_configuration_update_notification_settings_put,
    },
    models::{UpdateConfiguration, UpdateNotificationSettings},
};

use crate::{
    error::Error,
    models::config::{BaseNotificationConfig, NotificationConfig, TemperatureNotificationConfig},
};

use super::get_config;

pub async fn set_value(
    key: String,
    value: Option<serde_json::Value>,
) -> Result<serde_json::Value, Error> {
    let config = get_config().clone();
    log::info!("Setting config value: {} = {:?}", key, value);
    let model = UpdateConfiguration {
        key: key,
        value: value,
    };

    api_configuration_update_configuration_put(&config, model)
        .await
        .map_err(|err| err.into())
}

pub async fn get_settings(keys: Vec<String>) -> Result<HashMap<String, serde_json::Value>, Error> {
    let config = get_config().clone();

    api_configuration_get_configuration_entries_get(&config, keys)
        .await
        .map_err(|err| err.into())
}

pub async fn set_notifications_settings(settings: NotificationConfig) -> Result<(), Error> {
    let config = get_config().clone();
    let settings_model = UpdateNotificationSettings {
        email_enabled: settings.base.email_enabled,
        email_to: settings.base.email,
        temperature_threshold_min: settings.temperature.min,
        temperature_threshold_max: settings.temperature.max,
    };

    api_configuration_update_notification_settings_put(&config, settings_model)
        .await
        .map_err(|err| err.into())
}

pub async fn get_notifications_settings() -> Result<NotificationConfig, Error> {
    let config = get_config().clone();

    api_configuration_get_notification_settings_get(&config)
        .await
        .map(|settings| NotificationConfig {
            base: BaseNotificationConfig {
                email_enabled: settings.email_enabled,
                email: settings.email_to,
            },
            temperature: TemperatureNotificationConfig {
                min: settings.temperature_threshold_min,
                max: settings.temperature_threshold_max,
            },
        })
        .map_err(|err| err.into())
}
