use crate::to_title_case;
use atmosphere_api::models::{ReadingType, ValidationRuleDto};
use serde::{Deserialize, Serialize};
use web_sys::{Event, HtmlInputElement, HtmlSelectElement, InputEvent};
use yew::{
    function_component, html, use_effect_with_deps, use_state, Callback, Properties, TargetCast, use_node_ref,
};
use yew_hooks::{use_async, use_mount};

use crate::services::config;

#[derive(
    Clone, Copy, Debug, Eq, PartialEq, Ord, PartialOrd, Hash, Serialize, Deserialize, Properties,
)]
pub struct Props {
    pub reading_type: ReadingType,
    pub device_id: Option<uuid::Uuid>,
}

#[function_component(ValidationRule)]
pub fn validation_rule(props: &Props) -> Html {
    let rules = use_state(|| Vec::new());

    {
        let props = props.clone();
        let get_rules =
            use_async(async move { config::get_validation_rules(props.reading_type, props.device_id).await });

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
            return config::update_validation_rules(props.reading_type, (*rules).clone(), props.device_id).await;
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

    let update_severity = {
        let rules = rules.clone();
        let update_rule = update_rule.clone();
        Callback::from(move |(index, severity): (usize, String)| {
            let mut rule = (*rules).clone().into_iter().nth(index).unwrap();
            rule.severity = match severity.as_str() {
                "Info" => atmosphere_api::models::Severity::Info,
                "Warning" => atmosphere_api::models::Severity::Warning,
                "Error" => atmosphere_api::models::Severity::Error,
                _ => atmosphere_api::models::Severity::Info,
            };

            update_rule.emit((index, rule));
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
                        <th>{ "Severity" }</th>
                        <th>{ "Actions" }</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        for (*rules).clone().into_iter().enumerate().map(|(i, rule)| {
                            let rule_clone = rule.clone();
                            let update_rule = update_rule.clone();
                            let update_severity = update_severity.clone();
                            let remove_rule = remove_rule.clone();
                            html! {
                                <ValidationRuleInternal
                                    rule={rule_clone}
                                    update_rule={update_rule}
                                    update_severity={update_severity}
                                    remove_rule={remove_rule}
                                    i={i}
                                />
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

#[derive(Clone, Debug, PartialEq, Properties)]
struct InternalProp {
    rule: ValidationRuleDto,
    update_rule: Callback<(usize, ValidationRuleDto)>,
    update_severity: Callback<(usize, String)>,
    remove_rule: Callback<usize>,
    i: usize,
}

#[function_component(ValidationRuleInternal)]
fn validation_rule_internal(validation_rule: &InternalProp) -> Html {
    let rule = validation_rule.rule.clone();
    let update_rule = validation_rule.update_rule.clone();
    let update_severity = validation_rule.update_severity.clone();
    let remove_rule = validation_rule.remove_rule.clone();
    let i = validation_rule.i;

    let select_ref = use_node_ref();

    {
        let select_ref = select_ref.clone();
        use_effect_with_deps(
            move |severity| {
                let select = select_ref.cast::<HtmlSelectElement>().unwrap();
                select.set_value(&to_title_case(&severity.to_string()));
                || ()
            },
            rule.severity,
        );
    }

    html! {
        <tr>
            <td>
                <input
                    type="text"
                    class="form-control"
                    value={rule.message.clone()}
                    oninput={let rule = rule.clone(); update_rule.reform(move |e: InputEvent| (i, ValidationRuleDto {
                        message: e.target_unchecked_into::<HtmlInputElement>().value(),
                        ..rule.clone()
                    }))}
                />
            </td>
            <td>
                <input
                    type="text"
                    class="form-control"
                    value={rule.condition.clone()}
                    oninput={let rule = rule.clone(); update_rule.reform(move |e: InputEvent| (i, ValidationRuleDto {
                        condition: e.target_unchecked_into::<HtmlInputElement>().value(),
                        ..rule.clone()
                    }))}
                />
            </td>
            <td>
                <select
                    ref={select_ref.clone()}
                    class="form-control"
                    value={to_title_case(&rule.severity.to_string())}
                    onchange={update_severity.reform(move |e: Event| (i, e.target_unchecked_into::<HtmlSelectElement>().value()))}
                >
                    <option value="Info">{ "Info" }</option>
                    <option value="Warning">{ "Warning" }</option>
                    <option value="Error">{ "Error" }</option>
                </select>
            </td>
            <td>
                <button class="btn btn-danger" onclick={remove_rule.reform(move |_| i)}>{ "Remove" }</button>
            </td>
        </tr>
    }
}
