CREATE FUNCTION public.dd_get_all_child_ids_by_uid(p_parent_transaction_uid public.citext, p_consulta_resultado_tipo_id smallint)
    RETURNS TABLE(consulta_id integer, uid_base36 public.citext)
    LANGUAGE SQL STABLE STRICT COST 1 ROWS 5 PARALLEL SAFE
AS $$
    WITH RECURSIVE childs AS (
        -- Anchor
        SELECT c.consulta_id, c.uid_base36, c.consulta_resultado_tipo_id
        FROM public.consulta AS c
        WHERE c.master_uid = p_parent_transaction_uid
        UNION ALL
        -- Recursive Member
        SELECT c.consulta_id, c.uid_base36, c.consulta_resultado_tipo_id
        FROM public.consulta AS c
        INNER JOIN childs AS parent ON parent.consulta_id = c.consulta_master_id
    )
    SELECT consulta_id, uid_base36
    FROM childs
    WHERE
        uid_base36 != p_parent_transaction_uid
            AND
        consulta_resultado_tipo_id = p_consulta_resultado_tipo_id
    ORDER BY consulta_id;
$$;

CREATE FUNCTION public.dd_get_all_child_cliente_ids(master_cliente_id integer)
    RETURNS TABLE(cliente_id integer)
    stable
    strict
    parallel safe
    cost 1
    rows 5
    LANGUAGE SQL
AS $$
WITH RECURSIVE descendentes AS (
    -- Anchor
    SELECT c.cliente_id
    FROM public.cliente AS c
    WHERE c.parent_organization_id = master_cliente_id

    UNION ALL

    -- Recursive Member
    SELECT c.cliente_id
    FROM public.cliente AS c
    INNER JOIN descendentes d ON c.parent_organization_id = d.cliente_id
)
SELECT cliente_id FROM descendentes;
$$;
