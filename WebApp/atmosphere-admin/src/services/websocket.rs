use web_sys::WebSocket;

use super::get_config;

pub fn open_notification_socket() -> Option<WebSocket> {
    let config = get_config().clone();
    let Some(token) = config.api_key else {
        return None;
    };

    let url = format!(
        "{}{}?access_token={}",
        config.base_path, "/api/websocket/notifications", token.key
    ).replace("http", "ws");

    let socket = WebSocket::new(&url).expect("Failed to create websocket.");

    Some(socket)
}
