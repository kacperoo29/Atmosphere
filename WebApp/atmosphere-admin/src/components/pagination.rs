use yew::prelude::*;

#[derive(Clone, PartialEq, Properties)]
pub struct PaginationProps {
    pub current_page: i32,
    pub page_size: i32,
    pub total_pages: i32,
    pub has_next_page: bool,
    pub has_previous_page: bool,
    pub on_page_change: Callback<i32>,
}

#[function_component(Pagination)]
pub fn pagination(props: &PaginationProps) -> Html {
    let props = props.clone();

    html! {
        <nav aria-label="Page navigation" class="d-flex justify-content-center">
            <ul class="pagination">
                // first
                <li class="page-item">
                    <button
                        class="page-link"
                        onclick={props.on_page_change.reform(move |_| 1)}
                    >
                        {"First"}
                    </button>
                </li>

                <li class="page-item">
                    <button
                        class="page-link"
                        disabled={!props.has_previous_page}
                        onclick={props.on_page_change.reform(move |_| props.current_page - 1)}
                    >
                        {"Previous"}
                    </button>
                </li>

                <li class="page-item">
                    <button
                        class="page-link"
                        disabled={props.current_page - 10 < 1}
                        onclick={props.on_page_change.reform(move |_| props.current_page - 10)}
                    >
                        {"..."}
                    </button>
                </li>

                // previous
                {
                    if props.current_page > 1 {
                        html! {
                            <li class="page-item">
                                <button
                                    class="page-link"
                                    onclick={props.on_page_change.reform(move |_| props.current_page - 1)}
                                >
                                    {props.current_page - 1}
                                </button>
                            </li>
                        }
                    } else {
                        html! {}
                    }
                }

                // current
                <li class="page-item">
                    <button
                        class="page-link"
                        disabled={true}
                    >
                        {props.current_page}
                    </button>
                </li>

                // next
                {
                    if props.current_page < props.total_pages - 1 {
                        html! {
                            <li class="page-item">
                                <button
                                    class="page-link"
                                    onclick={props.on_page_change.reform(move |_| props.current_page + 1)}
                                >
                                    {props.current_page + 1}
                                </button>
                            </li>
                        }
                    } else {
                        html! {}
                    }
                }

                <li class="page-item">
                    <button
                        class="page-link"
                        disabled={props.current_page + 10 > props.total_pages}
                        onclick={props.on_page_change.reform(move |_| props.current_page + 10)}
                    >
                        {"..."}
                    </button>
                </li>

                <li class="page-item">
                    <button
                        class="page-link"
                        disabled={!props.has_next_page}
                        onclick={props.on_page_change.reform(move |_| props.current_page + 1)}
                    >
                        {"Next"}
                    </button>
                </li>

                // last
                <li class="page-item">
                    <button
                        class="page-link"
                        onclick={props.on_page_change.reform(move |_| props.total_pages)}
                    >
                        {"Last"}
                    </button>
                </li>

            </ul>

        </nav>
    }
}

