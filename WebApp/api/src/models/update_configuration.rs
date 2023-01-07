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
pub struct UpdateConfiguration {
    #[serde(rename = "key")]
    pub key: String,
    #[serde(rename = "value", deserialize_with = "Option::deserialize")]
    pub value: Option<serde_json::Value>,
}

impl UpdateConfiguration {
    pub fn new(key: String, value: Option<serde_json::Value>) -> UpdateConfiguration {
        UpdateConfiguration {
            key,
            value,
        }
    }
}

