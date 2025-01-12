/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

import { GenerateOtpRequest, OtpCode, OtpValidationResponse, ValidateOtpRequest } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Otp<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Otp
   * @name AttendanceOtpGenerateCreate
   * @request POST:/api/attendance/otp/generate
   * @secure
   */
  attendanceOtpGenerateCreate = (data: GenerateOtpRequest, params: RequestParams = {}) =>
    this.request<OtpCode, any>({
      path: `/api/attendance/otp/generate`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Otp
   * @name AttendanceOtpValidateCreate
   * @request POST:/api/attendance/otp/validate
   * @secure
   */
  attendanceOtpValidateCreate = (data: ValidateOtpRequest, params: RequestParams = {}) =>
    this.request<OtpValidationResponse, any>({
      path: `/api/attendance/otp/validate`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Otp
   * @name AttendanceOtpCurrentDetail
   * @request GET:/api/attendance/otp/{classId}/current
   * @secure
   */
  attendanceOtpCurrentDetail = (classId: number, params: RequestParams = {}) =>
    this.request<OtpCode, any>({
      path: `/api/attendance/otp/${classId}/current`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
}
