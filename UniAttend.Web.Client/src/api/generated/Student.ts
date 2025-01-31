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

import { AssignCardCommand, RegisterStudentCommand } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Student<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Student
   * @name StudentList
   * @request GET:/api/Student
   * @secure
   */
  studentList = (params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Student`,
      method: "GET",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Student
   * @name StudentCreate
   * @request POST:/api/Student
   * @secure
   */
  studentCreate = (data: RegisterStudentCommand, params: RequestParams = {}) =>
    this.request<number, any>({
      path: `/api/Student`,
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
   * @tags Student
   * @name StudentEnrolledGroupsList
   * @request GET:/api/Student/enrolled-groups
   * @secure
   */
  studentEnrolledGroupsList = (params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Student/enrolled-groups`,
      method: "GET",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Student
   * @name StudentAbsenceAlertsList
   * @request GET:/api/Student/absence-alerts
   * @secure
   */
  studentAbsenceAlertsList = (params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Student/absence-alerts`,
      method: "GET",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Student
   * @name StudentCardUpdate
   * @request PUT:/api/Student/{id}/card
   * @secure
   */
  studentCardUpdate = (id: number, data: AssignCardCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Student/${id}/card`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Student
   * @name StudentCardDelete
   * @request DELETE:/api/Student/{id}/card
   * @secure
   */
  studentCardDelete = (id: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Student/${id}/card`,
      method: "DELETE",
      secure: true,
      ...params,
    });
}
