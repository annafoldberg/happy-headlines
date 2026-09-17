create table if not exists articles (
    id integer generated always as identity primary key,
    public_id uuid not null unique,
    publication_timestamp_utc timestamptz not null,
    last_updated_timestamp_utc timestamptz,
    author text not null,
    title text not null,
    content text not null
);