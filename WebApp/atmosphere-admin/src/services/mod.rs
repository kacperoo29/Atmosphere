pub mod user;
use std::sync::{RwLock, RwLockReadGuard, RwLockWriteGuard};

use atmosphere_api::apis::configuration::{Configuration, ApiKey};
use lazy_static::lazy_static;

use crate::services::user::get_token;

lazy_static! {
    pub static ref CONFIG: RwLock<Configuration> = RwLock::new((|| {
        let mut config = Configuration::new();
        config.base_path = "http://localhost:5000".to_string();
        let Some(token) = get_token() else {
            return config;
        };

        config.api_key = Some(ApiKey {
            prefix: Some("Bearer".to_string()),
            key: token,
        });

        config
    })());
}

pub fn get_config() -> RwLockReadGuard<'static, Configuration> {
    CONFIG.read().expect("Couldn't lock config for reading.")
}

pub fn get_mut_config() -> RwLockWriteGuard<'static, Configuration> {
    CONFIG.write().expect("Couldn't lock config for write")
}
