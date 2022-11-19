use serde::{Serialize, Deserialize};

#[derive(Serialize, Deserialize, Clone, Debug, PartialEq, Default)]
pub struct UserInfo {
    pub email: String,
    pub token: String,
}
