# \AuthApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_auth_activate_user_id_put**](AuthApi.md#api_auth_activate_user_id_put) | **PUT** /api/Auth/ActivateUser/{id} | 
[**api_auth_authenticate_post**](AuthApi.md#api_auth_authenticate_post) | **POST** /api/Auth/Authenticate | 
[**api_auth_register_device_post**](AuthApi.md#api_auth_register_device_post) | **POST** /api/Auth/RegisterDevice | 



## api_auth_activate_user_id_put

> api_auth_activate_user_id_put(id)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**id** | **uuid::Uuid** |  | [required] |

### Return type

 (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_auth_authenticate_post

> String api_auth_authenticate_post(authenticate)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**authenticate** | [**Authenticate**](Authenticate.md) |  | [required] |

### Return type

**String**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_auth_register_device_post

> crate::models::DeviceDto api_auth_register_device_post(register_device)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**register_device** | [**RegisterDevice**](RegisterDevice.md) |  | [required] |

### Return type

[**crate::models::DeviceDto**](DeviceDto.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

