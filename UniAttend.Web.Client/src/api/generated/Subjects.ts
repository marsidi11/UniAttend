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

import { CreateSubjectCommand, SubjectDto, UpdateSubjectCommand } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Subjects<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Subjects
   * @name SubjectsList
   * @request GET:/api/Subjects
   * @secure
   */
  subjectsList = (
    query?: {
      /** @format int32 */
      departmentId?: number;
      isActive?: boolean;
    },
    params: RequestParams = {},
  ) =>
    this.request<SubjectDto[], any>({
      path: `/api/Subjects`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Subjects
   * @name SubjectsCreate
   * @request POST:/api/Subjects
   * @secure
   */
  subjectsCreate = (data: CreateSubjectCommand, params: RequestParams = {}) =>
    this.request<SubjectDto, any>({
      path: `/api/Subjects`,
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
   * @tags Subjects
   * @name SubjectsDetail
   * @request GET:/api/Subjects/{id}
   * @secure
   */
  subjectsDetail = (id: number, params: RequestParams = {}) =>
    this.request<SubjectDto, any>({
      path: `/api/Subjects/${id}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Subjects
   * @name SubjectsUpdate
   * @request PUT:/api/Subjects/{id}
   * @secure
   */
  subjectsUpdate = (id: number, data: UpdateSubjectCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Subjects/${id}`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Subjects
   * @name SubjectsDelete
   * @request DELETE:/api/Subjects/{id}
   * @secure
   */
  subjectsDelete = (id: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Subjects/${id}`,
      method: "DELETE",
      secure: true,
      ...params,
    });
}
