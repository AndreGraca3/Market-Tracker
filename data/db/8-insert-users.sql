-- CLIENTS
INSERT INTO "user" (id, name, email, role, created_at)
VALUES ('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11', 'User 1', 'user1@gmail.com', 'Client', '2019-01-01 00:00:00'),
       ('b1eebc99-9c0b-4ef8-bb6d-6bb9bd380a12', 'User 2', 'user2@gmail.com', 'Client', '2019-01-01 00:00:00'),
       ('c2eebc99-9c0b-4ef8-bb6d-6bb9bd380a13', 'User 3', 'user3@gmail.com', 'Client', '2019-01-01 00:00:00');

INSERT INTO account(user_id, password)
VALUES ('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11', 'password'),
       ('b1eebc99-9c0b-4ef8-bb6d-6bb9bd380a12', 'password'),
       ('c2eebc99-9c0b-4ef8-bb6d-6bb9bd380a13', 'password');

INSERT INTO client (id, username, avatar_url)
VALUES ('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11', 'user1', 'http://example.com/avatar1.png'),
       ('b1eebc99-9c0b-4ef8-bb6d-6bb9bd380a12', 'user2', 'http://example.com/avatar2.png'),
       ('c2eebc99-9c0b-4ef8-bb6d-6bb9bd380a13', 'user3', 'http://example.com/avatar3.png');

-- OPERATORS
INSERT INTO "user" (id, name, email, role, created_at)
VALUES ('550e8400-e29b-41d4-a716-446655440000', 'User 4', 'user4@gmail.com', 'Operator',
        '2019-01-01 00:00:00'),
       ('f47ac10b-58cc-4372-a567-0e02b2c3d479', 'User 5', 'user5@gmail.com', 'Operator',
        '2019-01-01 00:00:00'),
       ('de305d54-75b4-431b-adb2-eb6b9e546013', 'User 6', 'user6@gmail.com', 'Operator',
        '2019-01-01 00:00:00'),
       ('16379fa8-c2cd-4e1a-bdb3-0c2e16a7d769', 'User 7', 'user7@gmail.com', 'Operator',
        '2019-01-01 00:00:00'),
       ('2d5a08c9-2a52-4d2d-a65f-7d5bf5fc6b41', 'User 8', 'user8@gmail.com', 'Operator',
        '2019-01-01 00:00:00');

INSERT INTO account(user_id, password)
VALUES ('550e8400-e29b-41d4-a716-446655440000', 'password'),
       ('f47ac10b-58cc-4372-a567-0e02b2c3d479', 'password'),
       ('de305d54-75b4-431b-adb2-eb6b9e546013', 'password'),
       ('16379fa8-c2cd-4e1a-bdb3-0c2e16a7d769', 'password'),
       ('2d5a08c9-2a52-4d2d-a65f-7d5bf5fc6b41', 'password');

INSERT INTO operator (user_id, phone_number)
VALUES ('550e8400-e29b-41d4-a716-446655440000', '923456789'),
       ('f47ac10b-58cc-4372-a567-0e02b2c3d479', '981634321'),
       ('de305d54-75b4-431b-adb2-eb6b9e546013', '987654321'),
       ('16379fa8-c2cd-4e1a-bdb3-0c2e16a7d769', '945654321'),
       ('2d5a08c9-2a52-4d2d-a65f-7d5bf5fc6b41', '932458131');
