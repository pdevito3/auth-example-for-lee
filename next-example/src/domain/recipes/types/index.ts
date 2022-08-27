import { SortingState } from "@tanstack/react-table";

export interface QueryParams {
  pageNumber?: number;
  pageSize?: number;
  filters?: string;
  sortOrder?: SortingState;
}

export type RecipeDto = {
  id: string;
  title: string;
  directions: string;
  visibility: string;
  recipeSourceLink: string;
  description: string;
  rating?: number;
}

export interface RecipeForManipulationDto {
  id: string;
  title: string;
  directions: string;
  visibility: string;
  recipeSourceLink: string;
  description: string;
  rating?: number;
}

export interface RecipeForCreationDto extends RecipeForManipulationDto { }
export interface RecipeForUpdateDto extends RecipeForManipulationDto { }

// need a string enum list?
// const StatusList = ['Status1', 'Status2', null] as const;
// export type Status = typeof StatusList[number];
// Then use as --> status: Status;
