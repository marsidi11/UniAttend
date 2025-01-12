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

import { LoginCommand, RefreshTokenCommand, ResetPasswordCommand } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Auth<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Auth
   * @name AuthLoginCreate
   * @request POST:/api/Auth/login
   * @secure
   */
  authLoginCreate = (data: LoginCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Auth/login`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Auth
   * @name AuthRefreshTokenCreate
   * @request POST:/api/Auth/refresh-token
   * @secure
   */
  authRefreshTokenCreate = (data: RefreshTokenCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Auth/refresh-token`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags Auth
   * @name AuthLogoutCreate
   * @request POST:/api/Auth/logout
   * @secure
   */
  authLogoutCreate = (params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Auth/logout`,
      method: "POST",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags Auth
   * @name AuthResetPasswordCreate
   * @request POST:/api/Auth/reset-password
   * @secure
   */
  authResetPasswordCreate = (data: ResetPasswordCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Auth/reset-password`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
}
