# \ReadingApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**api_reading_create_reading_post**](ReadingApi.md#api_reading_create_reading_post) | **POST** /api/Reading/CreateReading | 
[**api_reading_get_all_readings_get**](ReadingApi.md#api_reading_get_all_readings_get) | **GET** /api/Reading/GetAllReadings | 



## api_reading_create_reading_post

> crate::models::ReadingDto api_reading_create_reading_post(create_reading)


### Parameters


Name | Type | Description  | Required | Notes
------------- | ------------- | ------------- | ------------- | -------------
**create_reading** | [**CreateReading**](CreateReading.md) |  | [required] |

### Return type

[**crate::models::ReadingDto**](ReadingDto.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: application/json, text/json, application/*+json
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


## api_reading_get_all_readings_get

> Vec<crate::models::ReadingDto> api_reading_get_all_readings_get()


### Parameters

This endpoint does not need any parameter.

### Return type

[**Vec<crate::models::ReadingDto>**](ReadingDto.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
