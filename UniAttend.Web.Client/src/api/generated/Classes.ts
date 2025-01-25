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

export class courseSessions<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags courseSessions
   * @name courseSessionsList
   * @request GET:/api/courseSessions
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
      path: `/api/courseSessions`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags courseSessions
   * @name courseSessionsCreate
   * @request POST:/api/courseSessions
   * @secure
   */
  courseSessionsCreate = (data: OpenCourseSessionCommand, params: RequestParams = {}) =>
    this.request<CourseSessionDto, ProblemDetails>({
      path: `/api/courseSessions`,
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
   * @tags courseSessions
   * @name courseSessionsCloseCreate
   * @request POST:/api/courseSessions/{id}/close
   * @secure
   */
  courseSessionsCloseCreate = (id: number, params: RequestParams = {}) =>
    this.request<void, ProblemDetails>({
      path: `/api/courseSessions/${id}/close`,
      method: "POST",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags courseSessions
   * @name courseSessionsDetail
   * @request GET:/api/courseSessions/{id}
   * @secure
   */
  courseSessionsDetail = (id: number, params: RequestParams = {}) =>
    this.request<CourseSessionDto, ProblemDetails>({
      path: `/api/courseSessions/${id}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags courseSessions
   * @name courseSessionsGroupDetail
   * @request GET:/api/courseSessions/group/{studyGroupId}
   * @secure
   */
  courseSessionsGroupDetail = (studyGroupId: number, params: RequestParams = {}) =>
    this.request<CourseSessionDto[], any>({
      path: `/api/courseSessions/group/${studyGroupId}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags courseSessions
   * @name courseSessionsClassroomDetail
   * @request GET:/api/courseSessions/classroom/{classroomId}
   * @secure
   */
  courseSessionsClassroomDetail = (classroomId: number, params: RequestParams = {}) =>
    this.request<CourseSessionDto[], any>({
      path: `/api/courseSessions/classroom/${classroomId}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
}
