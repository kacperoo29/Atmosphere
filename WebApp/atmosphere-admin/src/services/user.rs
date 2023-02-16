use std::sync::RwLock;

use atmosphere_api::{
    apis::{
        auth_api::{api_auth_authenticate_post, api_auth_get_current_user_get},
        configuration::ApiKey,
    },
    models::Authenticate,
};
use gloo_storage::{LocalStorage, Storage};
use lazy_static::lazy_static;

use crate::{
    error::Error,
    models::user::{LoginInfo, UserInfo},
};

use super::{get_config, get_mut_config};

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

pub async fn current() -> Result<UserInfo, Error> {
    let config = get_config().clone();
    let Some(token) = get_token() else {
        return Err(Error::Unauthorized("User not logged in.".to_string()));
    };

    match api_auth_get_current_user_get(&config).await {
        Ok(response) => Ok(UserInfo {
            username: response.username,
            token: token,
        }),
        Err(err) => Err(err.into()),
    }
}

pub async fn authenticate(login_info: LoginInfo) -> Result<UserInfo, Error> {
    let config = get_config().clone();
    let model = Authenticate {
        username: login_info.username,
        password: login_info.password,
    };

    match api_auth_authenticate_post(&config, model).await {
        Ok(response) => {
            let mut config = get_mut_config();
            set_token(Some(response.token.clone()));
            config.api_key = Some(ApiKey {
                prefix: Some("Bearer".to_string()),
                key: response.token.clone(),
            });

            Ok(response.token)
        }
        Err(err) => {
            Err(err.into())
        }
    }?;

    current().await
}
