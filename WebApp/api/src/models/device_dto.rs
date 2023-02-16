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
pub struct DeviceDto {
    #[serde(rename = "id")]
    pub id: uuid::Uuid,
    #[serde(rename = "identifier")]
    pub identifier: String,
    #[serde(rename = "isActive")]
    pub is_active: bool,
    #[serde(rename = "isConnected")]
    pub is_connected: bool,
}

impl DeviceDto {
    pub fn new(id: uuid::Uuid, identifier: String, is_active: bool, is_connected: bool) -> DeviceDto {
        DeviceDto {
            id,
            identifier,
            is_active,
            is_connected,
        }
    }
}


