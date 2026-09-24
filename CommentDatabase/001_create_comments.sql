create table if not exists comments (
    id integer generated always as identity primary key,
    public_id uuid not null unique,
    article_id uuid not null,
    author varchar(100) not null,
    content varchar(2000) not null,
    creation_timestamp_utc timestamptz not null,
    last_updated_timestamp_utc timestamptz
);