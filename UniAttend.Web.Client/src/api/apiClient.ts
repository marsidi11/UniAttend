import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:5255/api', // .NET backend URL
  headers: {
    'Content-Type': 'application/json'
  }
});

// Add request interceptor for auth token
apiClient.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default apiClient;