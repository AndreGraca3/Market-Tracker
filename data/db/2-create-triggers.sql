-- Trigger function to create stats entry for a new product
CREATE OR REPLACE FUNCTION create_product_stats()
    RETURNS TRIGGER AS
$$
BEGIN
    INSERT INTO product_stats_counts (product_id)
    VALUES (NEW.id);

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER create_product_stats_trigger
    AFTER INSERT
    ON product
    FOR EACH ROW
EXECUTE FUNCTION create_product_stats();

-- Trigger function to update product_stats_counts table
CREATE OR REPLACE FUNCTION update_product_stats()
    RETURNS TRIGGER AS
$$
DECLARE
    old_rating FLOAT;
    old_count  INTEGER;
    new_count  INTEGER;
BEGIN
    SELECT rating INTO old_rating FROM product WHERE id = NEW.product_id;
    SELECT ratings INTO old_count FROM product_stats_counts WHERE product_id = NEW.product_id;

    IF TG_OP = 'INSERT' THEN
        UPDATE product_stats_counts
        SET ratings = ratings + 1
        WHERE product_id = NEW.product_id;
    ELSIF TG_OP = 'DELETE' THEN
        UPDATE product_stats_counts
        SET ratings = ratings - 1
        WHERE product_id = OLD.product_id;
    END IF;

    IF TG_OP = 'INSERT' THEN
        new_count := old_count + 1;
        UPDATE product
        SET rating = ((old_rating * old_count) + NEW.rating) / new_count
        WHERE id = NEW.product_id;
    ELSIF TG_OP = 'UPDATE' THEN
        UPDATE product
        SET rating = (((old_rating * old_count) - OLD.rating + NEW.rating) / old_count)
        WHERE id = NEW.product_id;
    ELSIF TG_OP = 'DELETE' THEN
        new_count := old_count - 1;
        UPDATE product
        SET rating = old_rating + (NEW.rating - old_rating) / new_count
        WHERE id = OLD.product_id;
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_product_stats_trigger
    AFTER INSERT OR UPDATE of rating OR DELETE
    ON product_review
    FOR EACH ROW
EXECUTE FUNCTION update_product_stats();

-- Trigger function to update product_stats_counts table when a favorite is added or deleted
CREATE OR REPLACE FUNCTION update_favorite_stats()
    RETURNS TRIGGER AS
$$
BEGIN
    IF TG_OP = 'INSERT' THEN
        UPDATE product_stats_counts
        SET favourites = favourites + 1
        WHERE product_id = NEW.product_id;

    ELSIF TG_OP = 'DELETE' THEN
        UPDATE product_stats_counts
        SET favourites = favourites - 1
        WHERE product_id = OLD.product_id;

    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_favorite_stats_trigger
    AFTER INSERT OR DELETE
    ON product_favourite
    FOR EACH ROW
EXECUTE FUNCTION update_favorite_stats();

-- Trigger function to update product_stats_counts table when a product is added to a user's list
CREATE OR REPLACE FUNCTION update_list_stats()
    RETURNS TRIGGER AS
$$
BEGIN
    IF TG_OP = 'INSERT' THEN
        UPDATE product_stats_counts
        SET lists = lists + 1
        WHERE product_id = NEW.product_id;

    ELSIF TG_OP = 'DELETE' THEN
        UPDATE product_stats_counts
        SET lists = lists - 1
        WHERE product_id = OLD.product_id;

    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_list_stats_trigger
    AFTER INSERT OR DELETE
    ON list_entry
    FOR EACH ROW
EXECUTE FUNCTION update_list_stats();