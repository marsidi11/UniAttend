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
  ChangePasswordCommand,
  CreateUserCommand,
  ResetPasswordCommand,
  UpdateProfileCommand,
  UpdateUserCommand,
  UserDetailsDto,
  UserDto,
  UserProfileDto,
  UserRole,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class User<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags User
   * @name UserList
   * @request GET:/api/User
   * @secure
   */
  userList = (
    query?: {
      role?: UserRole;
      isActive?: boolean;
    },
    params: RequestParams = {},
  ) =>
    this.request<UserDto[], any>({
      path: `/api/User`,
      method: "GET",
      query: query,
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserCreate
   * @request POST:/api/User
   * @secure
   */
  userCreate = (data: CreateUserCommand, params: RequestParams = {}) =>
    this.request<number, any>({
      path: `/api/User`,
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
   * @tags User
   * @name UserDetail
   * @request GET:/api/User/{id}
   * @secure
   */
  userDetail = (id: number, params: RequestParams = {}) =>
    this.request<UserDto, any>({
      path: `/api/User/${id}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserUpdate
   * @request PUT:/api/User/{id}
   * @secure
   */
  userUpdate = (id: number, data: UpdateUserCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/User/${id}`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserDeactivateCreate
   * @request POST:/api/User/{id}/deactivate
   * @secure
   */
  userDeactivateCreate = (id: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/User/${id}/deactivate`,
      method: "POST",
      secure: true,
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserProfileList
   * @request GET:/api/User/profile
   * @secure
   */
  userProfileList = (params: RequestParams = {}) =>
    this.request<UserProfileDto, any>({
      path: `/api/User/profile`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserProfileUpdate
   * @request PUT:/api/User/profile
   * @secure
   */
  userProfileUpdate = (data: UpdateProfileCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/User/profile`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserDetailsDetail
   * @request GET:/api/User/details/{id}
   * @secure
   */
  userDetailsDetail = (id: number, params: RequestParams = {}) =>
    this.request<UserDetailsDto, any>({
      path: `/api/User/details/${id}`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserDetailsList
   * @request GET:/api/User/details
   * @secure
   */
  userDetailsList = (params: RequestParams = {}) =>
    this.request<UserDetailsDto, any>({
      path: `/api/User/details`,
      method: "GET",
      secure: true,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserChangePasswordUpdate
   * @request PUT:/api/User/change-password
   * @secure
   */
  userChangePasswordUpdate = (data: ChangePasswordCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/User/change-password`,
      method: "PUT",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags User
   * @name UserForgotPasswordCreate
   * @request POST:/api/User/forgot-password
   * @secure
   */
  userForgotPasswordCreate = (data: ResetPasswordCommand, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/User/forgot-password`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
}
