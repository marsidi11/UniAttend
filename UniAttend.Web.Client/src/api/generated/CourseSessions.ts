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

import { CourseSessionDto, OpenCourseSessionCommand, ProblemDetails } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class CourseSessions<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags CourseSessions
   * @name CourseSessionsList
   * @request GET:/api/CourseSessions
   * @secure
   */
  courseSessionsList = (
    query?: {
      /** @format int32 */
      studyGroupId?: number;
      /** @format int32 */
      classroomId?: number;
      /** @format date-time */
      date?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<CourseSessionDto[], any>({
      path: `/api/CourseSessions`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags CourseSessions
   * @name CourseSessionsCreate
   * @request POST:/api/CourseSessions
   * @secure
   */
  courseSessionsCreate = (data: OpenCourseSessionCommand, params: RequestParams = {}) =>
    this.request<CourseSessionDto, ProblemDetails>({
      path: `/api/CourseSessions`,
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
   * @tags CourseSessions
   * @name CourseSessionsCloseCreate
   * @request POST:/api/CourseSessions/{id}/close
   * @secure
   */
  courseSessionsCloseCreate = (id: number, params: RequestParams = {}) =>
    this.request<void, ProblemDetails>({
      path: `/api/CourseSessions/${id}/close`,
      method: "POST",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags CourseSessions
   * @name CourseSessionsDetail
   * @request GET:/api/CourseSessions/{id}
   * @secure
   */
  courseSessionsDetail = (id: number, params: RequestParams = {}) =>
    this.request<CourseSessionDto, ProblemDetails>({
      path: `/api/CourseSessions/${id}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags CourseSessions
   * @name CourseSessionsGroupDetail
   * @request GET:/api/CourseSessions/group/{studyGroupId}
   * @secure
   */
  courseSessionsGroupDetail = (studyGroupId: number, params: RequestParams = {}) =>
    this.request<CourseSessionDto[], any>({
      path: `/api/CourseSessions/group/${studyGroupId}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags CourseSessions
   * @name CourseSessionsClassroomDetail
   * @request GET:/api/CourseSessions/classroom/{classroomId}
   * @secure
   */
  courseSessionsClassroomDetail = (classroomId: number, params: RequestParams = {}) =>
    this.request<CourseSessionDto[], any>({
      path: `/api/CourseSessions/classroom/${classroomId}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
}
