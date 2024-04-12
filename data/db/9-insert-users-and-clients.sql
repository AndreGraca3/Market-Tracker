-- USERS
INSERT INTO "user" (id, username, name, email, password, created_at) VALUES
('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11', 'user1', 'User 1', 'user1@gmail.com', 'password', '2019-01-01 00:00:00'),
('b1eebc99-9c0b-4ef8-bb6d-6bb9bd380a12', 'user2', 'User 2', 'user2@gmail.com', 'password', '2019-01-01 00:00:00'),
('c2eebc99-9c0b-4ef8-bb6d-6bb9bd380a13', 'user3', 'User 3', 'user3@gmail.com', 'password', '2019-01-01 00:00:00');

-- CLIENTS
INSERT INTO client (id, avatar_url) VALUES
('a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11', 'http://example.com/avatar1.png'),
('b1eebc99-9c0b-4ef8-bb6d-6bb9bd380a12', 'http://example.com/avatar2.png'),
('c2eebc99-9c0b-4ef8-bb6d-6bb9bd380a13', 'http://example.com/avatar3.png');