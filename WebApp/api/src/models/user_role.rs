/*
 * Atmosphere API
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 * Generated by: https://openapi-generator.tech
 */


/// 
#[derive(Clone, Copy, Debug, Eq, PartialEq, Ord, PartialOrd, Hash, Serialize, Deserialize)]
pub enum UserRole {
    #[serde(rename = "user")]
    User,
    #[serde(rename = "admin")]
    Admin,
    #[serde(rename = "device")]
    Device,

}

impl ToString for UserRole {
    fn to_string(&self) -> String {
        match self {
            Self::User => String::from("user"),
            Self::Admin => String::from("admin"),
            Self::Device => String::from("device"),
        }
    }
}

impl Default for UserRole {
    fn default() -> UserRole {
        Self::User
    }
}




