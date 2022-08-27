import { clients } from "@/lib/axios";
import { AxiosError } from "axios";
import { useMutation, UseMutationOptions, useQueryClient } from "react-query";
import { RecipeDto, RecipeForCreationDto } from "../types";
import { RecipeKeys } from "./recipe.keys";

const addRecipe = async (data: RecipeForCreationDto) => {
  const axios = await clients.recipeManagement();
  console.log(data);
  return axios
    .post("/recipes", data)
    .then((response) => response.data as RecipeDto);
};

export function useAddRecipe(
  options?: UseMutationOptions<RecipeDto, AxiosError, RecipeForCreationDto>
) {
  const queryClient = useQueryClient();

  return useMutation(
    (newRecipe: RecipeForCreationDto) => addRecipe(newRecipe),
    {
      onSuccess: () => {
        queryClient.invalidateQueries(RecipeKeys.lists());
      },
      ...options,
    }
  );
}
