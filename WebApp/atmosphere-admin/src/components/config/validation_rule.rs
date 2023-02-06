use atmosphere_api::models::{ReadingType, ValidationRuleDto};
use serde::{Deserialize, Serialize};
use serde_json::de::Read;
use web_sys::{HtmlInputElement, InputEvent};
use yew::{
    function_component, html, use_effect_with_deps, use_state, Callback, Properties, TargetCast,
};
use yew_hooks::{use_async, use_mount};

use crate::services::config;

#[derive(
    Clone, Copy, Debug, Eq, PartialEq, Ord, PartialOrd, Hash, Serialize, Deserialize, Properties,
)]
pub struct Props {
    pub reading_type: ReadingType,
}

#[function_component(ValidationRule)]
pub fn validation_rule(props: &Props) -> Html {
    let rules = use_state(|| Vec::new());

    {
        let props = props.clone();
        let get_rules =
            use_async(async move { config::get_validation_rules(props.reading_type).await });

        {
            let get_rules = get_rules.clone();
            use_mount(move || get_rules.run());
        }

        let get_rules = get_rules.clone();
        let rules = rules.clone();
        use_effect_with_deps(
            move |get_rules| {
                if let Some(res) = &get_rules.data {
                    rules.set((*res).clone());
                }

                if let Some(err) = &get_rules.error {
                    log::error!("Error getting validation rules: {}", err);
                }
                || ()
            },
            get_rules.clone(),
        );
    }

    let update_rules = {
        let rules = rules.clone();
        let props = props.clone();
        use_async(async move {
            return config::update_validation_rules(props.reading_type, (*rules).clone()).await;
        })
    };

    let add_rule = {
        let rules = rules.clone();
        Callback::from(move |_| {
            rules.set(
                (*rules)
                    .clone()
                    .into_iter()
                    .chain(vec![ValidationRuleDto::default()])
                    .collect(),
            );
        })
    };

    let update_rule = {
        let rules = rules.clone();
        Callback::from(move |(index, rule): (usize, ValidationRuleDto)| {
            rules.set(
                (*rules)
                    .clone()
                    .into_iter()
                    .enumerate()
                    .map(|(i, r)| if i == index { rule.clone() } else { r })
                    .collect(),
            );
        })
    };

    let save_rules = {
        let update_rules = update_rules.clone();
        Callback::from(move |_| {
            update_rules.run();
        })
    };

    let remove_rule = {
        let rules = rules.clone();
        Callback::from(move |index: usize| {
            rules.set(
                (*rules)
                    .clone()
                    .into_iter()
                    .enumerate()
                    .filter(|(i, _)| i != &index)
                    .map(|(_, r)| r)
                    .collect(),
            );
        })
    };

    html! {
        <div class="mt-4 mb-4">
            <h3>{ format!("Validation Rules for {}", props.reading_type.to_string()) }</h3>
            <table class="table">
                <thead>
                    <tr>
                        <th>{ "Message" }</th>
                        <th>{ "Condition" }</th>
                        <th>{ "Actions" }</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        for (*rules).clone().into_iter().enumerate().map(|(i, rule)| {
                            let rule_clone = rule.clone();
                            html! {
                                <tr>
                                    <td>
                                        <input
                                            type="text"
                                            class="form-control"
                                            value={rule.message.clone()}
                                            oninput={update_rule.reform(move |e: InputEvent| (i, ValidationRuleDto {
                                                message: e.target_unchecked_into::<HtmlInputElement>().value(),
                                                ..rule_clone.clone()
                                            }))}
                                        />
                                    </td>
                                    <td>
                                        <input
                                            type="text"
                                            class="form-control"
                                            value={rule.condition.clone()}
                                            oninput={update_rule.reform(move |e: InputEvent| (i, ValidationRuleDto {
                                                condition: e.target_unchecked_into::<HtmlInputElement>().value(),
                                                ..rule.clone()
                                            }))}
                                        />
                                    </td>
                                    <td>
                                        <button class="btn btn-danger" onclick={remove_rule.reform(move |_| i)}>{ "Remove" }</button>
                                    </td>
                                </tr>
                            }
                        })
                    }
                </tbody>
            </table>
            <button class="btn btn-primary" onclick={add_rule}>{ "Add Rule" }</button>
            <button class="btn btn-primary" onclick={save_rules}>{ "Save Rules" }</button>
        </div>
    }
}
