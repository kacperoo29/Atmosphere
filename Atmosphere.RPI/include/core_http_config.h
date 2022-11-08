#ifndef _CORE_HTTP_CONFIG
#define _CORE_HTTP_CONFIG

#ifdef __cplusplus
    extern "C" {
#endif

#ifndef HTTP_USER_AGENT_VALUE
    #define HTTP_USER_AGENT_VALUE      "Mozilla/5.0 (compatible; esp8266 Lua; Windows NT 5.1)"
#endif

#ifndef HTTP_RECV_RETRY_TIMEOUT_MS
    #define HTTP_RECV_RETRY_TIMEOUT_MS    ( 10U )
#endif

#ifndef HTTP_SEND_RETRY_TIMEOUT_MS
    #define HTTP_SEND_RETRY_TIMEOUT_MS    ( 10U )
#endif

#ifdef __cplusplus
    }
#endif

#endif