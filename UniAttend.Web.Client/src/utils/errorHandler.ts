import { Ref } from 'vue';
import { useToast, POSITION } from 'vue-toastification';

interface ErrorResponse {
  type?: string;
  code?: string;
  message?: string;
  detail?: {
    exceptionMessage?: string;
    stackTrace?: string;
    source?: string;
  };
}

export function handleError(err: unknown, errorRef?: Ref<string | null>) {
  const toast = useToast();
  let errorMessage: string;

  if (err instanceof Error) {
    // Handle API errors with response data
    const apiError = err as any;
    const errorData: ErrorResponse = apiError.response?.data;

    errorMessage = errorData?.detail?.exceptionMessage || 
                  errorData?.message ||
                  apiError.message ||
                  'An unexpected error occurred';
  } else if (typeof err === 'object' && err && 'detail' in err) {
    // Handle direct error objects
    const errorData = err as ErrorResponse;
    errorMessage = errorData.detail?.exceptionMessage || 
                  errorData.message || 
                  'An unexpected error occurred';
  } else {
    errorMessage = 'An unexpected error occurred';
  }

  // Update error ref if provided
  if (errorRef) {
    errorRef.value = errorMessage;
  }

  // Show toast notification
  toast.error(errorMessage, {
    position: POSITION.TOP_RIGHT,
    timeout: 5000,
    closeOnClick: true,
    pauseOnHover: true
  });

  // Log error for debugging
  console.error('Error details:', err);
}