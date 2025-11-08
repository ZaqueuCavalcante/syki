CREATE OR REPLACE FUNCTION generate_base36_id(length int DEFAULT 14)
    RETURNS text AS $$
    DECLARE
        alphabet text := '0123456789abcdefghijklmnopqrstuvwxyz';
        result text := '';
        bytes bytea;
        i int;
BEGIN
    bytes := gen_random_bytes(length);
    
    FOR i IN 0..length-1 LOOP
        result := result || substr(alphabet, (get_byte(bytes, i) % 36) + 1, 1);
    END LOOP;
    
    RETURN result;
END;
$$ LANGUAGE plpgsql;
