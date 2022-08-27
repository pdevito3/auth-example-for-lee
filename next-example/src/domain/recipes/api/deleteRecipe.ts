import { clients } from "@/lib/axios";
import { AxiosError } from "axios";
import { useMutation, UseMutationOptions, useQueryClient } from "react-query";
import { RecipeKeys } from "./recipe.keys";

async function deleteRecipe(id: string) {
  const axios = await clients.recipeManagement();
  return axios.delete(`/recipes/${id}`).then(() => {});
}

export function useDeleteRecipe(
  options?: UseMutationOptions<void, AxiosError, string>
) {
  const queryClient = useQueryClient();

  return useMutation((id: string) => deleteRecipe(id), {
    onSuccess: () => {
      queryClient.invalidateQueries(RecipeKeys.lists());
      queryClient.invalidateQueries(RecipeKeys.details());
    },
    ...options,
  });
}
