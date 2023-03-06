use web_sys::WebSocket;

use super::get_config;

pub fn open_notification_socket(token: String) -> Option<WebSocket> {
    let config = get_config().clone();

    let url = format!(
        "{}{}?access_token={}",
        config.base_path, "/api/websocket/notifications", token
    )
    .replace("http", "ws");

    log::info!("Opening notification socket: {}", url);
    let socket = WebSocket::new(&url).expect("Failed to create websocket.");

    Some(socket)
}
