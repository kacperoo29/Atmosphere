use yew::prelude::*;

#[function_component(SignIn)]
pub fn signin() -> Html {
    html! {
        <div class="container">
            <div class="row">
                <div class="col-md-6 offset-md-3">
                    <div class="card">
                        <div class="card-header">
                            <h3>{"Sign In"}</h3>
                        </div>
                        <div class="card-body">
                            <form>
                                <div class="form-group mb-3">
                                    <label for="email">{"Email"}</label>
                                    <input type="email" class="form-control" id="email" />
                                </div>
                                <div class="form-group mb-3">
                                    <label for="password">{"Password"}</label>
                                    <input type="password" class="form-control" id="password" />
                                </div>
                                <div class="form-group mb-3">
                                    <button type="submit" class="btn btn-primary">{"Sign In"}</button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    }
}
