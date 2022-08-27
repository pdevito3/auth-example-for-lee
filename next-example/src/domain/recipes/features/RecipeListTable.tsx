import { PaginatedTable, usePaginatedTableContext } from "@/components/Forms";
import { RecipeDto } from "@/domain/recipes/types";
import { TrashIcon } from "@heroicons/react/24/outline";
import "@tanstack/react-table";
import { createColumnHelper, SortingState } from "@tanstack/react-table";
import { useRouter } from "next/router";
import toast from "react-hot-toast";
import { useDeleteRecipe, useRecipes } from "../api";

interface RecipeListTableProps {
  queryFilter?: string | undefined;
}

export function RecipeListTable({ queryFilter }: RecipeListTableProps) {
  const router = useRouter();
  const { sorting, pageSize, pageNumber } = usePaginatedTableContext();

  const deleteRecipeApi = useDeleteRecipe();
  function deleteRecipe(id: string) {
    deleteRecipeApi
      .mutateAsync(id)
      .then(() => {
        // TODO are you sure modal *****************************************
        toast.success("Recipe deleted successfully");
      })
      .catch((e) => {
        toast.error("There was an error deleting the recipe");
        console.error(e);
      });
  }

  const { data: recipeResponse, isLoading } = useRecipes({
    sortOrder: sorting as SortingState,
    pageSize,
    pageNumber,
    filters: queryFilter,
    hasArtificialDelay: true,
  });
  const recipeData = recipeResponse?.data;
  const recipePagination = recipeResponse?.pagination;

  const columnHelper = createColumnHelper<RecipeDto>();
  const columns = [
    columnHelper.accessor((row) => row.title, {
      id: "title",
      cell: (info) => <p className="">{info.getValue()}</p>,
      header: () => <span className="">Title</span>,
    }),
    columnHelper.accessor((row) => row.visibility, {
      id: "visibility",
      cell: (info) => <p className="">{info.getValue()}</p>,
      header: () => <span className="">Visibility</span>,
    }),
    columnHelper.accessor((row) => row.directions, {
      id: "directions",
      cell: (info) => <p className="">{info.getValue()}</p>,
      header: () => <span className="">Directions</span>,
    }),
    columnHelper.accessor((row) => row.rating, {
      id: "rating",
      cell: (info) => <p className="">{info.getValue()}</p>,
      header: () => <span className="">Rating</span>,
    }),
    columnHelper.accessor("id", {
      enableSorting: false,
      meta: { thClassName: "w-10" },
      cell: (row) => (
        <div className="flex items-center justify-center w-full">
          <button
            onClick={(e) => {
              deleteRecipe(row.getValue());
              e.stopPropagation();
            }}
            className="inline-flex items-center px-1 py-2 text-sm font-medium leading-5 transition duration-100 ease-in bg-white border border-gray-300 rounded-full shadow-sm hover:bg-red-200 hover:text-red-800 hover:outline-none dark:border-slate-900 dark:bg-slate-800 dark:text-white dark:hover:bg-red-800 dark:hover:text-red-300 dark:hover:outline-none sm:px-3 sm:py-1 sm:opacity-0 sm:group-hover:opacity-100 dark:hover:shadow dark:shadow-red-400 dark:hover:shadow-red-300"
          >
            <TrashIcon className="w-4 h-4" />
          </button>
        </div>
      ),
      header: () => <span className=""></span>,
    }),
  ];

  return (
    <PaginatedTable
      data={recipeData}
      columns={columns}
      apiPagination={recipePagination}
      entityPlural="Recipes"
      isLoading={isLoading}
      onRowClick={(row) => router.push(`/recipes/${row.id}`)}
    />
  );
}
