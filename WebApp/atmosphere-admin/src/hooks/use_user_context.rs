use std::fmt::{Debug, Formatter};
use std::ops::Deref;

use yew::{use_context, UseStateHandle};
use yew_router::prelude::{use_history, AnyHistory, History};

use crate::error::Error;
use crate::models::user::UserInfo;
use crate::routes::AppRoute;
use crate::services::user::set_token;

#[derive(Clone, PartialEq)]
pub struct UseUserContextHandle {
    inner: UseStateHandle<UserInfo>,
    history: AnyHistory,
}

impl UseUserContextHandle {
    pub fn login(&mut self, user: UserInfo){
        set_token(Some(user.token.clone()));
        self.inner.set(user);
        self.history.push(AppRoute::Home);
    }

    pub fn logout(&mut self) {
        set_token(None);
        self.inner.set(UserInfo::default());
        self.history.push(AppRoute::SignIn);
    }
}

impl Deref for UseUserContextHandle {
    type Target = UseStateHandle<UserInfo>;

    fn deref(&self) -> &Self::Target {
        &self.inner
    }
}

impl Debug for UseUserContextHandle {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        f.debug_struct("UseUserContextHandle")
            .field("inner", &self.inner)
            .finish()
    }
}

pub fn use_user_context() -> UseUserContextHandle {
    let inner = use_context::<UseStateHandle<UserInfo>>().unwrap();
    let history = use_history().unwrap();

    UseUserContextHandle { inner, history }
}
