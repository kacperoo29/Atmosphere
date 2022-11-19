#[derive(thiserror::Error, Clone, Debug, PartialEq)]
pub enum Error {
    #[error("Unknown error {:?}", self)]
    Unknown(String)
}

