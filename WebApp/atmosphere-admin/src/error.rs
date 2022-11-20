use atmosphere_api::apis;

#[derive(thiserror::Error, Clone, Debug, PartialEq)]
pub enum Error {
    #[error("Unauthorized: {0}")]
    Unauthorized(String),
    #[error("Unknown error {0}")]
    Unknown(String),
}

pub fn translate_api_error<T>(err: apis::Error<T>) -> Error {
    match err {
        apis::Error::ResponseError(err) => match err.status.as_u16() {
            401 => Error::Unauthorized(err.content),
            _ => Error::Unknown(err.content),
        }
        _ => Error::Unknown(err.to_string()),
    }
}
