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

export class StudyGroups<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags StudyGroups
   * @name StudyGroupsList
   * @request GET:/api/StudyGroups
   * @secure
   */
  studyGroupsList = (
    query?: {
      /** @format int32 */
      academicYearId?: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<StudyGroupDto[], any>({
      path: `/api/StudyGroups`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags StudyGroups
   * @name StudyGroupsCreate
   * @request POST:/api/StudyGroups
   * @secure
   */
  studyGroupsCreate = (data: CreateGroupCommand, params: RequestParams = {}) =>
    this.request<StudyGroupDto, any>({
      path: `/api/StudyGroups`,
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
   * @tags StudyGroups
   * @name StudyGroupsDetail
   * @request GET:/api/StudyGroups/{id}
   * @secure
   */
  studyGroupsDetail = (id: number, params: RequestParams = {}) =>
    this.request<StudyGroupDto, any>({
      path: `/api/StudyGroups/${id}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags StudyGroups
   * @name StudyGroupsUpdate
   * @request PUT:/api/StudyGroups/{id}
   * @secure
   */
  studyGroupsUpdate = (id: number, data: UpdateGroupCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/StudyGroups/${id}`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags StudyGroups
   * @name StudyGroupsProfessorDetail
   * @request GET:/api/StudyGroups/professor/{professorId}
   * @secure
   */
  studyGroupsProfessorDetail = (
    professorId: number,
    query?: {
      /** @format int32 */
      academicYearId?: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<StudyGroupDto[], any>({
      path: `/api/StudyGroups/professor/${professorId}`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags StudyGroups
   * @name StudyGroupsStudentsDetail
   * @request GET:/api/StudyGroups/{id}/students
   * @secure
   */
  studyGroupsStudentsDetail = (id: number, params: RequestParams = {}) =>
    this.request<GroupStudentDto[], any>({
      path: `/api/StudyGroups/${id}/students`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags StudyGroups
   * @name StudyGroupsStudentsEnrollCreate
   * @request POST:/api/StudyGroups/{id}/students/enroll
   * @secure
   */
  studyGroupsStudentsEnrollCreate = (id: number, data: EnrollStudentsCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/StudyGroups/${id}/students/enroll`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags StudyGroups
   * @name StudyGroupsStudentsDelete
   * @request DELETE:/api/StudyGroups/{groupId}/students/{studentId}
   * @secure
   */
  studyGroupsStudentsDelete = (groupId: number, studentId: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/StudyGroups/${groupId}/students/${studentId}`,
      method: "DELETE",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags StudyGroups
   * @name StudyGroupsTransferStudentCreate
   * @request POST:/api/StudyGroups/transfer-student
   * @secure
   */
  studyGroupsTransferStudentCreate = (data: TransferStudentCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/StudyGroups/transfer-student`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
}
