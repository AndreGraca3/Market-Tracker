drop trigger if exists create_product_stats_trigger on "MarketTracker".product;
drop trigger if exists update_product_stats_trigger on "MarketTracker".product_review;
drop trigger if exists update_favorite_stats_trigger on "MarketTracker".product_favourite;

-- Trigger function to create stats entry for a new product
CREATE OR REPLACE FUNCTION create_product_stats()
    RETURNS TRIGGER AS
$$
BEGIN
    INSERT INTO "MarketTracker".product_stats_counts (product_id)
    VALUES (NEW.id);

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER create_product_stats_trigger
    AFTER INSERT
    ON "MarketTracker".product
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
    SELECT rating INTO old_rating FROM "MarketTracker".product WHERE id = COALESCE(OLD.product_id, NEW.product_id);
    SELECT ratings
    INTO old_count
    FROM "MarketTracker".product_stats_counts
    WHERE product_id = COALESCE(OLD.product_id, NEW.product_id);

    IF TG_OP = 'INSERT' THEN
        UPDATE "MarketTracker".product_stats_counts
        SET ratings = ratings + 1
        WHERE product_id = NEW.product_id;
    ELSIF TG_OP = 'DELETE' THEN
        UPDATE "MarketTracker".product_stats_counts
        SET ratings = ratings - 1
        WHERE product_id = OLD.product_id;
    END IF;

    IF TG_OP = 'INSERT' THEN
        new_count := old_count + 1;
        UPDATE "MarketTracker".product
        SET rating = ((old_rating * old_count) + NEW.rating) / new_count
        WHERE id = NEW.product_id;
    ELSIF TG_OP = 'UPDATE' THEN
        UPDATE "MarketTracker".product
        SET rating = (((old_rating * old_count) - OLD.rating + NEW.rating) / old_count)
        WHERE id = NEW.product_id;
    ELSIF TG_OP = 'DELETE' THEN
        new_count := old_count - 1;
        IF new_count = 0 THEN
            UPDATE "MarketTracker".product
            SET rating = 0
            WHERE id = OLD.product_id;
        ELSE
            UPDATE "MarketTracker".product
            SET rating = (old_rating * old_count) - OLD.rating / new_count
            WHERE id = OLD.product_id;
        END IF;
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_product_stats_trigger
    AFTER INSERT OR UPDATE of rating OR DELETE
    ON "MarketTracker".product_review
    FOR EACH ROW
EXECUTE FUNCTION update_product_stats();

-- Trigger function to update product_stats_counts table when a favorite is added or deleted
CREATE OR REPLACE FUNCTION update_favorite_stats()
    RETURNS TRIGGER AS
$$
BEGIN
    IF TG_OP = 'INSERT' THEN
        UPDATE "MarketTracker".product_stats_counts
        SET favourites = favourites + 1
        WHERE product_id = NEW.product_id;

    ELSIF TG_OP = 'DELETE' THEN
        UPDATE "MarketTracker".product_stats_counts
        SET favourites = favourites - 1
        WHERE product_id = OLD.product_id;

    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_favorite_stats_trigger
    AFTER INSERT OR DELETE
    ON "MarketTracker".product_favourite
    FOR EACH ROW
EXECUTE FUNCTION update_favorite_stats();