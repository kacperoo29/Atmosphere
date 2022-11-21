use atmosphere_api::{
    apis::configuration_api::api_configuration_update_configuration_put,
    models::UpdateConfiguration,
};

use crate::error::Error;

use super::{get_config, user::get_token};

pub const NOTIFICATION_EMAIL_KEY: &str = "atmosphere.admin.notification.email";

pub async fn set_value(key: String, value: Option<serde_json::Value>) -> Result<serde_json::Value, Error> {
    let config = get_config().clone();
    let Some(_) = get_token() else {
        return Err(Error::Unauthorized("User not logged in.".to_string()));
    };

    let model = UpdateConfiguration {
        key: key,
        value: Some(value),
    };

    api_configuration_update_configuration_put(&config, model)
        .await
        .map_err(|err| err.into())
}
