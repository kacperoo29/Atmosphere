use std::collections::HashMap;

use atmosphere_api::{
    apis::configuration_api::{
        api_configuration_get_configuration_entries_get, api_configuration_get_email_config_get,
        api_configuration_get_validation_rules_get, api_configuration_update_configuration_put,
        api_configuration_update_email_config_patch, api_configuration_update_validation_rules_put, api_configuration_set_polling_rate_post,
    },
    models::{
        EmailConfiguration, ReadingType, UpdateConfiguration, UpdateValidationRules,
        ValidationRuleDto,
    },
};

use crate::error::Error;

use super::get_config;

pub async fn set_value(
    key: String,
    value: Option<serde_json::Value>,
) -> Result<serde_json::Value, Error> {
    let config = get_config().clone();
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

pub async fn get_validation_rules(
    reading_type: ReadingType,
) -> Result<Vec<ValidationRuleDto>, Error> {
    let config = get_config().clone();

    api_configuration_get_validation_rules_get(&config, reading_type)
        .await
        .map_err(|err| err.into())
}

pub async fn update_validation_rules(
    reading_type: ReadingType,
    validation_rules: Vec<ValidationRuleDto>,
) -> Result<(), Error> {
    let config = get_config().clone();

    let model = UpdateValidationRules {
        reading_type: reading_type,
        rules: validation_rules,
    };

    api_configuration_update_validation_rules_put(&config, model)
        .await
        .map_err(|err| err.into())
}

pub async fn get_email_config() -> Result<EmailConfiguration, Error> {
    let config = get_config().clone();

    api_configuration_get_email_config_get(&config)
        .await
        .map_err(|err| err.into())
}

pub async fn update_email_config(update_config: EmailConfiguration) -> Result<(), Error> {
    let config = get_config().clone();

    api_configuration_update_email_config_patch(&config, update_config)
        .await
        .map_err(|err| err.into())
}

pub async fn change_polling_rate(rate: i32) -> Result<(), Error> {
    let config = get_config().clone();

    api_configuration_set_polling_rate_post(&config, Some(rate))
        .await
        .map_err(|err| err.into())
}
