create table if not exists profanities (
    id integer generated always as identity primary key,
    term varchar(100) not null unique
);

create unique index if not exists ux_profanities_term_lower
    on profanities (lower(term));