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
  AcademicYearReportDto,
  AttendanceReportDto,
  DepartmentReportDto,
  GroupReportDto,
  StudentReportDto,
} from "./data-contracts";
import { HttpClient, RequestParams } from "./http-client";

export class Reports<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Reports
   * @name ReportsStudentsDetail
   * @request GET:/api/Reports/students/{id}
   * @secure
   */
  reportsStudentsDetail = (
    id: number,
    query?: {
      /** @format date-time */
      startDate?: string;
      /** @format date-time */
      endDate?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<StudentReportDto, any>({
      path: `/api/Reports/students/${id}`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Reports
   * @name ReportsMyReportList
   * @request GET:/api/Reports/my-report
   * @secure
   */
  reportsMyReportList = (
    query?: {
      /** @format date-time */
      startDate?: string;
      /** @format date-time */
      endDate?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<StudentReportDto, any>({
      path: `/api/Reports/my-report`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Reports
   * @name ReportsGroupsDetail
   * @request GET:/api/Reports/groups/{id}
   * @secure
   */
  reportsGroupsDetail = (
    id: number,
    query?: {
      /** @format date-time */
      startDate?: string;
      /** @format date-time */
      endDate?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<GroupReportDto, any>({
      path: `/api/Reports/groups/${id}`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Reports
   * @name ReportsDepartmentsDetail
   * @request GET:/api/Reports/departments/{id}
   * @secure
   */
  reportsDepartmentsDetail = (
    id: number,
    query?: {
      /** @format int32 */
      academicYearId?: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<DepartmentReportDto, any>({
      path: `/api/Reports/departments/${id}`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Reports
   * @name ReportsAttendanceList
   * @request GET:/api/Reports/attendance
   * @secure
   */
  reportsAttendanceList = (
    query?: {
      /** @format date-time */
      startDate?: string;
      /** @format date-time */
      endDate?: string;
      /** @format int32 */
      departmentId?: number;
      /** @format int32 */
      subjectId?: number;
      /** @format int32 */
      studyGroupId?: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<AttendanceReportDto, any>({
      path: `/api/Reports/attendance`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Reports
   * @name ReportsAcademicYearsDetail
   * @request GET:/api/Reports/academic-years/{id}
   * @secure
   */
  reportsAcademicYearsDetail = (id: number, params: RequestParams = {}) =>
    this.request<AcademicYearReportDto, any>({
      path: `/api/Reports/academic-years/${id}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Reports
   * @name ReportsExportAttendanceList
   * @request GET:/api/Reports/export/attendance
   * @secure
   */
  reportsExportAttendanceList = (
    query?: {
      /** @format int32 */
      studyGroupId?: number;
      /** @format date-time */
      startDate?: string;
      /** @format date-time */
      endDate?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<void, any>({
      path: `/api/Reports/export/attendance`,
      method: "GET",
      query: query,
      secure: true,
      ...params,
    });
}
