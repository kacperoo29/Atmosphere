cmake_minimum_required(VERSION 3.13)

include($ENV{PICO_SDK_PATH}/external/pico_sdk_import.cmake)

project(atmosphere_rpi C CXX ASM)
set(CMAKE_C_STANDARD 11)
set(CMAKE_CXX_STANDARD 17)

set(PICO_BOARD pico_w)
add_compile_options(-Wall)

if (PICO_SDK_VERSION_STRING VERSION_LESS "1.3.0")
    message(FATAL_ERROR "Raspberry Pi Pico SDK version 1.3.0 (or later) required. Your version is ${PICO_SDK_VERSION_STRING}")
endif()

pico_sdk_init()

if (PICO_CYW43_SUPPORTED)
    if (NOT TARGET pico_cyw43_arch)
        message("Only Pico W supported at the moment")
    else()
        set(WIFI_SSID "${WIFI_SSID}" CACHE INTERNAL "WiFi SSID")
        set(WIFI_PASSWORD "${WIFI_PASSWORD}" CACHE INTERNAL "WiFi password")
        add_compile_definitions(WIFI_SSID=\"${WIFI_SSID}\")
        add_compile_definitions(WIFI_PASSWORD=\"${WIFI_PASSWORD}\")

        if ("${WIFI_SSID}" STREQUAL "")
            message(SEND_ERROR "Wifi SSID not defined.")
        elseif ("${WIFI_PASSWORD}" STREQUAL "")
            message(SEND_ERROR "WIFI password not defined.")
        else()
            add_executable(atmosphere_rpi
                include/temp_reader.h
                src/temp_reader.cpp

                src/main.cpp
            )

            target_include_directories(atmosphere_rpi PRIVATE
                ${CMAKE_CURRENT_LIST_DIR}/include
            )

            add_subdirectory(3rdparty/onewire)

            target_link_libraries(atmosphere_rpi
                pico_stdlib
                pico_lwip_http
                pico_cyw43_arch_none
                pico_cyw43_arch_lwip_threadsafe_background
                pico_one_wire
            )

            pico_add_extra_outputs(atmosphere_rpi)

            if (TARGET tinyusb_device)
                message("Enabling usb support.")
                pico_enable_stdio_usb(atmosphere_rpi 1)
                pico_enable_stdio_uart(atmosphere_rpi 0)
            endif()
        endif()
    endif()
endif()
