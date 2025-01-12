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

import { RefreshTokenCommand } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class RefreshToken<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags RefreshToken
   * @name RefreshTokenRefreshCreate
   * @request POST:/api/RefreshToken/refresh
   * @secure
   */
  refreshTokenRefreshCreate = (data: RefreshTokenCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/RefreshToken/refresh`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
}
