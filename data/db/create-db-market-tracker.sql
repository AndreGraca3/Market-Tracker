drop table if exists post_comment;
drop table if exists post;
drop table if exists favorite;
drop table if exists list_product;
drop table if exists list;
drop table if exists token;
drop table if exists product_review;
drop table if exists moderator;
drop table if exists operator;
drop table if exists client;
drop table if exists "user";
drop table if exists promotion;
drop table if exists price_history;
drop table if exists last_checked;
drop table if exists store;
drop table if exists product;
drop table if exists brand;
drop table if exists category;
drop table if exists company;

create table if not exists brand
(
    id   int generated always as identity primary key,
    name varchar(50) unique not null
);

create table if not exists category
(
    id   int generated always as identity primary key,
    name varchar(50) unique not null
);

create table if not exists product
(
    id          decimal(13) primary key,
    name        varchar(100) not null,
    image_url   TEXT         not null,
    quantity    int                   default 1,
    unit        varchar(20)  not null default 'unidades' check (unit in ('unidades', 'kilogramas', 'gramas', 'litros', 'mililitros')),
    views       int          not null default 0,
    rate        float        not null default 0,
    brand_id    int references brand (id) on delete cascade,
    category_id int references category (id) on delete cascade
);

create table if not exists company
(
    id         int generated always as identity primary key,
    name       varchar(30) unique not null,
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
    price      integer not null,
    date       date    not null default now(),
    store_id   int references store (id) on delete cascade,
    product_id int references product (id) on delete cascade,
    primary key (product_id, store_id, date)
);

create table if not exists last_checked
(
    is_available boolean not null default true,
    date         date    not null default now(),
    product_id   int references product (id) on delete cascade,
    store_id     int references store (id) on delete cascade,
    primary key (product_id, store_id)
);

create table if not exists promotion
(
    percentage int not null check (percentage between 0 and 100),
    discount   int not null,
    product_id int,
    store_id   int,
    date       date,
    foreign key (product_id, store_id, date) references price_history (product_id, store_id, date) on delete cascade,
    primary key (product_id, store_id, date)
);

create table if not exists last_checked
(
    product_id int references product (id) on delete cascade,
    store_id   int references store (id) on delete cascade,
    date       date not null default now(),
    primary key (product_id, store_id)
);

create table if not exists "user"
(
    id         uuid primary key             default gen_random_uuid(),
    username   varchar(20) unique  not null,
    name       varchar(20),
    email      varchar(200) unique not null,
    password   varchar(30)         not null,
    created_at date                not null default now()
);

create table if not exists client
(
    id         uuid primary key references "user" (id) on delete cascade,
    avatar_url TEXT
);

create table if not exists operator
(
    user_id      uuid primary key references "user" (id) on delete cascade,
    store_id     int references store (id) on delete cascade,
    phone_number int not null check (phone_number between 210000000 and 999999999)
);

create table if not exists moderator
(
    user_id uuid primary key references "user" (id) on delete cascade
);

create table if not exists token
(
    token_value VARCHAR(256) primary key,
    created_at  date not null default now(),
    expires_at  date not null,
    user_id     uuid references "user" (id) on delete cascade
);

create table if not exists product_review
(
    client_id  uuid references client (id) on delete cascade,
    product_id int references product (id) on delete cascade,
    rate       int          not null check (rate between 1 and 5),
    text       varchar(255) not null,
    created_at date         not null default now(),
    primary key (client_id, product_id)
);

create table if not exists favorite
(
    client_id  uuid references client (id) on delete cascade,
    product_id int references product (id) on delete cascade,
    date       date not null default now(),
    primary key (client_id, product_id)
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
    product_id int references product (id) on delete cascade,
    store_id   int references store (id) on delete cascade,
    quantity   int not null,
    primary key (list_id, product_id, store_id)
);

create table if not exists post
(
    id         int generated always as identity primary key,
    title      varchar(20)  not null,
    text       varchar(255) not null,
    created_at date         not null default now(),
    client_id  uuid references client (id) on delete cascade,
    list_id    int references list (id) on delete cascade
);

create table if not exists post_comment
(
    text       varchar(255) not null,
    created_at date         not null default now(),
    client_id  uuid references client (id) on delete cascade,
    post_id    int references post (id) on delete cascade,
    primary key (client_id, post_id)
);