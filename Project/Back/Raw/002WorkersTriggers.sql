CREATE OR REPLACE FUNCTION notify_new_domain_event_function()
RETURNS trigger
LANGUAGE 'plpgsql'
AS $BODY$ 
BEGIN
    PERFORM pg_notify('new_domain_event', '');
    RETURN NEW;
END
$BODY$;

CREATE OR REPLACE TRIGGER notify_new_domain_event_trigger
AFTER INSERT ON exato.domain_events
EXECUTE PROCEDURE notify_new_domain_event_function();

---------------------------------------------------------------------------------------------------

CREATE OR REPLACE FUNCTION notify_new_command_function()
RETURNS trigger
LANGUAGE 'plpgsql'
AS $BODY$ 
BEGIN
    PERFORM pg_notify('new_command', '');
    RETURN NEW;
END
$BODY$;

CREATE OR REPLACE TRIGGER notify_new_command_trigger
AFTER INSERT ON exato.commands
EXECUTE PROCEDURE notify_new_command_function();

---------------------------------------------------------------------------------------------------

CREATE OR REPLACE FUNCTION update_command_batch_function()
RETURNS trigger
LANGUAGE 'plpgsql'
AS $BODY$
DECLARE
    batch_status text;
    batch_success boolean;
    batch_next_command_id uuid;
BEGIN
    IF NEW.batch_id IS NULL THEN
        RETURN NEW;
    END IF;

    SELECT
        status, next_command_id INTO batch_status, batch_next_command_id
    FROM exato.command_batches
    WHERE id = NEW.batch_id
    FOR UPDATE;

    IF batch_status = 'Error' THEN
        RETURN NEW;
    END IF;

    IF batch_status = 'Pending' THEN
        UPDATE exato.command_batches
        SET status = 'Processing'
        WHERE id = NEW.batch_id;
    END IF;

    IF NEW.status = 'Error' THEN
        UPDATE exato.command_batches
        SET status = 'Error'
        WHERE id = NEW.batch_id;
        RETURN NEW;
    END IF;

    SELECT count(1) = 0 INTO batch_success
    FROM exato.commands
    WHERE batch_id = NEW.batch_id AND status <> 'Success';

    IF NOT batch_success THEN
        RETURN NEW;
    END IF;

    UPDATE exato.command_batches
    SET status = 'Success', processed_at = now()
    WHERE id = NEW.batch_id;

    IF batch_next_command_id IS NOT NULL THEN
        UPDATE exato.commands
        SET status = 'Pending'
        WHERE id = batch_next_command_id;

        PERFORM pg_notify('new_command', '');
    END IF;

    RETURN NEW;
END
$BODY$;

CREATE OR REPLACE TRIGGER update_command_batch_trigger
AFTER UPDATE ON exato.commands
FOR EACH ROW EXECUTE PROCEDURE update_command_batch_function();
