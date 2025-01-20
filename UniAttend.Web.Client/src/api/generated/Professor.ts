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

import { ProfessorDto } from "./data-contracts";
import { HttpClient, RequestParams } from "./http-client";

export class Professor<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Professor
   * @name ProfessorList
   * @request GET:/api/Professor
   * @secure
   */
  professorList = (
    query?: {
      /** @format int32 */
      departmentId?: number;
      isActive?: boolean;
    },
    params: RequestParams = {},
  ) =>
    this.request<ProfessorDto[], any>({
      path: `/api/Professor`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Professor
   * @name ProfessorDetail
   * @request GET:/api/Professor/{id}
   * @secure
   */
  professorDetail = (id: number, params: RequestParams = {}) =>
    this.request<ProfessorDto, any>({
      path: `/api/Professor/${id}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
}
