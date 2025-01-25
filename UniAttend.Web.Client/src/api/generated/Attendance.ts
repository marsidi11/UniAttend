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

import {
  AttendanceRecordDto,
  RecordCardAttendanceCommand,
  RecordOtpAttendanceCommand,
  TotpSetupDto,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Attendance<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Attendance
   * @name AttendanceSetupTotpCreate
   * @request POST:/api/Attendance/setup-totp
   * @secure
   */
  attendanceSetupTotpCreate = (params: RequestParams = {}) =>
    this.request<TotpSetupDto, any>({
      path: `/api/Attendance/setup-totp`,
      method: "POST",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Attendance
   * @name AttendanceCardCreate
   * @request POST:/api/Attendance/card
   * @secure
   */
  attendanceCardCreate = (data: RecordCardAttendanceCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Attendance/card`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Attendance
   * @name AttendanceOtpCreate
   * @request POST:/api/Attendance/otp
   * @secure
   */
  attendanceOtpCreate = (data: RecordOtpAttendanceCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Attendance/otp`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Attendance
   * @name AttendanceCourseSessionsConfirmCreate
   * @request POST:/api/Attendance/courseSessions/{courseSessionId}/confirm
   * @secure
   */
  attendanceCourseSessionsConfirmCreate = (courseSessionId: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Attendance/courseSessions/${courseSessionId}/confirm`,
      method: "POST",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Attendance
   * @name AttendanceCourseSessionsDetail
   * @request GET:/api/Attendance/courseSessions/{courseSessionId}
   * @secure
   */
  attendanceCourseSessionsDetail = (
    courseSessionId: number,
    query?: {
      /** @format date-time */
      date?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<AttendanceRecordDto[], any>({
      path: `/api/Attendance/courseSessions/${courseSessionId}`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Attendance
   * @name AttendanceStudentList
   * @request GET:/api/Attendance/student
   * @secure
   */
  attendanceStudentList = (
    query?: {
      /** @format date-time */
      startDate?: string;
      /** @format date-time */
      endDate?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<AttendanceRecordDto[], any>({
      path: `/api/Attendance/student`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
}
