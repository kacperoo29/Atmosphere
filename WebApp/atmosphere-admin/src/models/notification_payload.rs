use serde::{Deserialize, Serialize};


#[derive(Debug, Serialize, Deserialize)]
pub struct NotificationPayload {
    pub r#type: String,
    pub payload: Vec<String>,
}