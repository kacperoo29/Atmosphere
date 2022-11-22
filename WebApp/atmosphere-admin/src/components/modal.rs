use yew::prelude::*;

#[derive(Clone, Debug, PartialEq, Properties)]
pub struct Props {
    pub hidden: bool,
    pub title: String,
    pub message: String,
    pub onclose: Callback<MouseEvent>,
}

#[function_component(Modal)]
pub fn modal(props: &Props) -> Html {
    let on_close = props.onclose.clone();

    let class = if props.hidden {
        "modal fade"
    } else {
        "modal fade show is-active"
    };

    let style = if props.hidden {
        "display: none;"
    } else {
        "display: block;"
    };

    let aria_hidden = if props.hidden {
        "true"
    } else {
        "false"
    };
    
    html! {
        <div tabindex="-1" class={class} hidden={props.hidden} style={style} aria-hidden={aria_hidden}>
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h2 class="modal-title">{ &props.title }</h2>
                        <button type="button" class="close" onclick={&on_close}>
                            <span aria-hidden="true">{"\u{00D7}"}</span>
                        </button>
                    </div>
                    <div class="modal-body modal-message">
                        { &props.message }
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" onclick={&on_close}>{ "Close" }</button>
                    </div>
                </div>
            </div>
        </div>
    }
}
