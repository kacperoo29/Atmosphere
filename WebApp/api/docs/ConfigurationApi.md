# \ConfigurationApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_configuration_get_all_configurations_get**](ConfigurationApi.md#api_configuration_get_all_configurations_get) | **GET** /api/Configuration/GetAllConfigurations | 
[**api_configuration_get_configuration_entries_get**](ConfigurationApi.md#api_configuration_get_configuration_entries_get) | **GET** /api/Configuration/GetConfigurationEntries | 
[**api_configuration_get_configuration_entry_get**](ConfigurationApi.md#api_configuration_get_configuration_entry_get) | **GET** /api/Configuration/GetConfigurationEntry | 
[**api_configuration_get_email_config_get**](ConfigurationApi.md#api_configuration_get_email_config_get) | **GET** /api/Configuration/GetEmailConfig | 
[**api_configuration_get_enabled_notification_types_get**](ConfigurationApi.md#api_configuration_get_enabled_notification_types_get) | **GET** /api/Configuration/GetEnabledNotificationTypes | 
[**api_configuration_get_notification_types_get**](ConfigurationApi.md#api_configuration_get_notification_types_get) | **GET** /api/Configuration/GetNotificationTypes | 
[**api_configuration_get_validation_rules_get**](ConfigurationApi.md#api_configuration_get_validation_rules_get) | **GET** /api/Configuration/GetValidationRules | 
[**api_configuration_set_polling_rate_post**](ConfigurationApi.md#api_configuration_set_polling_rate_post) | **POST** /api/Configuration/SetPollingRate | 
[**api_configuration_toggle_notification_type_put**](ConfigurationApi.md#api_configuration_toggle_notification_type_put) | **PUT** /api/Configuration/ToggleNotificationType | 
[**api_configuration_update_configuration_put**](ConfigurationApi.md#api_configuration_update_configuration_put) | **PUT** /api/Configuration/UpdateConfiguration | 
[**api_configuration_update_email_config_patch**](ConfigurationApi.md#api_configuration_update_email_config_patch) | **PATCH** /api/Configuration/UpdateEmailConfig | 
[**api_configuration_update_validation_rules_put**](ConfigurationApi.md#api_configuration_update_validation_rules_put) | **PUT** /api/Configuration/UpdateValidationRules | 



## api_configuration_get_all_configurations_get

> ::std::collections::HashMap<String, serde_json::Value> api_configuration_get_all_configurations_get()


### Parameters

This endpoint does not need any parameter.

### Return type

[**::std::collections::HashMap<String, serde_json::Value>**](serde_json::Value.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_get_configuration_entries_get

> ::std::collections::HashMap<String, serde_json::Value> api_configuration_get_configuration_entries_get(keys)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**keys** | [**Vec<String>**](String.md) |  | [required] |

### Return type

[**::std::collections::HashMap<String, serde_json::Value>**](serde_json::Value.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_get_configuration_entry_get

> serde_json::Value api_configuration_get_configuration_entry_get(key)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**key** | **String** |  | [required] |

### Return type

[**serde_json::Value**](serde_json::Value.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_get_email_config_get

> crate::models::EmailConfiguration api_configuration_get_email_config_get()


### Parameters

This endpoint does not need any parameter.

### Return type

[**crate::models::EmailConfiguration**](EmailConfiguration.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_get_enabled_notification_types_get

> Vec<crate::models::NotificationType> api_configuration_get_enabled_notification_types_get()


### Parameters

This endpoint does not need any parameter.

### Return type

[**Vec<crate::models::NotificationType>**](NotificationType.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_get_notification_types_get

> Vec<crate::models::NotificationType> api_configuration_get_notification_types_get()


### Parameters

This endpoint does not need any parameter.

### Return type

[**Vec<crate::models::NotificationType>**](NotificationType.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_get_validation_rules_get

> Vec<crate::models::ValidationRuleDto> api_configuration_get_validation_rules_get(reading_type, device_id)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**reading_type** | [**ReadingType**](.md) |  | [required] |
**device_id** | Option<**uuid::Uuid**> |  |  |

### Return type

[**Vec<crate::models::ValidationRuleDto>**](ValidationRuleDto.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_set_polling_rate_post

> api_configuration_set_polling_rate_post(polling_rate, device_id)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**polling_rate** | Option<**i32**> |  |  |
**device_id** | Option<**uuid::Uuid**> |  |  |

### Return type

 (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_toggle_notification_type_put

> Vec<crate::models::NotificationType> api_configuration_toggle_notification_type_put(toggle_notification_type)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**toggle_notification_type** | [**ToggleNotificationType**](ToggleNotificationType.md) |  | [required] |

### Return type

[**Vec<crate::models::NotificationType>**](NotificationType.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_update_configuration_put

> serde_json::Value api_configuration_update_configuration_put(update_configuration)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**update_configuration** | [**UpdateConfiguration**](UpdateConfiguration.md) |  | [required] |

### Return type

[**serde_json::Value**](serde_json::Value.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_update_email_config_patch

> api_configuration_update_email_config_patch(email_configuration)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**email_configuration** | [**EmailConfiguration**](EmailConfiguration.md) |  | [required] |

### Return type

 (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_update_validation_rules_put

> api_configuration_update_validation_rules_put(update_validation_rules)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**update_validation_rules** | [**UpdateValidationRules**](UpdateValidationRules.md) |  | [required] |

### Return type

 (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

