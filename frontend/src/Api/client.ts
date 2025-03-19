import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'https://localhost:7104/api',
  headers: {
    'Content-Type': 'application/json',
    'Jwt-Key': 'ThisIsASecretKeyForJwtTokenDontShareIt',
    'Jwt-Issuer': 'https://localhost:7104',
    'Jwt-Audience': 'https://localhost:7104'
  }
});

export default apiClient;
