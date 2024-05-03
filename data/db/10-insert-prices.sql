/*insert into price_entry (price, store_id, product_id)
values (189, 1, 8436048962697), --filipinos
       (189, 2, 8436048962697),
       (90, 3, 8436048962697),
       (90, 4, 8436048962697);

insert into product_availability (store_id, product_id)
values (1, 8436048962697), --filipinos
       (2, 8436048962697),
       (3, 8436048962697),
       (4, 8436048962697);*/

-- Continente
INSERT INTO price_entry (price, store_id, product_id)
VALUES (950, 1, '8445290490155'),
       (130, 1, '8436048962697'),
       (240, 1, '5601001180903'),
       (400, 1, '5601001322402'),
       (600, 1, '5601050206401');

INSERT INTO promotion (percentage, price_entry_id)
VALUES (20, (SELECT id
             FROM price_entry
             WHERE store_id = 1
               AND product_id = '8445290490155'
             LIMIT 1)),
       (10, (SELECT id
             FROM price_entry
             WHERE store_id = 1
               AND product_id = '5601001180903'
             LIMIT 1)),
       (70, (SELECT id
             FROM price_entry
             WHERE store_id = 1
               AND product_id = '8436048962697'
             LIMIT 1)),
       (95, (SELECT id
             FROM price_entry
             WHERE store_id = 1
               AND product_id = '5601050206401'
             LIMIT 1));

INSERT INTO product_availability (store_id, product_id)
values (1, '8445290490155'),
       (1, '5601001180903'),
       (1, '5601001322402'),
       (1, '5601050206401');

INSERT INTO product_availability (store_id, product_id, is_available)
values (1, '8436048962697', false);

       -- Meu Super
INSERT INTO price_entry (price, store_id, product_id)
VALUES (920, 3, '8445290490155'),
       (120, 3, '8436048962697'),
       (230, 3, '5601001180903'),
       (380, 3, '5601001322402'),
       (590, 3, '5601050206401'),
       (920, 4, '8445290490155'),
       (110, 4, '8436048962697'),
       (230, 4, '5601001180903'),
       (380, 4, '5601001322402'),
       (590, 4, '5601050206401');

-- yesterday price, should not be considered in today's price offers
INSERT INTO price_entry (price, store_id, product_id, created_at)
VALUES (2, 4, '5601050206401', '2024-04-22 00:00:00');

INSERT INTO product_availability (store_id, product_id)
values (3, '8445290490155'),
       (3, '8436048962697'),
       (3, '5601001180903'),
--       (3, '5601001322402'), -- not available in meu super
       (3, '5601050206401'),
       (4, '8445290490155'),
       (4, '8436048962697'),
       (4, '5601001180903'),
       (4, '5601001322402'),
       (4, '5601050206401');