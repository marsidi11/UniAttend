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

import { AssignReaderCommand, ClassroomDto, CreateClassroomCommand, UpdateClassroomCommand } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Classrooms<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Classrooms
   * @name ClassroomsList
   * @request GET:/api/Classrooms
   * @secure
   */
  classroomsList = (params: RequestParams = {}) =>
    this.request<ClassroomDto[], any>({
      path: `/api/Classrooms`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Classrooms
   * @name ClassroomsCreate
   * @request POST:/api/Classrooms
   * @secure
   */
  classroomsCreate = (data: CreateClassroomCommand, params: RequestParams = {}) =>
    this.request<number, any>({
      path: `/api/Classrooms`,
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
   * @tags Classrooms
   * @name ClassroomsDetail
   * @request GET:/api/Classrooms/{id}
   * @secure
   */
  classroomsDetail = (id: number, params: RequestParams = {}) =>
    this.request<ClassroomDto, any>({
      path: `/api/Classrooms/${id}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Classrooms
   * @name ClassroomsUpdate
   * @request PUT:/api/Classrooms/{id}
   * @secure
   */
  classroomsUpdate = (id: number, data: UpdateClassroomCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Classrooms/${id}`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Classrooms
   * @name ClassroomsReaderCreate
   * @request POST:/api/Classrooms/{id}/reader
   * @secure
   */
  classroomsReaderCreate = (id: number, data: AssignReaderCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Classrooms/${id}/reader`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Classrooms
   * @name ClassroomsReaderDelete
   * @request DELETE:/api/Classrooms/{id}/reader
   * @secure
   */
  classroomsReaderDelete = (id: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Classrooms/${id}/reader`,
      method: "DELETE",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Classrooms
   * @name ClassroomsAvailableList
   * @request GET:/api/Classrooms/available
   * @secure
   */
  classroomsAvailableList = (
    query?: {
      /** @format date-time */
      startTime?: string;
      /** @format date-time */
      endTime?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<ClassroomDto[], any>({
      path: `/api/Classrooms/available`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
}
