import { clients } from "@/lib/axios";
import { AxiosError } from "axios";
import { useMutation, UseMutationOptions, useQueryClient } from "react-query";
import { RecipeForUpdateDto } from "../types";
import { RecipeKeys } from "./recipe.keys";

export const updateRecipe = (id: string, data: RecipeForUpdateDto) => {
  return clients.recipeManagement.put(`/recipes/${id}`, data).then(() => {});
};

export function useUpdateRecipe(
  id: string,
  options?: UseMutationOptions<void, AxiosError, RecipeForUpdateDto>
) {
  const queryClient = useQueryClient();

  return useMutation(
    (updatedRecipe: RecipeForUpdateDto) => updateRecipe(id, updatedRecipe),
    {
      onSuccess: () => {
        queryClient.invalidateQueries(RecipeKeys.lists());
        queryClient.invalidateQueries(RecipeKeys.details());
      },
      ...options,
    }
  );
}
