import type { ApiConfig } from './generated/http-client';
import { useAuthStore } from '@/stores/auth.store';

export function createApiConfig<SecurityDataType>(): ApiConfig<SecurityDataType> {
  return {
    baseUrl: 'http://localhost:5255',
    baseApiParams: {
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      }
    },
    securityWorker: async () => {
      const token = localStorage.getItem('token');
      
      if (!token) {
        return;
      }

      return {
        headers: {
          Authorization: `Bearer ${token}`
        },
        credentials: 'include'
      };
    },
    customFetch: async (input: RequestInfo | URL, init?: RequestInit) => {
      try {
        // Add credentials to all requests
        const requestInit = {
          ...init,
          credentials: 'include' as RequestCredentials
        };

        const response = await fetch(input, requestInit);

        if (response.status === 401) {
          try {
            const authStore = useAuthStore();
            await authStore.refreshTokens();
            
            const newInit = {
              ...requestInit,
              headers: {
                ...requestInit?.headers,
                Authorization: `Bearer ${localStorage.getItem('token')}`
              }
            };
            
            return fetch(input, newInit);
          } catch (refreshError) {
            localStorage.clear();
            window.location.href = '/login';
            throw new Error('Authentication required');
          }
        }
        
        return response;
      } catch (error) {
        console.error('Request failed:', error);
        throw error;
      }
    }
  };
}