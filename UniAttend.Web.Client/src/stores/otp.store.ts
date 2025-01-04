import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { OtpCode } from '@/types/alert.types';
import { otpApi } from '@/api/endpoints/otpApi';

export const useOtpStore = defineStore('otp', () => {
  const isLoading = ref(false);
  const error = ref<string | null>(null);
  const currentOtp = ref<OtpCode | null>(null);

  async function generateOtp(classId: number) {
    isLoading.value = true;
    try {
      const { data } = await otpApi.generate(classId);
      currentOtp.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function validateOtp(code: string, classId: number) {
    isLoading.value = true;
    try {
      const { data } = await otpApi.validate(code, classId);
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  function handleError(err: unknown) {
    error.value = err instanceof Error ? err.message : 'An error occurred';
  }

  return {
    isLoading,
    error,
    currentOtp,
    generateOtp,
    validateOtp
  };
});