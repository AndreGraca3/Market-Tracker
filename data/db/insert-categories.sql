truncate table category cascade;
ALTER SEQUENCE category_id_seq RESTART WITH 1;

INSERT INTO category (name)
VALUES ('Alternativas Alimentares'),
       ('Animais'),
       ('Bebés'),
       ('Bebidas'),
       ('Bricolage, Auto e Jardim'),
       ('Casa'),
       ('Charcutaria'),
       ('Congelados'),
       ('Frutas e Legumes'),
       ('Higiene e Beleza'),
       ('Laticínios e Ovos'),
       ('Lazer'),
       ('Mercearia'),
       ('Padaria e Pastelaria'),
       ('Talho e Peixaria');