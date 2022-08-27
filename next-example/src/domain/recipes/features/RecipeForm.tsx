import {
  PaginatedTable,
  PaginatedTableProvider,
  useGlobalFilter,
  usePaginatedTableContext,
} from "@/components/Forms";
import { createColumnHelper, SortingState } from "@tanstack/react-table";
import { useRouter } from "next/router";
import { useRecipes } from "../api";
import { RecipeDto } from "../types";

function RecipeForm() {
  const router = useRouter();
  const {
    globalFilter: globalRecipeFilter,
    queryFilter: queryFilterForRecipes,
    calculateAndSetQueryFilter: calculateAndSetQueryFilterForRecipes,
  } = useGlobalFilter((value) => `(title|visibility|directions)@=*${value}`);

  return (
    <div className="space-y-6">
      <button
        className="px-3 py-2 border border-white rounded-md"
        onClick={() => router.back()}
      >
        Back
      </button>
      <div className="">
        <div className="flex items-center justify-between">
          <h1 className="max-w-4xl text-2xl font-medium tracking-tight font-display text-slate-900 dark:text-gray-50 sm:text-4xl">
            Recipes Edit Form
          </h1>
        </div>
        <div className="py-4">
          {/* prefer this. more composed approach */}
          <PaginatedTableProvider>
            <div className="pt-2">
              <RecipeListTable queryFilter={queryFilterForRecipes} />
            </div>
          </PaginatedTableProvider>
        </div>
      </div>
    </div>
  );
}

interface RecipeListTableProps {
  queryFilter?: string | undefined;
}

function RecipeListTable({ queryFilter }: RecipeListTableProps) {
  const { sorting, pageSize, pageNumber } = usePaginatedTableContext();

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
  ];

  return (
    <PaginatedTable
      data={recipeData}
      columns={columns}
      apiPagination={recipePagination}
      entityPlural="Recipes"
      isLoading={isLoading}
      onRowClick={(row) => alert(row.id)}
    />
  );
}

export { RecipeForm };
