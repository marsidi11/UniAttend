import { Ref } from 'vue';

export function handleError(err: unknown, errorRef: Ref<string | null>) {
  if (err instanceof Error) {
    errorRef.value = err.message;
  } else if (typeof err === 'object' && err && 'detail' in err) {
    errorRef.value = (err as any).detail?.exceptionMessage || 'An unexpected error occurred';
  } else {
    errorRef.value = 'An unexpected error occurred';
  }
}