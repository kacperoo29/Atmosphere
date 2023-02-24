use atmosphere_api::models::UserRole;
use serde::{Serialize, Deserialize};

#[derive(Serialize, Deserialize, Clone, Debug, PartialEq, Default)]
pub struct UserInfo {
    pub username: String,
    pub token: String,
    pub role: UserRole
}

#[derive(Serialize, Deserialize, Clone, Debug, PartialEq, Default)]
pub struct LoginInfo {
    pub username: String,
    pub password: String
}
