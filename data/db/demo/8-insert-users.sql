-- CLIENTS
INSERT INTO "user" (id, name, email, role, created_at)
VALUES ('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11', 'User 1', 'user1@gmail.com', 'Client', '2019-01-01 00:00:00');

INSERT INTO account(user_id, password)
VALUES ('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11', 'password');

INSERT INTO client (id, username, avatar_url)
VALUES ('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11', 'user1', 'https://i.imgur.com/fL67hTu.jpeg');

-- OPERATORS
INSERT INTO "user" (id, name, email, role, created_at)
VALUES ('550e8400-e29b-41d4-a716-446655440000', 'User 4', 'user4@gmail.com', 'Operator',
        '2019-01-01 00:00:00'),
       ('f47ac10b-58cc-4372-a567-0e02b2c3d479', 'User 5', 'user5@gmail.com', 'Operator',
        '2019-01-01 00:00:00'),
       ('de305d54-75b4-431b-adb2-eb6b9e546013', 'User 6', 'user6@gmail.com', 'Operator',
        '2019-01-01 00:00:00'),
       ('c694f26a-5184-4e65-9e59-ef461c0eacb3', 'User 7', 'user7@gmail.com', 'Operator',
        '2019-01-01 00:00:00'),
       ('e994f26a-5184-4e65-9e59-ef461c0eacb4', 'User 8', 'user8@gmail.com', 'Operator',
        '2019-01-01 00:00:00');

INSERT INTO account(user_id, password)
VALUES ('550e8400-e29b-41d4-a716-446655440000', 'password'),
       ('f47ac10b-58cc-4372-a567-0e02b2c3d479', 'password'),
       ('de305d54-75b4-431b-adb2-eb6b9e546013', 'password'),
       ('c694f26a-5184-4e65-9e59-ef461c0eacb3', 'password'),
       ('e994f26a-5184-4e65-9e59-ef461c0eacb4', 'password');

INSERT INTO operator (user_id, phone_number)
VALUES ('550e8400-e29b-41d4-a716-446655440000', '923456789'),
       ('f47ac10b-58cc-4372-a567-0e02b2c3d479', '981634321'),
       ('de305d54-75b4-431b-adb2-eb6b9e546013', '987654321'),
       ('c694f26a-5184-4e65-9e59-ef461c0eacb3', '911614321'),
       ('e994f26a-5184-4e65-9e59-ef461c0eacb4', '911614322');
