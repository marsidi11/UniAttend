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

import { CreateDepartmentCommand, DepartmentDto, UpdateDepartmentCommand } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Departments<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Departments
   * @name DepartmentsList
   * @request GET:/api/Departments
   * @secure
   */
  departmentsList = (params: RequestParams = {}) =>
    this.request<DepartmentDto[], any>({
      path: `/api/Departments`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Departments
   * @name DepartmentsCreate
   * @request POST:/api/Departments
   * @secure
   */
  departmentsCreate = (data: CreateDepartmentCommand, params: RequestParams = {}) =>
    this.request<number, any>({
      path: `/api/Departments`,
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
   * @tags Departments
   * @name DepartmentsDetail
   * @request GET:/api/Departments/{id}
   * @secure
   */
  departmentsDetail = (id: number, params: RequestParams = {}) =>
    this.request<DepartmentDto, any>({
      path: `/api/Departments/${id}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Departments
   * @name DepartmentsUpdate
   * @request PUT:/api/Departments/{id}
   * @secure
   */
  departmentsUpdate = (id: number, data: UpdateDepartmentCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Departments/${id}`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Departments
   * @name DepartmentsToggleStatusPartialUpdate
   * @request PATCH:/api/Departments/{id}/toggle-status
   * @secure
   */
  departmentsToggleStatusPartialUpdate = (id: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Departments/${id}/toggle-status`,
      method: "PATCH",
      secure: true,
      ...params,
    });
}
