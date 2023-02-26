/*
 * Atmosphere API
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 * Generated by: https://openapi-generator.tech
 */




#[derive(Clone, Debug, PartialEq, Default, Serialize, Deserialize)]
pub struct UserDto {
    #[serde(rename = "id")]
    pub id: uuid::Uuid,
    #[serde(rename = "email")]
    pub email: String,
    #[serde(rename = "role")]
    pub role: crate::models::UserRole,
    #[serde(rename = "isActive")]
    pub is_active: bool,
}

impl UserDto {
    pub fn new(id: uuid::Uuid, email: String, role: crate::models::UserRole, is_active: bool) -> UserDto {
        UserDto {
            id,
            email,
            role,
            is_active,
        }
    }
}

