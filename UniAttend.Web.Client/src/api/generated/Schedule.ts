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

import { CreateScheduleCommand, ScheduleDto, UpdateScheduleCommand } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Schedule<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Schedule
   * @name ScheduleClassroomDetail
   * @request GET:/api/Schedule/classroom/{classroomId}
   * @secure
   */
  scheduleClassroomDetail = (classroomId: number, params: RequestParams = {}) =>
    this.request<ScheduleDto[], any>({
      path: `/api/Schedule/classroom/${classroomId}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Schedule
   * @name ScheduleGroupDetail
   * @request GET:/api/Schedule/group/{studyGroupId}
   * @secure
   */
  scheduleGroupDetail = (studyGroupId: number, params: RequestParams = {}) =>
    this.request<ScheduleDto[], any>({
      path: `/api/Schedule/group/${studyGroupId}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Schedule
   * @name ScheduleCreate
   * @request POST:/api/Schedule
   * @secure
   */
  scheduleCreate = (data: CreateScheduleCommand, params: RequestParams = {}) =>
    this.request<number, any>({
      path: `/api/Schedule`,
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
   * @tags Schedule
   * @name ScheduleUpdate
   * @request PUT:/api/Schedule/{id}
   * @secure
   */
  scheduleUpdate = (id: number, data: UpdateScheduleCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Schedule/${id}`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Schedule
   * @name ScheduleDelete
   * @request DELETE:/api/Schedule/{id}
   * @secure
   */
  scheduleDelete = (id: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Schedule/${id}`,
      method: "DELETE",
      secure: true,
      ...params,
    });
}
