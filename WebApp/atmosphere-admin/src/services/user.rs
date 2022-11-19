use std::sync::{Arc, RwLock};

use atmosphere_api::{
    apis::{
        self,
        auth_api::{api_auth_authenticate_post, ApiAuthAuthenticatePostError},
        configuration::Configuration,
    },
    models::Authenticate,
};
use gloo_storage::{LocalStorage, Storage};
use lazy_static::lazy_static;

use crate::{error::Error, models::user::UserInfo};

const TOKEN_KEY: &str = "atmosphere.admin.token";

lazy_static! {
    pub static ref TOKEN: RwLock<Option<String>> = {
        if let Ok(token) = LocalStorage::get(TOKEN_KEY) {
            RwLock::new(Some(token))
        } else {
            RwLock::new(None)
        }
    };
}

pub fn set_token(token: Option<String>) {
    if let Some(token) = token.clone() {
        LocalStorage::set(TOKEN_KEY, &token).expect("Unable to set local storage token.");
    } else {
        LocalStorage::delete(TOKEN_KEY);
    }

    let mut token_lock = TOKEN.write().expect("Failed to lock token.");
    *token_lock = token;
}

pub fn get_token() -> Option<String> {
    let token_lock = TOKEN.read().expect("Failed to lock token.");

    token_lock.clone()
}

// TODO: Get current user here instead of token.
pub async fn current() -> Result<UserInfo, Error> {
    let mut config = Configuration::new();
    config.base_path = "http://localhost:5000".to_string();
    let model = Authenticate {
        username: "admin".to_string(),
        password: "secretPassword".to_string(),
    };

    match api_auth_authenticate_post(&config, model).await {
        Ok(token) => Ok(UserInfo {
            email: "admin".to_string(),
            token: token,
        }),
        Err(err) => match err {
            apis::Error::ResponseError(err) => {
                match err.status.as_u16() {
                    _ => Err(Error::Unknown(err.content))
                }
            },
            err=> Err(Error::Unknown(err.to_string()))
        },
    }
}
