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
pub struct ToggleNotificationType {
    #[serde(rename = "type")]
    pub r#type: crate::models::NotificationType,
}

impl ToggleNotificationType {
    pub fn new(r#type: crate::models::NotificationType) -> ToggleNotificationType {
        ToggleNotificationType {
            r#type,
        }
    }
}


