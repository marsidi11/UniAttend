import type { BaseEntity } from './base.types'

export interface Card extends BaseEntity {
  cardId: string
  studentId?: number
  studentName?: string
  departmentId?: number
  departmentName?: string
  isActive: boolean
  lastUsed?: Date
}

export interface CardAssignment {
  cardId: string
  studentId: number
}