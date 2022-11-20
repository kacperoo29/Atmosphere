use yew::prelude::*;
use yew_router::prelude::*;

use crate::components::nav::Nav;
use crate::components::user_context_provider::UserContextProvider;
use crate::routes::{switch, AppRoute};

/// Root app component
#[function_component(App)]
pub fn app() -> Html {
    html! {
        <UserContextProvider>
            <BrowserRouter>
                <div class="container-fluid">
                    <div class="row flex-nowrap">
                        <Nav />
                        <div class="col py-3">
                            <Switch<AppRoute> render={Switch::render(switch)} />
                        </div>
                    </div>
                </div>
            </BrowserRouter>
        </UserContextProvider>
    }
}
