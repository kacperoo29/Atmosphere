use wasm_bindgen::prelude::*;

#[wasm_bindgen(module = "/src/js/notifications.ts")]
extern "C" {
    #[wasm_bindgen]
    pub fn notify(title: &str, body: &str);
}