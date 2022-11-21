use atmosphere_api::apis;

#[derive(thiserror::Error, Clone, Debug, PartialEq)]
pub enum Error {
    #[error("Unauthorized: {0}")]
    Unauthorized(String),
    #[error("Unknown error {0}")]
    Unknown(String),
}

impl<T> Into<Error> for apis::Error<T> {
    fn into(self) -> Error {
        match self {
            apis::Error::ResponseError(err) => match err.status.as_u16() {
                401 => Error::Unauthorized(err.content),
                _ => Error::Unknown(err.content),
            }
            _ => Error::Unknown(self.to_string()),
        }
    }
}
