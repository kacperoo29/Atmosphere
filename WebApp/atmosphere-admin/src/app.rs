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
                <main class="d-flex flex-nowrap">
                    <Nav />
                    <div class="d-flex flex-column overflow-auto flex-grow-1 p-3">
                        <Switch<AppRoute> render={Switch::render(switch)} />
                    </div>
                </main>
            </BrowserRouter>
        </UserContextProvider>
    }
}
