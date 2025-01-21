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

import { ClassDto, OpenClassCommand, ProblemDetails } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Classes<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Classes
   * @name ClassesList
   * @request GET:/api/Classes
   * @secure
   */
  classesList = (
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
    this.request<ClassDto[], any>({
      path: `/api/Classes`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Classes
   * @name ClassesCreate
   * @request POST:/api/Classes
   * @secure
   */
  classesCreate = (data: OpenClassCommand, params: RequestParams = {}) =>
    this.request<ClassDto, ProblemDetails>({
      path: `/api/Classes`,
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
   * @tags Classes
   * @name ClassesCloseCreate
   * @request POST:/api/Classes/{id}/close
   * @secure
   */
  classesCloseCreate = (id: number, params: RequestParams = {}) =>
    this.request<void, ProblemDetails>({
      path: `/api/Classes/${id}/close`,
      method: "POST",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Classes
   * @name ClassesDetail
   * @request GET:/api/Classes/{id}
   * @secure
   */
  classesDetail = (id: number, params: RequestParams = {}) =>
    this.request<ClassDto, ProblemDetails>({
      path: `/api/Classes/${id}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Classes
   * @name ClassesGroupDetail
   * @request GET:/api/Classes/group/{studyGroupId}
   * @secure
   */
  classesGroupDetail = (studyGroupId: number, params: RequestParams = {}) =>
    this.request<ClassDto[], any>({
      path: `/api/Classes/group/${studyGroupId}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Classes
   * @name ClassesClassroomDetail
   * @request GET:/api/Classes/classroom/{classroomId}
   * @secure
   */
  classesClassroomDetail = (classroomId: number, params: RequestParams = {}) =>
    this.request<ClassDto[], any>({
      path: `/api/Classes/classroom/${classroomId}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
}
