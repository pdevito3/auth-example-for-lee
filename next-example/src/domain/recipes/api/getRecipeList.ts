import { clients } from "@/lib/axios";
import { PagedResponse, Pagination } from "@/types/api";
import { generateSieveSortOrder } from "@/utils/sorting";
import { AxiosResponse } from "axios";
import queryString from "query-string";
import { useQuery } from "react-query";
import { QueryParams, RecipeDto } from "../types";
import { RecipeKeys } from "./recipe.keys";

interface delayProps {
  hasArtificialDelay?: boolean;
  delayInMs?: number;
}

interface recipeListApiProps extends delayProps {
  queryString: string;
}
const getRecipes = async ({
  queryString,
  hasArtificialDelay,
  delayInMs,
}: recipeListApiProps) => {
  queryString = queryString == "" ? queryString : `?${queryString}`;

  delayInMs = hasArtificialDelay ? delayInMs : 0;

  const [json] = await Promise.all([
    clients.recipeManagement().then((axios) =>
      axios
        .get(`/recipes${queryString}`)
        .then((response: AxiosResponse<RecipeDto[]>) => {
          return {
            data: response.data as RecipeDto[],
            pagination: JSON.parse(
              response.headers["x-pagination"] ?? ""
            ) as Pagination,
          } as PagedResponse<RecipeDto>;
        })
    ),
    new Promise((resolve) => setTimeout(resolve, delayInMs)),
  ]);
  return json;
};

interface recipeListHookProps extends QueryParams, delayProps {}
export const useRecipes = ({
  pageNumber,
  pageSize,
  filters,
  sortOrder,
  hasArtificialDelay = false,
  delayInMs = 500,
}: recipeListHookProps) => {
  let sortOrderString = generateSieveSortOrder(sortOrder);
  let queryParams = queryString.stringify({
    pageNumber,
    pageSize,
    filters,
    sortOrder: sortOrderString,
  });
  // var temp = useSession();
  // console.log(temp.data.accessToken);

  return useQuery(RecipeKeys.list(queryParams ?? ""), () =>
    getRecipes({ queryString: queryParams, hasArtificialDelay, delayInMs })
  );
};
