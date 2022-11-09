cmake_minimum_required(VERSION 3.13)

include($ENV{PICO_SDK_PATH}/external/pico_sdk_import.cmake)

project(atmosphere_rpi C CXX ASM)
set(CMAKE_C_STANDARD 11)
set(CMAKE_CXX_STANDARD 17)

set(PICO_BOARD pico_w)
add_compile_options(-Wall -Wextra -fexceptions)

if (PICO_SDK_VERSION_STRING VERSION_LESS "1.3.0")
    message(FATAL_ERROR "Raspberry Pi Pico SDK version 1.3.0 (or later) required. Your version is ${PICO_SDK_VERSION_STRING}")
endif()

set(PICO_CXX_ENABLE_EXCEPTIONS 1)
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
            set(FREERTOS_ADDONS_SRC
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/ccondition_variable.cpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/cevent_groups.cpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/cmem_pool.cpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/cmutex.cpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/cqueue.cpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/cread_write_lock.cpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/csemaphore.cpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/ctasklet.cpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/cthread.cpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/ctickhook.cpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/ctimer.cpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/cworkqueue.cpp

                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/condition_variable.hpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/event_groups.hpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/mem_pool.hpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/mutex.hpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/queue.hpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/read_write_lock.hpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/semaphore.hpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/tasklet.hpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/thread.hpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/tickhook.hpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/timer.hpp
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include/workqueue.hpp
            )

            add_executable(atmosphere_rpi
                include/tasks/connect_task.h
                src/tasks/connect_task.cpp

                include/tasks/mqtt_task.h
                src/tasks/mqtt_task.cpp

                include/temp_reader.h
                src/temp_reader.cpp

                include/mqtt.h
                src/mqtt.cpp

                src/main.cpp

                ${FREERTOS_ADDONS_SRC}
            )

            target_include_directories(atmosphere_rpi PRIVATE
                ${CMAKE_CURRENT_LIST_DIR}/include
                ${CMAKE_CURRENT_LIST_DIR}/3rdparty/freertos-addons/c++/Source/include
            )

            add_subdirectory(3rdparty/onewire)

            set(FREERTOS_CONFIG_FILE_DIRECTORY ${CMAKE_CURRENT_LIST_DIR}/include CACHE STRING "")
            set(FREERTOS_PORT GCC_RP2040 CACHE STRING "")
            add_subdirectory(3rdparty/FreeRTOS-Kernel)

            target_sources(atmosphere_rpi PRIVATE ${PICO_SDK_PATH}/lib/lwip/src/apps/mqtt/mqtt.c)
            target_link_libraries(atmosphere_rpi                
                pico_stdlib
                pico_cyw43_arch_lwip_sys_freertos
                pico_one_wire
                freertos_kernel
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
