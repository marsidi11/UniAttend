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
  AcademicYearDto,
  CreateAcademicYearCommand,
  ProblemDetails,
  UpdateAcademicYearCommand,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class AcademicYear<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags AcademicYear
   * @name AcademicYearList
   * @request GET:/api/AcademicYear
   * @secure
   */
  academicYearList = (params: RequestParams = {}) =>
    this.request<AcademicYearDto[], any>({
      path: `/api/AcademicYear`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags AcademicYear
   * @name AcademicYearCreate
   * @request POST:/api/AcademicYear
   * @secure
   */
  academicYearCreate = (data: CreateAcademicYearCommand, params: RequestParams = {}) =>
    this.request<AcademicYearDto, any>({
      path: `/api/AcademicYear`,
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
   * @tags AcademicYear
   * @name AcademicYearActiveList
   * @request GET:/api/AcademicYear/active
   * @secure
   */
  academicYearActiveList = (params: RequestParams = {}) =>
    this.request<AcademicYearDto, ProblemDetails>({
      path: `/api/AcademicYear/active`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags AcademicYear
   * @name AcademicYearUpdate
   * @request PUT:/api/AcademicYear/{id}
   * @secure
   */
  academicYearUpdate = (id: number, data: UpdateAcademicYearCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/AcademicYear/${id}`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags AcademicYear
   * @name AcademicYearCloseCreate
   * @request POST:/api/AcademicYear/{id}/close
   * @secure
   */
  academicYearCloseCreate = (id: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/AcademicYear/${id}/close`,
      method: "POST",
      secure: true,
      ...params,
    });
}
