import http from 'k6/http';

export const options = {
    vus: 10,
    duration: '30s',
};

export default function () {
    const url = 'http://localhost:5001/users';

    const payload = JSON.stringify({
        email: 'k6@syki.com',
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    http.post(url, payload, params);
}
