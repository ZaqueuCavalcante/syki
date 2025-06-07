import http from 'k6/http';
import { check } from 'k6';

const JWT = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIwMTk3NGE0MC1mNDkyLTdjMTMtOWMwZC1lMDE2MmVjY2EwYWIiLCJzdWIiOiIwMTk3MzM1NC0yZDRmLTdkZGEtOTdhZS0wYjJjNmQyYjFhNDMiLCJyb2xlIjoiQWNhZGVtaWMiLCJuYW1lIjoiYWNhZGVtaWNAZ21haWwuY29tIiwiZW1haWwiOiJhY2FkZW1pY0BnbWFpbC5jb20iLCJpbnN0aXR1dGlvbiI6ImJjMTM0NTFlLTI1YWEtNGIxZC1iMzI4LTVkZjc2OTAwYTlkYiIsIm5iZiI6MTc0OTI5NzQ2MCwiZXhwIjoxNzQ5NjU3NDYwLCJpYXQiOjE3NDkyOTc0NjAsImlzcyI6ImRldiIsImF1ZCI6ImRldiJ9.yoHucSTpy3_9eYEesDPnw9IASpE96ntEYPvrYSqr_Vs';

export const options = {
    vus: 10,
    duration: '60s',
};

export default function () {
  const params = {
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${JWT}`,
    },
  };

  const res = http.get('http://localhost:5001/academic/courses', params);

  check(res, {
    'status is 2xx': (r) => r.status >= 200 && r.status < 300,
  });
}
