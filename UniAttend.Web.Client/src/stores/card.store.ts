import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Card, CardAssignment } from '@/types/card.types';
import { cardApi } from '@/api/endpoints/cardApi';

export const useCardStore = defineStore('card', () => {
  // State
  const cards = ref<Card[]>([]);
  const unassignedCards = ref<Card[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // Actions
  async function fetchAllCards() {
    isLoading.value = true;
    try {
      const { data } = await cardApi.getAll();
      cards.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchUnassignedCards() {
    isLoading.value = true;
    try {
      const { data } = await cardApi.getUnassigned();
      unassignedCards.value = data;
    } catch (err) {
      handleError(err);
    } finally {
      isLoading.value = false;
    }
  }

  async function assignCardToStudent(assignment: CardAssignment) {
    isLoading.value = true;
    try {
      await cardApi.assignToStudent(assignment);
      await fetchAllCards();
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function unassignCard(cardId: string) {
    isLoading.value = true;
    try {
      await cardApi.unassign(cardId);
      await fetchAllCards();
    } catch (err) {
      handleError(err);
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  function handleError(err: unknown) {
    error.value = err instanceof Error ? err.message : 'An error occurred';
  }

  return {
    // State
    cards,
    unassignedCards,
    isLoading,
    error,

    // Actions
    fetchAllCards,
    fetchUnassignedCards,
    assignCardToStudent,
    unassignCard
  };
});