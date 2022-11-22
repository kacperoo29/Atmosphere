use serde::{Serialize, Deserialize};

#[derive(Serialize, Deserialize, Clone, Debug, PartialEq, Default)]
pub struct BaseNotificationConfig {
    pub email_enabled: bool,
    pub email: String,
}

#[derive(Serialize, Deserialize, Clone, Debug, PartialEq, Default)]
pub struct TemperatureNotificationConfig {
    pub min: f64,
    pub max: f64,
}

#[derive(Serialize, Deserialize, Clone, Debug, PartialEq, Default)]
pub struct NotificationConfig {
    pub base: BaseNotificationConfig,
    pub temperature: TemperatureNotificationConfig,
}