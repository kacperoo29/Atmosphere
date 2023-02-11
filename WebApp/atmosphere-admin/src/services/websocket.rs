use web_sys::WebSocket;

use crate::error::Error;

use super::get_config;

pub fn open_notification_socket() -> Result<WebSocket, Error> {
    let config = get_config().clone();
    let Some(token) = config.api_key else {
        return Err(Error::Unauthorized("User not logged in.".to_string()));
    };

    let url = format!(
        "{}{}?access_token={}",
        config.base_path, "/api/websocket/notifications", token.key
    ).replace("http", "ws");

    log::info!("Opening websocket: {}", url);

    WebSocket::new(&url).map_err(|e| e.into())
}
