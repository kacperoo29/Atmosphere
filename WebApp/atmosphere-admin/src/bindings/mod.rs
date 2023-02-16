use wasm_bindgen::prelude::*;

#[wasm_bindgen(module = "/src/js/notifications.ts")]
extern "C" {
    #[wasm_bindgen]
    pub fn notify(title: &str, body: &str);
    #[wasm_bindgen]
    pub fn show_error(text: &str);
    #[wasm_bindgen]
    pub fn set_send_ping_interval(ws: &web_sys::WebSocket, interval: u32);
    #[wasm_bindgen]
    pub fn set_on_close(ws: &web_sys::WebSocket);
}
