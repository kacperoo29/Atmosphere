use std::fmt::{Debug, Formatter};
use std::ops::Deref;

use atmosphere_api::models::UserRole;
use yew::{use_context, UseStateHandle};
use yew_router::prelude::{use_history, AnyHistory, History};

use crate::models::user::UserInfo;
use crate::routes::AppRoute;
use crate::services::user::set_token;

#[derive(Clone, PartialEq)]
pub struct UseUserContextHandle {
    inner: UseStateHandle<Option<UserInfo>>,
    history: AnyHistory,
}

impl UseUserContextHandle {
    pub fn login(&self, user: UserInfo){
        set_token(Some(user.token.clone()));
        self.inner.set(Some(user));
        self.history.push(AppRoute::Home);
    }

    pub fn logout(&self) {
        set_token(None);
        self.inner.set(None);
        self.history.push(AppRoute::SignIn);
    }

    pub fn is_logged_in(&self) -> bool {
        (*self.inner).is_some()
    }

    pub fn info(&self) -> Option<UserInfo> {
        (*self.inner).clone()
    }

    pub fn role(&self) -> Option<UserRole> {
        (*self.inner).as_ref().map(|user| user.role.clone())
    }
}

impl Deref for UseUserContextHandle {
    type Target = UseStateHandle<Option<UserInfo>>;

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
    let inner = use_context::<UseStateHandle<Option<UserInfo>>>().unwrap();
    let history = use_history().unwrap();

    UseUserContextHandle { inner, history }
}
