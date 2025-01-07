import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { OtpCode, OtpValidationResponse } from '@/types/otp.types';
import { otpApi } from '@/api/endpoints/otpApi';

export const useOtpStore = defineStore('otp', () => {
  // State
  const currentOtp = ref<OtpCode | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Actions
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

  async function validateOtp(code: string, classId: number): Promise<OtpValidationResponse> {
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

  async function requestNewOtp(classId: number) {
    isLoading.value = true;
    try {
      const { data } = await otpApi.requestOtp(classId);
      currentOtp.value = data;
      return data;
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getCurrentOtp(classId: number) {
    isLoading.value = true;
    try {
      const { data } = await otpApi.getCurrentOtp(classId);
      currentOtp.value = data;
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
    // State
    currentOtp,
    isLoading,
    error,

    // Actions
    generateOtp,
    validateOtp,
    requestNewOtp,
    getCurrentOtp
  };
});