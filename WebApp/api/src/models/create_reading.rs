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
pub struct CreateReading {
    #[serde(rename = "value")]
    pub value: f64,
    #[serde(rename = "type")]
    pub r#type: crate::models::ReadingType,
    #[serde(rename = "timestamp")]
    pub timestamp: String,
}

impl CreateReading {
    pub fn new(value: f64, r#type: crate::models::ReadingType, timestamp: String) -> CreateReading {
        CreateReading {
            value,
            r#type,
            timestamp,
        }
    }
}


