create table if not exists drafts (
    id integer generated always as identity primary key,
    public_id uuid not null unique,
    author varchar(100),
    title varchar(200),
    content text,
    creation_timestamp_utc timestamptz not null,
    last_updated_timestamp_utc timestamptz
);