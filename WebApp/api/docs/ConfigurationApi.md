# \ConfigurationApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_configuration_get_all_configurations_get**](ConfigurationApi.md#api_configuration_get_all_configurations_get) | **GET** /api/Configuration/GetAllConfigurations | 
[**api_configuration_get_configuration_entries_get**](ConfigurationApi.md#api_configuration_get_configuration_entries_get) | **GET** /api/Configuration/GetConfigurationEntries | 
[**api_configuration_get_configuration_entry_get**](ConfigurationApi.md#api_configuration_get_configuration_entry_get) | **GET** /api/Configuration/GetConfigurationEntry | 
[**api_configuration_update_configuration_put**](ConfigurationApi.md#api_configuration_update_configuration_put) | **PUT** /api/Configuration/UpdateConfiguration | 



## api_configuration_get_all_configurations_get

> Vec<serde_json::Value> api_configuration_get_all_configurations_get()


### Parameters

This endpoint does not need any parameter.

### Return type

[**Vec<serde_json::Value>**](serde_json::Value.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_configuration_get_configuration_entries_get

> Vec<serde_json::Value> api_configuration_get_configuration_entries_get(keys)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**keys** | [**Vec<String>**](String.md) |  | [required] |

### Return type

[**Vec<serde_json::Value>**](serde_json::Value.md)

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

