export function get()
{
    return document.cookie;
}

export function set(key, value)
{
    document.cookie = `${key}=${value}`;
}
