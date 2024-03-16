drop table if exists list_product;
drop table if exists list;
drop table if exists token;
drop table if exists operator;
drop table if exists client_review;
drop table if exists client;
drop table if exists "user";
drop table if exists price_history;
drop table if exists store;
drop table if exists company;
drop table if exists promotion;
drop table if exists product;
drop table if exists brand;
drop table if exists category;

create table if not exists promotion
(
    id         int generated always as identity primary key,
    percentage int not null check (percentage between 0 and 100),
    discount   int not null
);

create table if not exists brand
(
    id   int generated always as identity primary key,
    name varchar(20) unique not null
);

create table if not exists category
(
    id        int generated always as identity primary key,
    name      varchar(20) unique not null,
    "order"   int                not null,
    parent_id int references category (id) on delete cascade
);

create table if not exists product
(
    id           varchar(13) primary key check (length(id) = 13),
    name         TEXT unique not null,
    description  TEXT        not null,
    image_url    TEXT,
    quantity     int                  default 1,
    unit         varchar(10) not null default 'unidades' check (unit in ('unidades', 'kilogramas', 'gramas', 'litros', 'mililitros')),
    is_available boolean     not null default true,
    last_checked date        not null default now(),
    views        int         not null default 0,
    brand_id     int references brand (id) on delete cascade,
    category_id  int references category (id) on delete cascade,
    rate         float       not null default 1.0 check (rate between 1 and 5)
);

create table if not exists company
(
    id         int generated always as identity primary key,
    name       varchar(20) unique not null,
    created_at date               not null default now()
);

create table if not exists store
(
    id         int generated always as identity primary key,
    address    varchar(200) unique not null,
    city       varchar(30)         not null,
    open_time  date,
    close_time date,
    company_id int references company (id) on delete cascade
);

create table if not exists price_history
(
    id         int generated always as identity primary key,
    price      integer not null,
    product_id varchar(13) references product (id) on delete cascade,
    store_id   int references store (id) on delete cascade,
    date       date    not null default now()
);

create table if not exists "user"
(
    id         uuid primary key default gen_random_uuid(),
    username   varchar(20) unique  not null,
    name       varchar(20),
    email      varchar(200) unique not null,
    password   varchar(30)         not null,
    avatar_url TEXT,
    created_at date                not null default now()
);

create table if not exists client
(
    id uuid primary key references "user" (id) on delete cascade
);

create table if not exists client_review
(
    id         int generated always as identity primary key,
    client_id  uuid references client (id) on delete cascade,
    product_id varchar(13) references product (id) on delete cascade,
    rate       float        not null check (rate between 1 and 5),
    comment    varchar(255) not null,
    created_at date         not null default now()
);

create table if not exists operator
(
    user_id      uuid primary key references "user" (id) on delete cascade,
    store_id     int references store (id) on delete cascade,
    phone_number int not null check (phone_number between 210000000 and 999999999)
);

create table if not exists token
(
    token_value VARCHAR(256) primary key,
    created_at  date not null default now(),
    expires_at  date not null,
    user_id     uuid references "user" (id) on delete cascade
);

create table if not exists list
(
    id          int generated always as identity primary key,
    client_id   uuid references client (id),
    archived_at date not null
);

create table if not exists list_product
(
    list_id    int references list (id) on delete cascade,
    product_id varchar(13) references product (id) on delete cascade,
    store_id   int references store (id) on delete cascade,
    quantity   int not null,
    primary key (list_id, product_id, store_id)
);