

create table if not exists brand 
(
    id int generated always as identity primary key,
    name varchar(20) unique not null
);

create table if not exists product
(
    id     varchar(13) primary key,
    name TEXT unique not null,
    description TEXT not null,
    imageUrl TEXT,
    quantity int default 1,
    unit varchar(10),
    rate float default 0.0
);

create table if not exists company
(
    id int generated always as identity primary key,
    name varchar(20) unique not null,
    created_at date not null default now()
);


create table if not exists store
(
    id int generated always as identity primary key,
    address varchar(200) unique not null,
    city varchar(30) not null,
    openTime date,
    closeTime date,
    company_id int references company (id) on delete cascade
);

create table if not exists "user"
(
    id         int generated always as identity primary key,
    username   varchar(20) unique not null
    name       varchar(20),
    email      varchar(200) unique not null,
    password   varchar(30) not null,
    avatar_url TEXT,
    created_at date                not null                                     default now()
);

create table if not exists client
(
    user_id int primary key references "user" (id) on delete cascade
);

create table if not exists operator
(
    user_id int primary references "user" (id) on delete cascade,
    store_id int references store (id) on delete cascade,
    phone_number int not null check (LENGTH(phone_number) = 9)
);

create table if not exists token
(
    token_value VARCHAR(256) primary key,
    created_at  date not null default now(),
    expires_at  date not null,
    user_id     int references "user" (id) on delete cascade
);

create table if not exists list
(
    id int generated always as identity primary key,
    client_id int references client(id) primary key,
    closed_at date not null
);