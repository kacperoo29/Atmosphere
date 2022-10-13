use yew::prelude::*;

pub struct NotFoundPage {}

impl Component for NotFoundPage {
    type Message = ();
    type Properties = ();

    fn create(context: &Context<Self>) -> Self {
        return Self {}
    }

    fn view(&self, context: &Context<Self>) -> Html {
        html! {
            <div class="container">
                <h1>{"404"}</h1>
                <p>{"Page not found"}</p>
            </div>
        }
    }

    fn update(&mut self, ctx: &Context<Self>, msg: Self::Message) -> bool {
        return false;
    }

    fn changed(&mut self, ctx: &Context<Self>) -> bool {
        return false;
    }

    fn rendered(&mut self, ctx: &Context<Self>, first_render: bool) {}

    fn destroy(&mut self, ctx: &Context<Self>) {}
}
