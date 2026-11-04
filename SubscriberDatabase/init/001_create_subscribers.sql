create table if not exists subscribers (
    id integer generated always as identity primary key,
    public_id uuid not null unique,
    email varchar(255) not null,
    subscription_timestamp_utc timestamptz not null default now()
);

create unique index if not exists ux_subscribers_email_lower
    on subscribers (lower(email));