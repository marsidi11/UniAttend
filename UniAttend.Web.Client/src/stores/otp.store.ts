import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { 
  OtpCode, 
  OtpValidationResponse, 
  GenerateOtpRequest,
  ValidateOtpRequest 
} from '@/api/generated/data-contracts';
import { otpApi } from '@/api/apiInstances';
import { handleError } from '@/utils/errorHandler';

export const useOtpStore = defineStore('otp', () => {
  // State
  const currentOtp = ref<OtpCode | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Actions
  async function generateOtp(classId: number, studentId: number) {
    isLoading.value = true;
    try {
      const { data } = await otpApi.attendanceOtpGenerateCreate({ 
        classId,
        studentId 
      });
      currentOtp.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function validateOtp(code: string, classId: number): Promise<OtpValidationResponse> {
    isLoading.value = true;
    try {
      const { data } = await otpApi.attendanceOtpValidateCreate({
        code,
        classId
      });
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getCurrentOtp(classId: number) {
    isLoading.value = true;
    try {
      const { data } = await otpApi.attendanceOtpCurrentDetail(classId);
      currentOtp.value = data;
      return data;
    } catch (err) {
      handleError(err, error);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  return {
    // State
    currentOtp,
    isLoading,
    error,

    // Actions
    generateOtp,
    validateOtp,
    getCurrentOtp
  };
});