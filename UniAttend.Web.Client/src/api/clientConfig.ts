import type { ApiConfig } from './generated/http-client';
import { useAuthStore } from '@/stores/auth.store';

class ApiError extends Error {
  constructor(
    message: string,
    public status?: number,
    public data?: any
  ) {
    super(message);
    this.name = 'ApiError';
  }
}

interface ApiErrorResponse {
  type: string;
  code: string;
  message: string;
  traceId: string;
  details?: any;
}

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
      return token ? {
        headers: {
          Authorization: `Bearer ${token}`
        },
        credentials: 'include'
      } : undefined;
    },
    customFetch: async (input: RequestInfo | URL, init?: RequestInit) => {
      try {
        const requestInit = {
          ...init,
          credentials: 'include' as RequestCredentials
        };

        const response = await fetch(input, requestInit);

        // Handle specific status codes
        switch (response.status) {

          case 400: {
            const errorData: ApiErrorResponse = await response.json();
            throw new ApiError(
              errorData.message || 'Bad Request',
              400,
              errorData
            );
          }

          case 401: {
            try {
              const authStore = useAuthStore();
              await authStore.refreshTokens();
              return fetch(input, {
                ...requestInit,
                headers: {
                  ...requestInit?.headers,
                  Authorization: `Bearer ${localStorage.getItem('token')}`
                }
              });
            } catch (refreshError) {
              localStorage.clear();
              window.location.href = '/login';
              throw new ApiError('Authentication required', 401);
            }
          }
          case 403:
            throw new ApiError('Access forbidden', 403);
          case 404: {
            const errorData = await response.json().catch(() => ({}));
            throw new ApiError(
              errorData.message || 'Resource not found',
              404,
              errorData
            );
          }
          case 422: {
            const validationData = await response.json();
            throw new ApiError(
              validationData.message || 'Validation failed',
              422,
              validationData
            );
          }
          case 500: {
            const serverError = await response.json();
            throw new ApiError(
              serverError.message || 'Internal server error', // Use server message directly
              500,
              serverError
            );
          }
        }

        if (!response.ok) {
          const errorData = await response.json().catch(() => null);
          throw new ApiError(
            errorData?.message || `HTTP error! status: ${response.status}`,
            response.status,
            errorData
          );
        }

        return response;

      } catch (error: unknown) {
        const requestUrl = input instanceof Request ? input.url : 
                         input instanceof URL ? input.toString() : input;
                         
        console.error('Request failed:', {
          url: requestUrl,
          method: init?.method || 'GET',
          status: error instanceof ApiError ? error.status : undefined,
          message: error instanceof Error ? error.message : 'Unknown error',
          data: error instanceof ApiError ? error.data : undefined
        });
        throw error;
      }
    }
  };
}