use serde::{Serialize, Deserialize};

#[derive(Serialize, Deserialize, Clone, Debug, PartialEq, Default)]
pub struct UserInfo {
    pub username: String,
    pub token: String,
}

#[derive(Serialize, Deserialize, Clone, Debug, PartialEq, Default)]
pub struct LoginInfo {
    pub username: String,
    pub password: String
}
