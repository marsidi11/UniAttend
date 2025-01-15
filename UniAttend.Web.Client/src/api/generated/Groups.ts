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
  CreateGroupCommand,
  EnrollStudentsCommand,
  GroupStudentDto,
  StudyGroupDto,
  TransferStudentCommand,
  UpdateGroupCommand,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Groups<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  groupsDetail(id: number): { data: any; } | PromiseLike<{ data: any; }> {
    throw new Error('Method not implemented.');
  }
  groupsList(): { data: any; } | PromiseLike<{ data: any; }> {
    throw new Error('Method not implemented.');
  }
  /**
   * No description
   *
   * @tags Groups
   * @name GroupsProfessorDetail
   * @request GET:/api/Groups/professor/{professorId}
   * @secure
   */
  groupsProfessorDetail = (
    professorId: number,
    query?: {
      /** @format int32 */
      academicYearId?: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<StudyGroupDto[], any>({
      path: `/api/Groups/professor/${professorId}`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Groups
   * @name GroupsStudentsDetail
   * @request GET:/api/Groups/{id}/students
   * @secure
   */
  groupsStudentsDetail = (id: number, params: RequestParams = {}) =>
    this.request<GroupStudentDto[], any>({
      path: `/api/Groups/${id}/students`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Groups
   * @name GroupsCreate
   * @request POST:/api/Groups
   * @secure
   */
  groupsCreate = (data: CreateGroupCommand, params: RequestParams = {}) =>
    this.request<StudyGroupDto, any>({
      path: `/api/Groups`,
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
   * @tags Groups
   * @name GroupsUpdate
   * @request PUT:/api/Groups/{id}
   * @secure
   */
  groupsUpdate = (id: number, data: UpdateGroupCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Groups/${id}`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Groups
   * @name GroupsStudentsEnrollCreate
   * @request POST:/api/Groups/{id}/students/enroll
   * @secure
   */
  groupsStudentsEnrollCreate = (id: number, data: EnrollStudentsCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Groups/${id}/students/enroll`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Groups
   * @name GroupsStudentsDelete
   * @request DELETE:/api/Groups/{groupId}/students/{studentId}
   * @secure
   */
  groupsStudentsDelete = (groupId: number, studentId: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Groups/${groupId}/students/${studentId}`,
      method: "DELETE",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Groups
   * @name GroupsTransferStudentCreate
   * @request POST:/api/Groups/transfer-student
   * @secure
   */
  groupsTransferStudentCreate = (data: TransferStudentCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Groups/transfer-student`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
}
